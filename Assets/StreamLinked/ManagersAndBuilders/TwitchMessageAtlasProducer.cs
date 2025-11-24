using System;
using System.Collections;
using System.Collections.Generic;

using ScoredProductions.Assets.StreamLinked.ManagersAndBuilders.Containers;
using ScoredProductions.StreamLinked.IRC;
using ScoredProductions.StreamLinked.ManagersAndBuilders.Containers;
using ScoredProductions.StreamLinked.Utility;

using TMPro;

using UnityEngine;
using UnityEngine.TextCore;

namespace ScoredProductions.StreamLinked.ManagersAndBuilders {

	/// <summary>
	/// Atlas builder for TextMeshPro objects.Builds atlas from Badges and Emotes from their respective Singleton Managers.
	/// </summary>
	public class TwitchMessageAtlasProducer : SingletonInstance<TwitchMessageAtlasProducer> {

		public bool DebugMessages = false;

		[SerializeField]
		private bool persistBetweenScenes = true;
		public override bool PersistBetweenScenes => this.persistBetweenScenes;

		[Tooltip("Requires GameObject Restart to take effect")]
		[Range(1, 10)]
		public int NumberOfThreads = 5;

		public static event EventHandler<Guid> OnAtlasRequestComplete = delegate { };

		private readonly HashSet<CoroutineWrapper> atlasConstructors = new HashSet<CoroutineWrapper>();

		private readonly WaitForSecondsRealtime Wait1Second = new WaitForSecondsRealtime(1);

		private readonly Queue<QueueContainer> AtlasWorkQueue = new Queue<QueueContainer>();
		private readonly Dictionary<Guid, TMP_SpriteAsset> SpriteProducedList = new Dictionary<Guid, TMP_SpriteAsset>();

		private TwitchEmoteManager emotesManager;
		private TwitchBadgeManager badgeManager;

		public int NumberOfItemsWaiting => this.AtlasWorkQueue.Count;
		public int ItemsAwaitingRetrieval => this.SpriteProducedList.Count;

		private void OnEnable() {
			if (this.atlasConstructors.Count != this.NumberOfThreads) {
				this.atlasConstructors.Clear();

				for (int x = 0; x < this.NumberOfThreads; x++) {
					CoroutineWrapper wrapper = new CoroutineWrapper();
					this.atlasConstructors.Add(wrapper);
					wrapper.Coroutine = this.StartCoroutine(this.AtlasProducerCoroutine(wrapper));
				}
			}
		}

		private void OnDestroy() {
			this.EndFunctionality();
		}

		private void EndFunctionality() {
			this.atlasConstructors.Clear();
			OnAtlasRequestComplete = null;
		}

		// Try adding emote search by TMP name
		private bool WaitForBuilders() {
			bool emotesExist = TwitchEmoteManager.GetInstance(out this.emotesManager);
			bool badgesExist = TwitchBadgeManager.GetInstance(out this.badgeManager);
			if (!emotesExist && !badgesExist) {
				return true;
			}
			else if ((!emotesExist || !this.emotesManager.Busy) && (!badgesExist || !this.badgeManager.Busy)) {
				return false;
			}
			return true;
		}

		public Guid EnqueueAtlasBuild(TwitchMessage message) {
			return this.EnqueueAtlasBuild(message.GetEmoteNames(), message.GetBadgeNames(out long room_id), room_id);
		}

		public Guid EnqueueAtlasBuild(string[] emotes, string[] badges, long room_id) {
			Guid id = Guid.NewGuid();
			this.AtlasWorkQueue.Enqueue(new QueueContainer() { ID = id, Emotes = emotes, Badges = badges, RoomId = room_id });
			if (this.DebugMessages) {
				DebugManager.LogMessage($"(TwitchMessageAtlasProducer) Build Queued ID:[{id}] Emotes:[{string.Join(",", emotes)}] Badges:[{string.Join(",", badges)}]");
			}
			return id;
		}

		public bool TryGetAtlas(Guid id, out TMP_SpriteAsset foundAsset) {
			bool found = this.SpriteProducedList.TryGetValue(id, out foundAsset) && foundAsset != null;
			if (found) {
				this.SpriteProducedList.Remove(id);
			}
			if (this.DebugMessages) {
				DebugManager.LogMessage($"(TwitchMessageAtlasProducer) Requested ID:[{id}] Found:[{found}]");
			}
			return found;
		}

		public IEnumerator AtlasProducerCoroutine(CoroutineWrapper self) {
			// Allocate memory of non specific objects, specific to coroutine
			List<TwitchEmote> emotes = new List<TwitchEmote>();
			List<TwitchBadge> badges = new List<TwitchBadge>();
			List<Texture2D> textures = new List<Texture2D>();
			List<string> matchedNames = new List<string>();

			Start:
			// Make sure containers are cleared of old work
			emotes.Clear();
			badges.Clear();
			textures.Clear();
			matchedNames.Clear();

			do {
				if (!this.atlasConstructors.Contains(self)) { // If routine no longer in list, shut down
					yield break;
				}
				if (this.WaitForBuilders()) {
					yield return this.Wait1Second;
				} else {
					goto Work;
				}
			} while (true);

			Work:
			if (this.AtlasWorkQueue.TryDequeue(out QueueContainer work)) {
				Guid ID = work.ID;

				int emotesLen = work.Emotes.Length;
				int badgesLen = work.Badges.Length;

				if (work.Emotes != null && emotesLen > 0 && this.emotesManager != null) {
					emotes.AddRange(this.emotesManager.SearchEmotes(work.Emotes));
				}
				
				if (work.Badges != null && badgesLen > 0 && this.badgeManager != null) {
					badges.AddRange(this.badgeManager.GetBadges(work.Badges , work.RoomId));
				}

				if (this.DebugMessages) {
					DebugManager.LogMessage($"(TwitchMessageAtlasProducer) Build ID:[{ID}] Started. Emotes:[{string.Join(",", emotes)}] Badges:[{string.Join(",", badges)}]");
				}

				TMP_SpriteAsset atlas = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
				Texture2D spriteTexture = new Texture2D(1, 1);

				for (int x = 0; x < emotes.Count; x++) {
					TwitchEmote emote = emotes[x];
					if (!emote.IgnoreEmote) {
						textures.AddRange(emotes[x].DownloadedTexture);
					}
				}

				for (int x = 0; x < badges.Count; x++) {
					TwitchBadge badge = badges[x];
					if (!badge.IgnoreBadge) {
						for (int y = 0; y < badge.Versions.Count; y++) {
							textures.Add(badge.Versions[y].ImageData);
						}
					}
				}

				atlas.spriteSheet = spriteTexture;
				Rect[] bounds = spriteTexture.PackTextures(textures.ToArray(), 0, int.MaxValue);

				atlas.spriteInfoList = new List<TMP_Sprite>();

				int textureCount = 0;
				uint glyphID = 0;

				for (int emoteCount = 0; emoteCount < emotes.Count; emoteCount++) {
					TwitchEmote emote = emotes[emoteCount];

					if (emote.IgnoreEmote) {
						textureCount += emote.DownloadedTexture.Count;
					}
					else {
						emote.AnimationStartIndex = textureCount;

						for (int x = 0; x < emote.DownloadedTexture.Count; x++, textureCount++, glyphID++) {
							Rect emoteBounds = bounds[textureCount];

							string EmoteName = x == 0 ? emote.ID : emote.ID + x.ToString();

							TMP_Sprite spriteData = new TMP_Sprite() {
								name = EmoteName,
								scale = 1.0f,
								id = textureCount,
								height = emoteBounds.height * spriteTexture.height,
								width = emoteBounds.width * spriteTexture.width,
								x = emoteBounds.x * spriteTexture.width,
								y = emoteBounds.y * spriteTexture.height,
							};
							atlas.spriteInfoList.Add(spriteData);

							TMP_SpriteGlyph spriteGlyph = new TMP_SpriteGlyph() {
								glyphRect = new GlyphRect { width = (int)spriteData.width, height = (int)spriteData.height, x = (int)spriteData.x, y = (int)spriteData.y },
								metrics = new GlyphMetrics { width = spriteData.width, height = spriteData.height, horizontalBearingY = spriteData.height, horizontalBearingX = 0, horizontalAdvance = spriteData.width },
								index = glyphID,
								atlasIndex = textureCount,
								sprite = spriteData.sprite,
							};
							atlas.spriteGlyphTable.Add(spriteGlyph);

							TMP_SpriteCharacter spriteCharacter = new TMP_SpriteCharacter(glyphID, atlas, spriteGlyph) {
								name = EmoteName,
							};
							atlas.spriteCharacterTable.Add(spriteCharacter);

							atlas.spriteCharacterLookupTable.Add(glyphID, spriteCharacter);

							matchedNames.Add(spriteCharacter.name);
						}
						//textureCount already incremented for next emote
						emote.AnimationEndIndex = textureCount - 1;
					}
				}

				for (int badgeCount = 0; badgeCount < badges.Count; badgeCount++) {
					TwitchBadge badge = badges[badgeCount];
					if (badge.IgnoreBadge) {
						textureCount += badge.Versions.Count;
					}
					else {
						for (int x = 0; x < badge.Versions.Count; x++, textureCount++, glyphID++) {
							TwitchBadge.Version version = badge.Versions[x];
							Rect versionBounds = bounds[textureCount];
							string versionName = badge.AssociatedChannel + badge.Set_ID + version.id;

							TMP_Sprite spriteData = new TMP_Sprite() {
								name = versionName,
								scale = 1.0f,
								id = textureCount,
								height = versionBounds.height * spriteTexture.height,
								width = versionBounds.width * spriteTexture.width,
								x = versionBounds.x * spriteTexture.width,
								y = versionBounds.y * spriteTexture.height,
							};
							atlas.spriteInfoList.Add(spriteData);

							TMP_SpriteGlyph spriteGlyph = new TMP_SpriteGlyph() {
								glyphRect = new GlyphRect { width = (int)spriteData.width, height = (int)spriteData.height, x = (int)spriteData.x, y = (int)spriteData.y },
								metrics = new GlyphMetrics { width = spriteData.width, height = spriteData.height, horizontalBearingY = spriteData.height, horizontalBearingX = 0, horizontalAdvance = spriteData.width },
								index = glyphID,
								atlasIndex = textureCount,
								sprite = spriteData.sprite,
							};
							atlas.spriteGlyphTable.Add(spriteGlyph);

							TMP_SpriteCharacter spriteCharacter = new TMP_SpriteCharacter(glyphID, atlas, spriteGlyph) {
								name = versionName,
							};
							atlas.spriteCharacterTable.Add(spriteCharacter);

							atlas.spriteCharacterLookupTable.Add(glyphID, spriteCharacter);

							matchedNames.Add(spriteCharacter.name);
						}
					}
				}

				atlas.name = ID.ToString();
				atlas.UpdateLookupTables(); // Nukes index values for lookup... I assume because its not a pre exisiting Atlas Object

				atlas.material = new Material(Shader.Find("TextMeshPro/Sprite")) {
					mainTexture = spriteTexture
				};

				if (this.DebugMessages) {
					DebugManager.LogMessage($"(TwitchMessageAtlasProducer) Build ID:[{ID}] Started. Processed Characters: [{string.Join(", ", matchedNames)}]");
				}

				this.SpriteProducedList.Add(ID, atlas);
				OnAtlasRequestComplete?.Invoke(this, ID);
			}
			yield return null;
			goto Start;
		}
	}

}