using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage_delete">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelChatMessageDelete : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string target_user_id { get; set; }
		[field: SerializeField] public string target_user_name { get; set; }
		[field: SerializeField] public string target_user_login { get; set; }
		[field: SerializeField] public string message_id { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Chat_Message_Delete;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_bot,
			TwitchScopesEnum.user_bot,
			TwitchScopesEnum.user_read_chat,
		};

		public ChannelChatMessageDelete(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.target_user_id = body[TwitchWords.TARGET_USER_ID].AsString;
			this.target_user_name = body[TwitchWords.TARGET_USER_NAME].AsString;
			this.target_user_login = body[TwitchWords.TARGET_USER_LOGIN].AsString;
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
		}
	}
}