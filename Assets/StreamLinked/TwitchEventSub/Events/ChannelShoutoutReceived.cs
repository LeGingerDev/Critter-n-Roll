using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutreceive">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelShoutoutReceived : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string from_broadcaster_user_id { get; set; }
		[field: SerializeField] public string from_broadcaster_user_login { get; set; }
		[field: SerializeField] public string from_broadcaster_user_name { get; set; }
		[field: SerializeField] public int viewer_count { get; set; }
		[field: SerializeField] public string started_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Shoutout_Received;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_shoutouts,
			TwitchScopesEnum.moderator_manage_shoutouts,
		};

		public ChannelShoutoutReceived(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.from_broadcaster_user_id = body[TwitchWords.FROM_BROADCASTER_USER_ID].AsString;
			this.from_broadcaster_user_login = body[TwitchWords.FROM_BROADCASTER_USER_LOGIN].AsString;
			this.from_broadcaster_user_name = body[TwitchWords.FROM_BROADCASTER_USER_NAME].AsString;
			this.viewer_count = body[TwitchWords.VIEWER_COUNT].AsInteger;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
		}
	}
}
