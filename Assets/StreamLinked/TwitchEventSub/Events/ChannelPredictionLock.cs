using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionlock">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelPredictionLock : IPrediction {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public Outcomes[] outcomes { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string locks_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Prediction_Lock;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_predictions,
			TwitchScopesEnum.channel_manage_predictions,
		};

		public ChannelPredictionLock(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.outcomes = body[TwitchWords.OUTCOMES].AsJsonArray?.ToModelArray<Outcomes>();
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.locks_at = body[TwitchWords.LOCKS_AT].AsString;
		}
	}
}