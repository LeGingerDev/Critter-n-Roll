using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsuspicious_userupdate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelSuspiciousUserUpdate : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string low_trust_status { get; set; }
		[field: SerializeField] public string[] shared_ban_channel_ids { get; set; }
		[field: SerializeField] public string[] types { get; set; }
		[field: SerializeField] public string ban_evasion_evaluation { get; set; }
		[field: SerializeField] public Message message { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Suspicious_User_Update;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_suspicious_users,
		};

		public ChannelSuspiciousUserUpdate(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.low_trust_status = body[TwitchWords.LOW_TRUST_STATUS].AsString;
			this.shared_ban_channel_ids = body[TwitchWords.SHARED_BAN_CHANNEL_IDS].AsJsonArray?.CastToStringArray;
			this.types = body[TwitchWords.TYPES].AsJsonArray?.CastToStringArray;
			this.ban_evasion_evaluation = body[TwitchWords.BAN_EVASION_EVALUATION].AsString;
			this.message = new Message(body[TwitchWords.MESSAGE]);
		}
	}
}