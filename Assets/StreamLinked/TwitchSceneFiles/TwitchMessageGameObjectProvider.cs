using System.Collections.Generic;

using ScoredProductions.StreamLinked.IRC;

using UnityEngine;
using UnityEngine.UI;

namespace ScoredProductions.StreamLinked.TwitchSceneFiles {
	public class TwitchMessageGameObjectProvider : MonoBehaviour {

		public GameObject ListObjectLocation;
		public GameObject PrefabTwitchMessage;
		public ScrollRect scrollBar;

		private TwitchIRCClient ircClient;

		private readonly Queue<TwitchMessage> messageQueue = new Queue<TwitchMessage>();

		private void Awake() {
			if (this.ListObjectLocation == null || this.PrefabTwitchMessage == null) {
				this.enabled = false;
			}
		}

		private void OnEnable() {
			if (TwitchIRCClient.CreateOrGetInstance(out this.ircClient)) {
				this.ircClient.OnPRIVMSG.AddListener(this.TwitchIRCClient_OnMessageReceived);
			}
			else {
				this.enabled = false;
			}
		}

		private void OnDisable() {
			if (this.ircClient != null) {
				this.ircClient.OnPRIVMSG.RemoveListener(this.TwitchIRCClient_OnMessageReceived);
			}
		}

		private void Update() {
			if (this.messageQueue.TryDequeue(out TwitchMessage message)) {
				this.BuildMessage(message);
			}
		}

		private void TwitchIRCClient_OnMessageReceived(TwitchMessage obj) {
			this.messageQueue.Enqueue(obj);
		}

		private void BuildMessage(TwitchMessage twitchMessage) {
			GameObject newMessage = Instantiate(this.PrefabTwitchMessage);
			newMessage.transform.SetParent(this.ListObjectLocation.transform);
			RectTransform messageRec = newMessage.GetComponent<RectTransform>();
			messageRec.offsetMax = Vector2.zero;
			messageRec.offsetMin = Vector2.zero;
			TwitchMessageGameObject messageClass = newMessage.GetComponent<TwitchMessageGameObject>();
			messageClass.ReceiveTwitchMessage(twitchMessage);

			if (this.scrollBar) {
				this.scrollBar.verticalNormalizedPosition = 0;
			}
		}

	}
}
