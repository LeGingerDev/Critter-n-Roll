using System.Threading;
using System.Threading.Tasks;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.Users;
using ScoredProductions.StreamLinked.EventSub;
using ScoredProductions.StreamLinked.EventSub.Events;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.UI;

namespace ScoredProductions.StreamLinked.TwitchSceneFiles {

	/// <summary>
	/// Simple implementation object of starting the Twitch EventSub and subscribing to the OAuth channels Poll related subscriptions.
	/// </summary>
	public class TwitchEventSubStartupExample : MonoBehaviour {

		public Text OutputText;

		public GetUsers UsernameData { get; private set; }

		private TwitchEventSubClient referenceEventSub;

		private CancellationTokenSource cts = new CancellationTokenSource();

		private void Awake() {
			if (!TwitchAPIClient.CreateOrGetInstance(out _) | !TwitchEventSubClient.CreateOrGetInstance(out this.referenceEventSub)) {
				DebugManager.LogMessage("TwitchEventSubInterface requires TwitchAPIClient and TwitchEventSubClient to be in the scene to work.".RichTextColour("red"));
				this.gameObject.SetActive(false);
			}
		}

		private void OnEnable() {
			this.cts ??= new CancellationTokenSource();

			this.referenceEventSub.OnChannelPollBegin.AddListener(this.OnChannelPollBegin);
			this.referenceEventSub.OnChannelPollProgress.AddListener(this.OnChannelPollProgress);
			this.referenceEventSub.OnChannelPollEnd.AddListener(this.OnChannelPollEnd);
		}

		private void OnDisable() {
			this.cts?.Cancel();
			this.cts?.Dispose();

			this.referenceEventSub.OnChannelPollBegin.RemoveListener(this.OnChannelPollBegin);
			this.referenceEventSub.OnChannelPollProgress.RemoveListener(this.OnChannelPollProgress);
			this.referenceEventSub.OnChannelPollEnd.RemoveListener(this.OnChannelPollEnd);
		}

		public async Task BuildUserSubscriptionsForUser() {
			string UserID;

			if (this.cts.IsCancellationRequested) { 
				return;
			}

			TwitchAPIDataContainer<GetUsers> returnedData
					= await TwitchAPIClient.MakeTwitchAPIRequestAsync<GetUsers>(
						cancelToken: this.cts.Token);
			if (!returnedData.HasErrored) {
				this.UsernameData = returnedData.data[0];				
				UserID = this.UsernameData.id;
			}
			else {
				DebugManager.LogMessage($"GetUsers API call failed to get User associated with OAuth. Error: {returnedData.ErrorText}");
				return;
			}

			if (this.cts.IsCancellationRequested) { 
				return;
			}

			if (TwitchEventSubClient.GetInstance(out this.referenceEventSub)) {
				Condition broadcasterCondition = new Condition() {
					broadcaster_user_id = UserID
				};

				await this.referenceEventSub.BeginConnectionSession(false, false,
					(TwitchEventSubSubscriptionsEnum.Channel_Poll_Begin, broadcasterCondition),
					(TwitchEventSubSubscriptionsEnum.Channel_Poll_Progress, broadcasterCondition),
					(TwitchEventSubSubscriptionsEnum.Channel_Poll_End, broadcasterCondition)
				);

				//if (this.cts.IsCancellationRequested) { 
				//	return;
				//}

				//await this.referenceEventSub.SubscribeToEvent<ChannelPollBegin>(new Condition() {
				//	broadcaster_user_id = UserID
				//});

				//if (this.cts.IsCancellationRequested) { 
				//	return; 
				//}

				//await this.referenceEventSub.SubscribeToEvent<ChannelPollProgress>(new Condition() {
				//	broadcaster_user_id = UserID
				//});

				//if (this.cts.IsCancellationRequested) {
				//	return; 
				//}

				//await this.referenceEventSub.SubscribeToEvent<ChannelPollEnd>(new Condition() {
				//	broadcaster_user_id = UserID
				//});
			}
		}

		private void OnChannelPollBegin(ChannelPollBegin begin) {
			if (!string.IsNullOrEmpty(begin.id)) {
				DebugManager.LogMessage("OnChannelPollBegin : " + begin.id);

				if (this.OutputText == null) {
					return;
				}

				this.OutputText.text = JsonWriter.Serialize(begin, true);
			}
		}
		
		private void OnChannelPollProgress(ChannelPollProgress progress) {
			if (!string.IsNullOrEmpty(progress.id)) {
				DebugManager.LogMessage("OnChannelPollProgress : " + progress.id);

				if (this.OutputText == null) {
					return;
				}

				this.OutputText.text = JsonWriter.Serialize(progress, true);
			}
		}
		
		private void OnChannelPollEnd(ChannelPollEnd end) {
			if (!string.IsNullOrEmpty(end.id)) {
				DebugManager.LogMessage("OnChannelPollEnd : " + end.id);

				if (this.OutputText == null) {
					return;
				}

				this.OutputText.text = JsonWriter.Serialize(end, true);
			}
		}
	}
}