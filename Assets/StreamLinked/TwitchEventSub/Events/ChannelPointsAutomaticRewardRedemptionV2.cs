using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd-v2">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelPointsAutomaticRewardRedemptionV2 : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public Reward reward { get; set; }
		[field: SerializeField] public Message message { get; set; }
		[field: SerializeField] public string redeemed_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Points_Automatic_Reward_Redemption_V2;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_redemptions,
			TwitchScopesEnum.channel_manage_redemptions,
		};

		public ChannelPointsAutomaticRewardRedemptionV2(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.id = body[TwitchWords.ID].AsString;
			this.reward = new Reward(body[TwitchWords.REWARD]);
			this.message = new Message(body[TwitchWords.MESSAGE]);
			this.redeemed_at = body[TwitchWords.REDEEMED_AT].AsString;
		}
	}
}