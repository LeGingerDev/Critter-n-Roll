using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Streams {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-followed-streams">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetFollowedStreams : IStreams {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string game_id { get; set; }
		[field: SerializeField] public string game_name { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public int viewer_count { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string language { get; set; }
		[field: SerializeField] public string thumbnail_url { get; set; }
		[Obsolete("As of February 28, 2023, this field is deprecated and returns only an empty array. If you use this field, please update your code to use the tags field.")]
		[field: SerializeField] public string[] tag_ids { get; set; }
		[field: SerializeField] public string[] tags { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.game_id = body[TwitchWords.GAME_ID].AsString;
			this.game_name = body[TwitchWords.GAME_NAME].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.viewer_count = body[TwitchWords.VIEWER_COUNT].AsInteger;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.language = body[TwitchWords.LANGUAGE].AsString;
			this.thumbnail_url = body[TwitchWords.THUMBNAIL_URL].AsString;
#pragma warning disable CS0618 // Type or member is obsolete
			this.tag_ids = body[TwitchWords.TAG_IDS].AsJsonArray?.CastToStringArray;
#pragma warning restore CS0618 // Type or member is obsolete
			this.tags = body[TwitchWords.TAGS].AsJsonArray?.CastToStringArray;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetFollowedStreams;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetFollowedStreams;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_follows,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}