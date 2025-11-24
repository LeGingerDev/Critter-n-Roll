using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.API.Chat;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.API.Users;
using ScoredProductions.StreamLinked.IRC.Tags;
using ScoredProductions.StreamLinked.ManagersAndBuilders.Containers;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.Networking;

namespace ScoredProductions.StreamLinked.ManagersAndBuilders {

	/// <summary>
	/// Twitch badge downloader and manager.
	/// </summary>
	public class TwitchBadgeManager : SingletonDispatcher<TwitchBadgeManager> {

		/// <summary>
		/// A non web address compatible name as Twitch user names are translated to links for their channel
		/// </summary>
		public const string GlobalBadgeName = "*Global"; // * to make invalid channel name as 'Global' is itself a channel name

		public readonly static string[] CustomBadgeNames = new string[] { "subscriber", "cheer" };

		[SerializeField]
		private bool persistBetweenScenes = true;
		public override bool PersistBetweenScenes => this.persistBetweenScenes;

		public Dictionary<long, List<TwitchBadge>> RoomBadges { get; } = new Dictionary<long, List<TwitchBadge>>();

		public List<GetUsers> AquiredRooms { get; } = new List<GetUsers>(); // Reduce calls to Twitch API, if we already have it then dont get it again

		public bool DownloaderWorking => this.threadsWorking.ContainsValue(true);

		[Range(1, 100)]
		[Tooltip("The amount of coroutines to work in a Frame to aquire badge data. e.g 4 = 4 coroutines downloading a single badge per frame")]
		public int NumWorkersToCreateOnAwake = 4;

		public TwitchBadge.BadgeSize BadgeSize = TwitchBadge.BadgeSize.Three;

		public bool Busy => this.CurrentWaitingItems > 0 || this.DownloaderWorking;

		public int CurrentWaitingItems => this.downloadQueue.Count;


		private readonly Dictionary<int, bool> threadsWorking = new Dictionary<int, bool>();

		private readonly List<Coroutine> downloaderCoroutines = new List<Coroutine>();

		private readonly Queue<TwitchBadge> downloadQueue = new Queue<TwitchBadge>();

		private CancellationTokenSource RequestAPICancellationToken;

		public IEnumerator DownloaderService(int threadNum) {
			WaitUntil downloadsWaiting = new WaitUntil(() => this.downloadQueue.Count > 0);

			if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
				DebugManager.LogMessage($"Badge Downloader Coroutine Started. Num: {threadNum}".RichTextBold());
			}

			Start:
			this.threadsWorking[threadNum] = false;
			yield return downloadsWaiting;
			this.threadsWorking[threadNum] = true;

			while (this.downloadQueue.TryDequeue(out TwitchBadge badge)) {
				if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
					DebugManager.LogMessage($"Starting download of {badge.Set_ID}".RichTextColour("blue"));
				}
				yield return this.DownloadBadgesFromWeb(badge);
			}

			goto Start;
		}

		public IEnumerator DownloadBadgesFromWeb(TwitchBadge badge) {
			for (int x = 0; x < badge.Versions.Count; x++) {
				TwitchBadge.Version version = badge.Versions[x];
				string url = version.DownloadURL(badge.Size);

				if (!string.IsNullOrEmpty(url)) {
					UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
					yield return www.SendWebRequest();
					if (www.result == UnityWebRequest.Result.Success) {
						version.ImageData = DownloadHandlerTexture.GetContent(www);
					}
					else {
						DebugManager.LogMessage(www.error, DebugManager.ErrorLevel.Error);
					}
					www.Dispose();
				}
			}

			if (this.RoomBadges.ContainsKey(badge.Room_Id)) {
				this.RoomBadges[badge.Room_Id].Add(badge);
			}
			else {
				this.RoomBadges.Add(badge.Room_Id, new List<TwitchBadge>(1) { badge });
			}
		}

		public string GetBadgeTMPText(BadgeData[] messageBadges, long room_id) {
			List<TwitchBadge> channelBadges = null;

			bool globalExists = this.RoomBadges.TryGetValue(TwitchBadge.GlobalRoomValue, out List<TwitchBadge> globalBadges);
			bool channelExists = false;
			if (room_id != TwitchBadge.GlobalRoomValue) {
				channelExists = this.RoomBadges.TryGetValue(room_id, out channelBadges);
			}

			if (!channelExists && !globalExists) {
				return string.Empty;
			}

			int len = messageBadges.Length;
			string returnedBadges = "";

			for (int x = 0; x < len;) {
				BadgeData data = messageBadges[x];
				if (channelExists && this.CheckBadgeCanHaveCustomVariants(data.Name)) { // Check channel badges if its a custom badge
					for (int y = 0; y < channelBadges.Count; y++) {
						TwitchBadge download = channelBadges[y];

						if (!download.IgnoreBadge && download.Set_ID == data.Name && download.IsDownloaded(this.BadgeSize)) {
							for (int v = 0; v < download.Versions.Count; v++) {
								TwitchBadge.Version version = download.Versions[v];
								if (version.id == data.Value.ToString()) {
									returnedBadges += download.TextmeshFormating(version);
									goto NextMessage; // Jump the whole process to the next badge
								}
							}
						}
					}
				}

				if (globalExists) {
					for (int y = 0; y < globalBadges.Count; y++) {
						TwitchBadge download = globalBadges[y];

						if (!download.IgnoreBadge && download.Set_ID == data.Name && download.IsDownloaded(this.BadgeSize)) {
							for (int v = 0; v < download.Versions.Count; v++) {
								TwitchBadge.Version version = download.Versions[v];
								if (version.id == data.Value.ToString()) {
									returnedBadges += download.TextmeshFormating(version);
									goto NextMessage;
								}
							}
						}
					}
				}

				NextMessage:
				x++;
			}

			return returnedBadges;
		}

		public TwitchBadge[] GetBadges(string[] searchBadges, long room_id) {
			List<TwitchBadge> globalBadges = null;
			List<TwitchBadge> channelBadges = null;

			bool globalExists = this.RoomBadges.TryGetValue(TwitchBadge.GlobalRoomValue, out globalBadges);
			bool channelExists = false;
			if (room_id != TwitchBadge.GlobalRoomValue) {
				channelExists = this.RoomBadges.TryGetValue(room_id, out channelBadges);
			}

			int len = searchBadges.Length;
			int index = 0;
			TwitchBadge[] badges = new TwitchBadge[len];

			for (int x = 0; x < len;) {
				string name = searchBadges[x];

				if (this.CheckBadgeCanHaveCustomVariants(name) && channelExists) { // Check channel badges if its a custom badge
					for (int y = 0; y < channelBadges.Count; y++) {
						TwitchBadge download = channelBadges[y];

						if (!download.IgnoreBadge && download.Set_ID == name && download.IsDownloaded(this.BadgeSize)) {
							badges[index++] = download;
							goto Next;
						}
					}
				}

				if (globalExists) {
					for (int y = 0; y < globalBadges.Count; y++) {
						TwitchBadge download = globalBadges[y];

						if (!download.IgnoreBadge && download.Set_ID == name && download.IsDownloaded(this.BadgeSize)) {
							badges[index++] = download;
							goto Next;
						}
					}
				}

				DebugManager.LogMessage($"Badge {{{name}}} was not found in the system".RichTextColour("red"));
				Next:
				x++;
			}
			Array.Resize(ref badges, index);
			return badges;
		}

		private TwitchBadgeManager() { }

		protected override void Awake() {
			if (base.EstablishSingleton(true)) {
				TwitchAPIClient.CreateOrGetInstance(out _);

				this.RestartCoroutineThreads();

				Task.Run(this.GetGlobalBadges);
			}
		}

		private void OnEnable() {
			this.RequestAPICancellationToken ??= new CancellationTokenSource();
		}

		private void OnDisable() {
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

		public void RestartCoroutineThreads() {
			this.StopAllCoroutines();

			if (this.NumWorkersToCreateOnAwake < 1) {
				this.NumWorkersToCreateOnAwake = 1;
			}
			if (this.NumWorkersToCreateOnAwake > 100) {
				this.NumWorkersToCreateOnAwake = 100;
			}

			this.downloaderCoroutines.Clear();
			for (int x = 0; x < this.NumWorkersToCreateOnAwake; x++) {
				this.downloaderCoroutines.Add(this.StartCoroutine(this.DownloaderService(x)));
			}

		}

		public async Task GetGlobalBadges() {
			TwitchAPIDataContainer<GetGlobalChatBadges> returnedData
				= await TwitchAPIClient.MakeTwitchAPIRequestAsync<GetGlobalChatBadges>(
					cancelToken: this.RequestAPICancellationToken.Token);
			if (!returnedData.HasErrored) {
				GetGlobalChatBadges[] badgeData = returnedData.data;

				foreach (GetGlobalChatBadges badge in badgeData) {
					TwitchBadge newBadge = new TwitchBadge() {
						Set_ID = badge.set_id,
						LastAccessTime = DateTime.Now,
						Size = this.BadgeSize,
						Versions = new List<TwitchBadge.Version>(),
						AssociatedChannel = GlobalBadgeName,
						IsGlobal = true,
					};

					foreach (BadgeVersion version in badge.versions) {
						newBadge.Versions.Add(new TwitchBadge.Version() {
							id = version.id,
							image_url_1x = version.image_url_1x,
							image_url_2x = version.image_url_2x,
							image_url_4x = version.image_url_4x,
							title = version.title,
							description = version.description,
							click_action = version.click_action,
							click_url = version.click_url,
						});
					}

					this.QueueBadgeDownload(newBadge);
				}
			}
			else {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("GetGlobalBadges failed to get badges.");
				}
			}
		}

		public void GetChannelBadges(long room_id, bool force = false, TokenInstance credentials = null) {
			MainThreadDispatchQueue.Enqueue(() => this.StartCoroutine(this.InternalCoroutineGetChannelBadges(room_id, force, credentials)));
		}

		public void GetChannelBadges(GetUsers user, bool force = false, TokenInstance credentials = null) {
			MainThreadDispatchQueue.Enqueue(() => this.StartCoroutine(this.InternalCoroutineGetChannelBadges(user, force, credentials)));
		}

		private IEnumerator InternalCoroutineGetChannelBadges(long room_id, bool force = false, TokenInstance credentials = null) {
			bool dataSuccess = false;
			GetUsers userData = default;

			void UserCallback(TwitchAPIDataContainer<GetUsers> data) {
				if (data.HasErrored) {
					DebugManager.LogMessage(data.RawResponse, DebugManager.ErrorLevel.Error);
				}
				else {
					dataSuccess = true;
					userData = data.data[0];

					bool found = false;
					for (int x = 0; x < this.AquiredRooms.Count; x++) {
						GetUsers u = this.AquiredRooms[x];
						if (userData.id == u.id) {
							found = true;
							break;
						}
					}
					if (!found) {
						force = true;
						this.AquiredRooms.Add(userData);
					}
				}
			}

			for (int x = 0; x < this.AquiredRooms.Count; x++) {
				GetUsers u = this.AquiredRooms[x];
				if (u.id == room_id.ToString()) {
					userData = u;
					break;
				}
			}

			if (string.IsNullOrWhiteSpace(userData.id)) {
				yield return TwitchAPIClient.MakeTwitchAPIRequest<GetUsers>(
							Credentials: credentials,
							QueryParameters: new (string, string)[] {
								(GetUsers.ID, room_id.ToString())
							},
							SuccessCallback: UserCallback);

				if (!dataSuccess) {
					yield break;
				}
			}
			if (force) {
				yield return this.InternalCoroutineGetChannelBadges(userData, force, credentials);
			}
		}

		private IEnumerator InternalCoroutineGetChannelBadges(GetUsers user, bool force = false, TokenInstance credentials = null) {
			bool found = false;
			for (int x = 0; x < this.AquiredRooms.Count; x++) {
				GetUsers u = this.AquiredRooms[x];
				if (user.id == u.id) {
					found = true;
					break;
				}
			}
			if (!found) {
				force = true;
				this.AquiredRooms.Add(user);
			}
			if (!force) {
				yield break;
			}

			bool dataSuccess = false;
			GetChannelChatBadges[] badgeData = null;

			void BadgesCallback(TwitchAPIDataContainer<GetChannelChatBadges> data) {
				if (data.HasErrored) {
					DebugManager.LogMessage(data.RawResponse, DebugManager.ErrorLevel.Error);
				}
				else {
					dataSuccess = true;
					badgeData = data.data;
				}
			}

			yield return TwitchAPIClient.MakeTwitchAPIRequest<GetChannelChatBadges>(
						Credentials: credentials,
						QueryParameters: new (string, string)[] {
							(GetChannelChatBadges.BROADCASTER_ID, user.id)
						},
						SuccessCallback: BadgesCallback);

			if (!dataSuccess) {
				yield break;
			}

			try {
				long room_id = long.Parse(user.id);
				if (!this.RoomBadges.TryGetValue(room_id, out List<TwitchBadge> downloadedBadges)) {
					downloadedBadges = new List<TwitchBadge>();
					this.RoomBadges[room_id] = downloadedBadges;
				}

				for (int x = 0; x < badgeData.Length; x++) {
					GetChannelChatBadges receivedBadge = badgeData[x];
					TwitchBadge currentBadge = null;
					for (int y = 0; y < downloadedBadges.Count; y++) {
						TwitchBadge existingBadge = downloadedBadges[y];
						if (existingBadge.Set_ID == receivedBadge.set_id) {
							currentBadge = existingBadge;
							break;
						}
					}

					if (currentBadge == null) {
						currentBadge = new TwitchBadge() {
							Set_ID = receivedBadge.set_id,
							LastAccessTime = DateTime.Now,
							Size = this.BadgeSize,
							Versions = new List<TwitchBadge.Version>(),
							AssociatedChannel = user.login,
							Room_Id = room_id,
						};
						foreach (BadgeVersion version in receivedBadge.versions) {
							currentBadge.Versions.Add(new TwitchBadge.Version() {
								id = version.id,
								image_url_1x = version.image_url_1x,
								image_url_2x = version.image_url_2x,
								image_url_4x = version.image_url_4x,
								title = version.title,
								description = version.description,
								click_action = version.click_action,
								click_url = version.click_url,
							});
						}
						this.QueueBadgeDownload(currentBadge);
					}
					else {
						currentBadge.LastAccessTime = DateTime.Now;
						currentBadge.Size = this.BadgeSize;
						currentBadge.AssociatedChannel = user.login;
						currentBadge.Room_Id = room_id;
						foreach (BadgeVersion version in receivedBadge.versions) {
							bool exists = false;
							foreach (TwitchBadge.Version existingVersion in currentBadge.Versions) {
								if (version.id == existingVersion.id) {
									existingVersion.image_url_1x = version.image_url_1x;
									existingVersion.image_url_2x = version.image_url_2x;
									existingVersion.image_url_4x = version.image_url_4x;
									existingVersion.title = version.title;
									existingVersion.description = version.description;
									existingVersion.click_action = version.click_action;
									existingVersion.click_url = version.click_url;

									exists = true;
									break;
								}
							}
							if (!exists) {
								currentBadge.Versions.Add(new TwitchBadge.Version() {
									id = version.id,
									image_url_1x = version.image_url_1x,
									image_url_2x = version.image_url_2x,
									image_url_4x = version.image_url_4x,
									title = version.title,
									description = version.description,
									click_action = version.click_action,
									click_url = version.click_url,
								});
							}
						}
						if (force) {
							this.QueueBadgeDownload(currentBadge);
						}
					}

				}
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}
		}

		public bool CheckBadgeCanHaveCustomVariants(string badge) {
			for (int x = 0; x < CustomBadgeNames.Length; x++) {
				if (badge.Contains(CustomBadgeNames[x])) {
					return true;
				}
			}
			return false;
		}

		public int GetBadgeCount() {
			int num = 0;

			foreach (List<TwitchBadge> badges in this.RoomBadges.Values) {
				num += badges?.Count ?? 0;
			}

			return num;
		}

		public void QueueBadgesDownloads(params TwitchBadge[] badges) {
			for (int x = 0; x < badges.Length; x++) {
				this.QueueBadgeDownload(badges[x]);
			}
		}

		public void QueueBadgeDownload(TwitchBadge badge) {
			badge.IgnoreBadge = false;
			if (badge.Versions != null && badge.IsDownloaded(this.BadgeSize)) {
				return;
			}

			if (!this.downloadQueue.Contains(badge)) {
				this.downloadQueue.Enqueue(badge);
			}
		}
	}
}