using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userupdate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct UserUpdate : IUser {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string email { get; set; }
		[field: SerializeField] public bool email_verified { get; set; }
		[field: SerializeField] public string description { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.User_Update;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_email,
		};

		public UserUpdate(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.email = body[TwitchWords.EMAIL].AsString;
			this.email_verified = body[TwitchWords.EMAIL_VERIFIED].AsBoolean;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
		}
	}
}