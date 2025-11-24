using System;
using System.Collections;
using System.Collections.Generic;

using ScoredProductions.StreamLinked.IRC;
using ScoredProductions.StreamLinked.ManagersAndBuilders;

using TMPro;

using UnityEngine;

namespace ScoredProductions.StreamLinked.TwitchSceneFiles {
	[RequireComponent(typeof(TextMeshPro))]
	public class TwitchMessageSingleDisplayer : MonoBehaviour {

		[SerializeField, HideInInspector]
		private string _filterByUser;
		public string FilterByUser {
			get => this._filterByUser;
			set
			{
				if (value != this._filterByUser) {
					this.messageQueue.Clear();
					this.atlasRequests.Clear();
				}

				this._filterByUser = value?.ToLower() ?? "";
			}
		}

		[SerializeField, HideInInspector]
		private int _messageDelay;
		public int MessageDelay {
			get => this._messageDelay;
			set
			{
				if (value != this._messageDelay) {
					this.waitTimer = new WaitForSeconds(this._messageDelay);
				}

				this._messageDelay = value;
			}
		}

		private TextMeshPro textBody;

		private TwitchIRCClient ircClient;

		private WaitForSeconds waitTimer;

		private Coroutine messageUpdater;

		private TwitchMessageAtlasProducer atlasProducer;

		private readonly Queue<TwitchMessage> messageQueue = new Queue<TwitchMessage>();

		private readonly Dictionary<TwitchMessage, Guid> atlasRequests = new Dictionary<TwitchMessage, Guid>();

		private void Awake() {
			this.textBody = this.GetComponent<TextMeshPro>();
			this.waitTimer = new WaitForSeconds(this._messageDelay);
		}

		private void OnEnable() {
			TwitchMessageAtlasProducer.GetInstance(out this.atlasProducer);

			if (TwitchMessageAtlasProducer.GetInstance(out this.atlasProducer)) {
				this.messageUpdater ??= this.StartCoroutine(this.UpdateMessageCoroutine());
			} else {
				this.OnDisable();
				return;
			}

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
			if (this.messageUpdater != null) {
				this.StopCoroutine(this.messageUpdater);
			}
		}

		private void TwitchIRCClient_OnMessageReceived(TwitchMessage obj) {
			if (string.IsNullOrWhiteSpace(this.FilterByUser) || obj.Username == this.FilterByUser) {
				this.messageQueue.Enqueue(obj);
				this.atlasRequests.Add(obj, this.atlasProducer.EnqueueAtlasBuild(obj));
			}
		}

		private IEnumerator UpdateMessageCoroutine() {
			Restart:
			yield return this.waitTimer;
			if (this.messageQueue.TryDequeue(out TwitchMessage message)) {
				Guid requestId = this.atlasRequests[message];
				TMP_SpriteAsset atlas = null;
				while (!this.atlasProducer.TryGetAtlas(requestId, out atlas)) {
					yield return TwitchStatic.OneSecondWait;
				}
				this.textBody.spriteAsset = atlas;
				(string user, string body) = message.BuildMessageForTextmesh();
				this.textBody.text = user + body;
				this.textBody.SetAllDirty();
				this.atlasRequests.Remove(message);
			}
			goto Restart;
		}
	}
}
