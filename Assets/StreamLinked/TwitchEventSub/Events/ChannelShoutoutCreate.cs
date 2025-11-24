using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutcreate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelShoutoutCreate : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string to_broadcaster_user_id { get; set; }
		[field: SerializeField] public string to_broadcaster_user_login { get; set; }
		[field: SerializeField] public string to_broadcaster_user_name { get; set; }
		[field: SerializeField] public string moderator_user_id { get; set; }
		[field: SerializeField] public string moderator_user_login { get; set; }
		[field: SerializeField] public string moderator_user_name { get; set; }
		[field: SerializeField] public int viewer_count { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string cooldown_ends_at { get; set; }
		[field: SerializeField] public string target_cooldown_ends_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Shoutout_Create;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_shoutouts,
			TwitchScopesEnum.moderator_manage_shoutouts,
		};

		public ChannelShoutoutCreate(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.to_broadcaster_user_id = body[TwitchWords.TO_BROADCASTER_USER_ID].AsString;
			this.to_broadcaster_user_login = body[TwitchWords.TO_BROADCASTER_USER_LOGIN].AsString;
			this.to_broadcaster_user_name = body[TwitchWords.TO_BROADCASTER_USER_NAME].AsString;
			this.moderator_user_id = body[TwitchWords.MODERATOR_USER_ID].AsString;
			this.moderator_user_login = body[TwitchWords.MODERATOR_USER_LOGIN].AsString;
			this.moderator_user_name = body[TwitchWords.MODERATOR_USER_NAME].AsString;
			this.viewer_count = body[TwitchWords.VIEWER_COUNT].AsInteger;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.cooldown_ends_at = body[TwitchWords.COOLDOWN_ENDS_AT].AsString;
			this.target_cooldown_ends_at = body[TwitchWords.TARGET_COOLDOWN_ENDS_AT].AsString;
		}
	}
}
