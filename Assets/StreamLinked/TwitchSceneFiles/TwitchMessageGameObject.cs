using System;
using System.Threading;

using ScoredProductions.StreamLinked.IRC;
using ScoredProductions.StreamLinked.ManagersAndBuilders;

using TMPro;

using UnityEngine;

namespace ScoredProductions.StreamLinked.TwitchSceneFiles {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TwitchMessageGameObject : MonoBehaviour {
		private TextMeshProUGUI textBody;
		private TwitchMessage message;

		[Tooltip("Editing this value will not update the used sprite atlas")]
		public TMP_SpriteAsset UnityAtlas;

		private readonly CancellationTokenSource cts = new CancellationTokenSource();

		private Guid AtlasReference;

		private void Awake() {
			this.textBody = this.GetComponent<TextMeshProUGUI>();
		}

		public void ReceiveTwitchMessage(TwitchMessage obj) {
			this.message = obj;
			this.ProduceContent();
		}

		private void OnDisable() {
			TwitchMessageAtlasProducer.OnAtlasRequestComplete -= this.OnAtlasRequestComplete; // Double check it is removed
		}

		private void ProduceContent() {
			if (this.textBody == null) {
				return;
			}

			if (!this.message.CheckHasBadgesOrEmotes()) {
				this.BuildMessageText(this.message.BuildMessageForTextmesh());
				return;
			}

			if (TwitchMessageAtlasProducer.GetInstance(out TwitchMessageAtlasProducer atlasManager)) {
				TwitchMessageAtlasProducer.OnAtlasRequestComplete += this.OnAtlasRequestComplete;
				this.AtlasReference = atlasManager.EnqueueAtlasBuild(message);
			}
		}

		private void OnAtlasRequestComplete(object sender, Guid e) {
			if (e == this.AtlasReference
				&& sender is TwitchMessageAtlasProducer atlasManager // EventHandlers allow bypas of GetInstance
				&& atlasManager.TryGetAtlas(e, out TMP_SpriteAsset atlas)) {
				this.UnityAtlas = atlas;

				this.textBody.spriteAsset = atlas;

				this.StartCoroutine(this.message.BuildMessageForTextmeshWithWait(this.BuildMessageText));

				TwitchMessageAtlasProducer.OnAtlasRequestComplete -= this.OnAtlasRequestComplete; // We have our content, dont need event any more
			}
		}

		private void BuildMessageText((string user, string body) value) {
			this.textBody.autoSizeTextContainer = true;
			this.textBody.SetText(value.user + value.body);
		}

		private void OnDestroy() {
			this.cts.Cancel();
			this.cts.Dispose();
		}
	}
}
