using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userauthorizationgrant">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct UserAuthorizationGrant : IUser {

		[field: SerializeField] public string client_id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.User_Authorization_Grant;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public UserAuthorizationGrant(JsonValue body) {
			this.client_id = body[TwitchWords.CLIENT_ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
		}
	}
}