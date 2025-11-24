using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Users {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-block-list">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetUserBlockList : IUsers {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string display_name { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.display_name = body[TwitchWords.DISPLAY_NAME].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.UserBlockList;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetUserBlockList;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_blocked_users,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}