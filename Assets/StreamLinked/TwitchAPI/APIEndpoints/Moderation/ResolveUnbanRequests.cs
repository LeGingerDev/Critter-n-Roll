using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#resolve-unban-requests">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct ResolveUnbanRequests : IModeration {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public string moderator_login { get; set; }
		[field: SerializeField] public string moderator_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string text { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string resolved_at { get; set; }
		[field: SerializeField] public string resolution_text { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.moderator_login = body[TwitchWords.MODERATOR_LOGIN].AsString;
			this.moderator_name = body[TwitchWords.MODERATOR_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.text = body[TwitchWords.TEXT].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.resolved_at = body[TwitchWords.RESOLVED_AT].AsString;
			this.resolution_text = body[TwitchWords.RESOLUTION_TEXT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.UnbanRequests;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.ResolveUnbanRequests;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] { 
			TwitchScopesEnum.moderator_manage_unban_requests,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string STATUS => TwitchWords.STATUS;
		public static string UNBAN_REQUEST_ID => TwitchWords.UNBAN_REQUEST_ID;
		public static string RESOLUTION_TEXT => TwitchWords.RESOLUTION_TEXT;

	}
}