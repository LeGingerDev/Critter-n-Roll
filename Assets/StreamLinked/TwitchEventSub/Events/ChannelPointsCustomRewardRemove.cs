using System;

using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.LightJson;
using UnityEngine;
using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardremove">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelPointsCustomRewardRemove : ICustomReward {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public bool is_enabled { get; set; }
		[field: SerializeField] public bool is_paused { get; set; }
		[field: SerializeField] public bool is_in_stock { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public int cost { get; set; }
		[field: SerializeField] public string prompt { get; set; }
		[field: SerializeField] public bool is_user_input_required { get; set; }
		[field: SerializeField] public bool should_redemptions_skip_request_queue { get; set; }
		[field: SerializeField] public MaxPerStream max_per_stream { get; set; }
		[field: SerializeField] public MaxPerUserPerStream max_per_user_per_stream { get; set; }
		[field: SerializeField] public string background_color { get; set; }
		[field: SerializeField] public Image image { get; set; }
		[field: SerializeField] public Image default_image { get; set; }
		[field: SerializeField] public GlobalCooldown global_cooldown { get; set; }
		[field: SerializeField] public string cooldown_expires_at { get; set; }
		[field: SerializeField] public int redemptions_redeemed_current_stream { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Remove;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_redemptions,
			TwitchScopesEnum.channel_manage_redemptions,
		};

		public ChannelPointsCustomRewardRemove(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.is_enabled = body[TwitchWords.IS_ENABLED].AsBoolean;
			this.is_paused = body[TwitchWords.IS_PAUSED].AsBoolean;
			this.is_in_stock = body[TwitchWords.IS_IN_STOCK].AsBoolean;
			this.title = body[TwitchWords.TITLE].AsString;
			this.cost = body[TwitchWords.COST].AsInteger;
			this.prompt = body[TwitchWords.PROMPT].AsString;
			this.is_user_input_required = body[TwitchWords.IS_USER_INPUT_REQUIRED].AsBoolean;
			this.should_redemptions_skip_request_queue = body[TwitchWords.SHOULD_REDEMPTIONS_SKIP_REQUEST_QUEUE].AsBoolean;
			this.max_per_stream = new MaxPerStream(body[TwitchWords.MAX_PER_STREAM]);
			this.max_per_user_per_stream = new MaxPerUserPerStream(body[TwitchWords.MAX_PER_USER_PER_STREAM]);
			this.background_color = body[TwitchWords.BACKGROUND_COLOR].AsString;
			this.image = new Image(body[TwitchWords.IMAGE]);
			this.default_image = new Image(body[TwitchWords.DEFAULT_IMAGE]);
			this.global_cooldown = new GlobalCooldown(body[TwitchWords.GLOBAL_COOLDOWN]);
			this.cooldown_expires_at = body[TwitchWords.COOLDOWN_EXPIRES_AT].AsString;
			this.redemptions_redeemed_current_stream = body[TwitchWords.REDEMPTIONS_REDEEMED_CURRENT_STREAM].AsInteger;
		}
	}
}