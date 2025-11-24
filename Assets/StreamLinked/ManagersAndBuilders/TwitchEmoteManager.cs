using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.Chat;
using ScoredProductions.StreamLinked.EventSub.Events;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.IRC.Tags;
using ScoredProductions.StreamLinked.ManagersAndBuilders.Containers;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.Networking;

namespace ScoredProductions.StreamLinked.ManagersAndBuilders {

	/// <summary>
	/// Twitch emote downloader and manager.
	/// </summary>
	public class TwitchEmoteManager : SingletonInstance<TwitchEmoteManager> {

		public DebugManager.DebugLevel LogDebugLevel = DebugManager.DebugLevel.Normal;

		[SerializeField]
		private bool persistBetweenScenes = true;
		public override bool PersistBetweenScenes => this.persistBetweenScenes;

		public List<TwitchEmote> DownloadedEmotes { get; private set; } = new List<TwitchEmote>();

		public bool DownloaderWorking => this.threadsWorking.ContainsValue(true);

		private readonly Dictionary<int, bool> threadsWorking = new Dictionary<int, bool>();

		public TwitchEmote.EmoteSize EmoteSize;
		public TwitchEmote.ThemeMode EmoteThemeMode;

		[Range(1, 100)]
		[Tooltip("The amount of coroutines to work in a Frame to aquire emote data. e.g 4 = 4 coroutines downloading a single emote per frame")]
		public int NumWorkersToCreateOnAwake = 4;

		public bool Busy => this.CurrentWaitingItems > 0 || this.DownloaderWorking;

		public int CurrentWaitingItems => this.downloadQueue.Count;

		private CancellationTokenSource RequestAPICancellationToken;

		private readonly Queue<TwitchEmote> downloadQueue = new Queue<TwitchEmote>();

		private WaitUntil downloadsWaiting;

		private readonly List<Coroutine> downloaderCoroutines = new List<Coroutine>();

		private IEnumerator DownloaderService(int threadNum) {
			if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
				DebugManager.LogMessage($"Emote Downloader Coroutine Started. Num: {threadNum}".RichTextBold());
			}

			Start:
			this.threadsWorking[threadNum] = false;
			yield return this.downloadsWaiting;
			this.threadsWorking[threadNum] = true;

			while (this.downloadQueue.TryDequeue(out TwitchEmote emote)) {
				if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
					DebugManager.LogMessage($"Starting download or {emote.ID}".RichTextColour("blue"));
				}
				yield return this.DownloadEmoteFromWeb(emote);
			}
			goto Start;
		}


		public IEnumerator DownloadEmoteFromWeb(TwitchEmote emote) {
			string url = emote.CreateImageURL(this.EmoteThemeMode, this.EmoteSize);

			if (!string.IsNullOrEmpty(url)) {
				UnityWebRequest www = UnityWebRequest.Get(url);
				yield return www.SendWebRequest();

				if (www.result == UnityWebRequest.Result.Success) {
					if (emote.DownloadedTexture == null) {
						emote.DownloadedTexture = new List<Texture2D>();
					}
					else {
						emote.DownloadedTexture.Clear();
					}

					if (www.downloadHandler.data.IsGIF()) {
						(List<UniGif.UniGif.GifTexture>, int, int, int) processedGif = UniGif.UniGif.GetTextureList(www.downloadHandler.data);
						int frameCount = processedGif.Item1.Count;
						float smallestDelay = 0;
						Span<float> frameDelays = stackalloc float[frameCount];

						for (int x = 0; x < processedGif.Item1.Count; x++) {
							float value = processedGif.Item1[x].m_delaySec;
							frameDelays[x] = value;

							if (smallestDelay == 0 || value < smallestDelay) {
								smallestDelay = value;
							}
						}

						for (int x = 0; x < processedGif.Item1.Count; x++) {
							// Add first frame regardless
							emote.DownloadedTexture.Add(processedGif.Item1[x].m_texture2d);

							int delayMultiple = (int)(frameDelays[x] / smallestDelay); // Equalise frame rate

							if (delayMultiple > 1) {
								// 0 already added, add greater than index 0 here
								for (int y = 1; y < delayMultiple; y++) {
									emote.DownloadedTexture.Add(processedGif.Item1[x].m_texture2d);
								}
							}
						}

						// Smallest delay after frame equalising is closest value to get framerate
						// (1 second / smallest frame delay = framerate)
						emote.FrameRate = 1.0f / smallestDelay;
					}
					else {
						Texture2D texture = new Texture2D(1, 1);
						texture.LoadImage(www.downloadHandler.data);

						emote.DownloadedTexture.Add(texture);
					}

					emote.DownloadedTheme = this.EmoteThemeMode;
					emote.DownloadedSize = this.EmoteSize;
					if (!this.DownloadedEmotes.Contains(emote)) {
						this.DownloadedEmotes.Add(emote);
					}
				} else {
					DebugManager.LogMessage(www.error, DebugManager.ErrorLevel.Error);
				}
				www.Dispose();
			}
		}

		/// <summary>
		/// Returns the TMP text of the provided emotes, List contents are unique and not duplicated if duplicates are found in the provided values.
		/// </summary>
		/// <param name="EmotesToGet"></param>
		/// <returns></returns>
		public List<(string emoteName, string textmeshpro)> GetEmotesTMPText(SortedSet<EmotePosition> EmotesToGet) {
			EmotePosition[] emotePositions = new EmotePosition[EmotesToGet.Count];
			EmotesToGet.CopyTo(emotePositions, 0);

			return this.GetEmotesTMPText(emotePositions);
		}

		/// <summary>
		/// Returns the TMP text of the provided emotes, List contents are unique and not duplicated if duplicates are found in the provided values.
		/// </summary>
		/// <param name="EmotesToGet"></param>
		/// <returns></returns>
		public List<(string emoteName, string textmeshpro)> GetEmotesTMPText(params EmotePosition[] EmotesToGet) {
			int len = EmotesToGet.Length;
			List<(string emoteName, string textmeshpro)> returnedEmotes = new List<(string emoteName, string textmeshpro)>(len);

			for (int x = 0; x < len; x++) {
				EmotePosition ep = EmotesToGet[x];
				bool isUnique = true;
				for (int y = 0; y < returnedEmotes.Count; y++) {
					if (returnedEmotes[y].emoteName == ep.EmoteId) {
						isUnique = false;
						break;
					}
				}
				if (!isUnique) {
					continue;
				}

				string emoteName = ep.EmoteId;
				bool found = false;
				for (int y = 0; y < this.DownloadedEmotes.Count; y++) {
					TwitchEmote emote = this.DownloadedEmotes[y];
					if (emote.ID.Equals(emoteName) && emote.IsDownloaded(this.EmoteSize, this.EmoteThemeMode)) {
						returnedEmotes.Add((emoteName, emote.TextmeshFormating));
						emote.TimesCalled++;
						found = true;
						break;
					}
				}
				if (!found && this.LogDebugLevel == DebugManager.DebugLevel.Full) {
					DebugManager.LogMessage($"Emote {{{emoteName}}} was not found in the system".RichTextColour("red"));
				}
			}

			return returnedEmotes;
		}

		public List<TwitchEmote> SearchEmotes(string[] searchEmotes) {
			int len = searchEmotes.Length;
			List<TwitchEmote> foundEmotes = new List<TwitchEmote>(len);

			for (int x = 0; x < len; x++) {
				string search = searchEmotes[x];
				if (string.IsNullOrWhiteSpace(search)) {
					continue;
				}
				bool isUnique = true;
				for (int y = 0; y < foundEmotes.Count; y++) {
					if (foundEmotes[y].ID == search) {
						isUnique = false;
						break;
					}
				}
				if (!isUnique) {
					continue;
				}

				bool found = false;
				for (int y = 0; y < this.DownloadedEmotes.Count; y++) {
					TwitchEmote emote = this.DownloadedEmotes[y];
					if (emote.ID.Equals(search) && emote.IsDownloaded(this.EmoteSize, this.EmoteThemeMode)) {
						foundEmotes.Add(emote);
						emote.TimesCalled++;
						found = true;
						break;
					}
				}
				if (!found && this.LogDebugLevel == DebugManager.DebugLevel.Full) {
					DebugManager.LogMessage($"Emote {{{search}}} was not found in the system".RichTextColour("red"));
				}
			}
			return foundEmotes;
		}

		protected override void Awake() {
			if (this.EstablishSingleton(true)) {
				TwitchAPIClient.CreateOrGetInstance(out _);

				this.downloadsWaiting = new WaitUntil(() => this.downloadQueue.Count > 0);

				if (this.NumWorkersToCreateOnAwake < 1) {
					this.NumWorkersToCreateOnAwake = 1;
				}
				if (this.NumWorkersToCreateOnAwake > 100) {
					this.NumWorkersToCreateOnAwake = 100;
				}

				for (int x = 0; x < this.NumWorkersToCreateOnAwake; x++) {
					this.downloaderCoroutines.Add(this.StartCoroutine(this.DownloaderService(x)));
				}

				Task.Run(this.GetGlobalChatEmotes);
			}
		}

		private void OnEnable() {
			this.RequestAPICancellationToken ??= new CancellationTokenSource();
		}

		private void OnDestroy() {
			this.EndFunctionality();
		}

		private void OnDisable() {
			this.EndFunctionality();
		}

		protected override void OnApplicationQuit() {
			this.EndFunctionality();
		}

		private void EndFunctionality() {
			foreach (Coroutine worker in this.downloaderCoroutines) {
				if (worker != null) {
					this.StopCoroutine(worker);
				}
			}

			this.threadsWorking?.Clear();

			if (this.RequestAPICancellationToken != null) {
				try {
					this.RequestAPICancellationToken.Cancel();
					this.RequestAPICancellationToken.Dispose();
					this.RequestAPICancellationToken = null;
				} catch {
					if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
						DebugManager.LogMessage("RequestAPICancellationToken was Canceled".RichTextColour("orange"));
					}
				}
			}
		}

		public async Task GetGlobalChatEmotes() {
			TwitchAPIDataContainer<GetGlobalEmotes> returnedData
				= await TwitchAPIClient.MakeTwitchAPIRequestAsync<GetGlobalEmotes>(
					cancelToken: this.RequestAPICancellationToken.Token);
			if (!returnedData.HasErrored) {
				GetGlobalEmotes[] emoteData = returnedData.data;

				foreach (GetGlobalEmotes emote in emoteData) {
					this.QueueEmoteDownload(emote.id, emote.name, true);
				}
			}
			else {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("GetGlobalChatEmotes failed to get emotes.");
				}
			}
		}
		
		public async Task GetChannelChatEmotes(string broadcasterId) {
			TwitchAPIDataContainer<GetChannelEmotes> returnedData
				= await TwitchAPIClient.MakeTwitchAPIRequestAsync<GetChannelEmotes>(
					QueryParameters: new (string, string)[] { (GetChannelEmotes.BROADCASTER_ID, broadcasterId) },
					cancelToken: this.RequestAPICancellationToken.Token);
			if (!returnedData.HasErrored) {
				GetChannelEmotes[] emoteData = returnedData.data;

				foreach (GetChannelEmotes emote in emoteData) {
					this.QueueEmoteDownload(emote.id, emote.name, false);
				}
			}
			else {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("GetGlobalChatEmotes failed to get emotes.");
				}
			}
		}

		public void QueueEmoteDownload(string ID, string Name = null, bool IsGlobal = false) {
			if (string.IsNullOrWhiteSpace(ID)) {
				return;
			}

			TwitchEmote download = null;
			for (int x = 0; x < this.DownloadedEmotes.Count; x++) { // Try to find existing emote
				TwitchEmote index = this.DownloadedEmotes[x];
				if (index.ID == ID) {
					download = index;
					break;
				}
			}
			if ((download == null || !download.IsDownloaded(this.EmoteSize, this.EmoteThemeMode)) // If emote not found or not downloaded
				&& !this.downloadQueue.Contains(download)) {
				download ??= new TwitchEmote();
				download.ID = ID;
				download.Name = Name;
				download.IgnoreEmote = false;
				download.IsGlobal = IsGlobal;
				download.LastAccessTime = DateTime.Now;

				this.downloadQueue.Enqueue(download);
			}
			else if (download != null) {
				download.IgnoreEmote = false;
			}
		}

		public void QueueEmoteDownload(GetGlobalEmotes globalEmote) {
			if (string.IsNullOrWhiteSpace(globalEmote.id)) {
				return;
			}
			this.QueueEmoteDownload(globalEmote.id, globalEmote.name);
		}

		public void QueueEmoteDownload(GetChannelEmotes channelEmote) {
			if (string.IsNullOrWhiteSpace(channelEmote.id)) {
				return;
			}
			this.QueueEmoteDownload(channelEmote.id, channelEmote.name);
		}

		/// <summary>
		/// EventSub Messages
		/// </summary>
		/// <param name="messageData"></param>
		public void QueueEmoteDownload(Message messageData) {
			foreach (Emote e in messageData.emotes) {
				if (string.IsNullOrWhiteSpace(e.id)) {
					continue;
				}
				this.QueueEmoteDownload(e.id, e.name);
			}

			foreach (Fragment f in messageData.fragments) {
				if (string.IsNullOrWhiteSpace(f.emote.id)) {
					continue;
				}
				this.QueueEmoteDownload(f.emote.id, f.emote.name);
			}
		}
	}
}