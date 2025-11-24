using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-banned-users">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetBannedUsers : IModeration {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string expires_at { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string reason { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public string moderator_login { get; set; }
		[field: SerializeField] public string moderator_name { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.expires_at = body[TwitchWords.EXPIRES_AT].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.reason = body[TwitchWords.REASON].AsString;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.moderator_login = body[TwitchWords.MODERATOR_LOGIN].AsString;
			this.moderator_name = body[TwitchWords.MODERATOR_NAME].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetBannedUsers;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetBannedUsers;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderation_read,
			TwitchScopesEnum.moderator_manage_banned_users,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string USER_ID => TwitchWords.USER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
		public static string BEFORE => TwitchWords.BEFORE;
	}
}