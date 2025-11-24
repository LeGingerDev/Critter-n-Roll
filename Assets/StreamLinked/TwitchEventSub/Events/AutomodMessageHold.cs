using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessagehold">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct AutoMessageHold : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string message_id { get; set; }
		[field: SerializeField] public Message message { get; set; }
		[field: SerializeField] public string category { get; set; }
		[field: SerializeField] public int level { get; set; }
		[field: SerializeField] public string held_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Automod_Message_Hold;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_automod,
		};

		public AutoMessageHold(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
			this.message = new Message(body[TwitchWords.MESSAGE]);
			this.category = body[TwitchWords.CATEGORY].AsString;
			this.level = body[TwitchWords.LEVEL].AsInteger;
			this.held_at = body[TwitchWords.HELD_AT].AsString;
		}
	}
}