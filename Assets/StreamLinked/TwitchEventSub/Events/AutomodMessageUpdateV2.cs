using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessageupdate-v2">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct AutoMessageUpdateV2 : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string moderator_user_id { get; set; }
		[field: SerializeField] public string moderator_user_login { get; set; }
		[field: SerializeField] public string moderator_user_name { get; set; }
		[field: SerializeField] public string message_id { get; set; }
		[field: SerializeField] public Message message { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public string held_at { get; set; }
		[field: SerializeField] public string reason { get; set; }
		[field: SerializeField] public Automod automod { get; set; }
		[field: SerializeField] public BlockedTerm blocked_term { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Automod_Message_Update_V2;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_automod,
		};

		public AutoMessageUpdateV2(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.moderator_user_id = body[TwitchWords.MODERATOR_USER_ID].AsString;
			this.moderator_user_login = body[TwitchWords.MODERATOR_USER_LOGIN].AsString;
			this.moderator_user_name = body[TwitchWords.MODERATOR_USER_NAME].AsString;
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
			this.message = new Message(body[TwitchWords.MESSAGE]);
			this.status = body[TwitchWords.STATUS].AsString;
			this.held_at = body[TwitchWords.HELD_AT].AsString;
			this.reason = body[TwitchWords.REASON].AsString;
			this.automod = new Automod(body[TwitchWords.AUTOMOD]);
			this.blocked_term = new BlockedTerm(body[TwitchWords.BLOCKED_TERM]);
		}
	}
}