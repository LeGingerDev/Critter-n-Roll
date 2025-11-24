using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelChatMessage : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string chatter_user_id { get; set; }
		[field: SerializeField] public string chatter_user_name { get; set; }
		[field: SerializeField] public string chatter_user_login { get; set; }
		[field: SerializeField] public string message_id { get; set; }
		[field: SerializeField] public Message message { get; set; }
		[field: SerializeField] public string message_type { get; set; }
		[field: SerializeField] public Badge[] badges { get; set; }
		[field: SerializeField] public string color { get; set; }
		[field: SerializeField] public Cheer cheer { get; set; }
		[field: SerializeField] public Reply reply { get; set; }
		[field: SerializeField] public string channel_points_custom_reward_id { get; set; }
		[field: SerializeField] public string source_broadcaster_user_id { get; set; }
		[field: SerializeField] public string source_broadcaster_user_name { get; set; }
		[field: SerializeField] public string source_broadcaster_user_login { get; set; }
		[field: SerializeField] public string source_message_id { get; set; }
		[field: SerializeField] public Badge[] source_badges { get; set; }
		[field: SerializeField] public bool is_source_only { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Chat_Message;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_bot,
			TwitchScopesEnum.user_bot,
			TwitchScopesEnum.user_read_chat,
		};

		public ChannelChatMessage(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.chatter_user_id = body[TwitchWords.CHATTER_USER_ID].AsString;
			this.chatter_user_name = body[TwitchWords.CHATTER_USER_NAME].AsString;
			this.chatter_user_login = body[TwitchWords.CHATTER_USER_LOGIN].AsString;
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
			this.message = new Message(body[TwitchWords.MESSAGE]);
			this.message_type = body[TwitchWords.MESSAGE_TYPE].AsString;
			this.badges = body[TwitchWords.BADGES].AsJsonArray?.ToModelArray<Badge>();
			this.color = body[TwitchWords.COLOR].AsString;
			this.cheer = new Cheer(body[TwitchWords.CHEER]);
			this.reply = new Reply(body[TwitchWords.REPLY]);
			this.channel_points_custom_reward_id = body[TwitchWords.CHANNEL_POINTS_CUSTOM_REWARD_ID].AsString;
			this.source_broadcaster_user_id = body[TwitchWords.SOURCE_BROADCASTER_USER_ID].AsString;
			this.source_broadcaster_user_name = body[TwitchWords.SOURCE_BROADCASTER_USER_NAME].AsString;
			this.source_broadcaster_user_login = body[TwitchWords.SOURCE_BROADCASTER_USER_LOGIN].AsString;
			this.source_message_id = body[TwitchWords.SOURCE_MESSAGE_ID].AsString;
			this.source_badges = body[TwitchWords.SOURCE_BADGES].AsJsonArray?.ToModelArray<Badge>();
			this.is_source_only = body[TwitchWords.IS_SOURCE_ONLY].AsBoolean;
		}
	}
}