using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_settingsupdate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelChatSettingsUpdate : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public bool emote_mode { get; set; }
		[field: SerializeField] public bool follower_mode { get; set; }
		[field: SerializeField] public int follower_mode_duration_minutes { get; set; }
		[field: SerializeField] public bool slow_mode { get; set; }
		[field: SerializeField] public int slow_mode_wait_time_seconds { get; set; }
		[field: SerializeField] public bool subscriber_mode { get; set; }
		[field: SerializeField] public bool unique_chat_mode { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Chat_Settings_Update;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_bot,
			TwitchScopesEnum.user_bot,
			TwitchScopesEnum.user_read_chat,
		};

		public ChannelChatSettingsUpdate(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.emote_mode = body[TwitchWords.EMOTE_MODE].AsBoolean;
			this.follower_mode = body[TwitchWords.FOLLOWER_MODE].AsBoolean;
			this.follower_mode_duration_minutes = body[TwitchWords.FOLLOWER_MODE_DURATION_MINUTES].AsInteger;
			this.slow_mode = body[TwitchWords.SLOW_MODE].AsBoolean;
			this.slow_mode_wait_time_seconds = body[TwitchWords.SLOW_MODE_WAIT_TIME_SECONDS].AsInteger;
			this.subscriber_mode = body[TwitchWords.SUBSCRIBER_MODE].AsBoolean;
			this.unique_chat_mode = body[TwitchWords.UNIQUE_CHAT_MODE].AsBoolean;
		}
	}
}