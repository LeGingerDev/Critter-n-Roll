using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptiongift">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelSubscriptionGift : ISubscription {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public int total { get; set; }
		[field: SerializeField] public string tier { get; set; }
		[field: SerializeField] public int cumulative_total { get; set; }
		[field: SerializeField] public bool is_anonymous { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Subscription_Gift;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_subscriptions,
		};

		public ChannelSubscriptionGift(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.total = body[TwitchWords.TOTAL].AsInteger;
			this.tier = body[TwitchWords.TIER].AsString;
			this.cumulative_total = body[TwitchWords.CUMULATIVE_TOTAL].AsInteger;
			this.is_anonymous = body[TwitchWords.IS_ANONYMOUS].AsBoolean;
		}
	}
}