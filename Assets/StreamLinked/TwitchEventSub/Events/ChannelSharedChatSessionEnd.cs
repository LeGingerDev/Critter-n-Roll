using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-shared-chat-session-update-event">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelSharedChatSessionEnd : IChannel {

		[field: SerializeField] public string session_id { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string host_broadcaster_user_id { get; set; }
		[field: SerializeField] public string host_broadcaster_user_name { get; set; }
		[field: SerializeField] public string host_broadcaster_user_login { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Shared_Chat_Session_End;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public ChannelSharedChatSessionEnd(JsonValue body) {
			this.session_id = body[TwitchWords.SESSION_ID].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.host_broadcaster_user_id = body[TwitchWords.HOST_BROADCASTER_USER_ID].AsString;
			this.host_broadcaster_user_name = body[TwitchWords.HOST_BROADCASTER_USER_NAME].AsString;
			this.host_broadcaster_user_login = body[TwitchWords.HOST_BROADCASTER_USER_LOGIN].AsString;
		}
	}
}