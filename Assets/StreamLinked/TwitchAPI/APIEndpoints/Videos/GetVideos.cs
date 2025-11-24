using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Videos {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-videos">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetVideos : IVideos {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string stream_id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string published_at { get; set; }
		[field: SerializeField] public string url { get; set; }
		[field: SerializeField] public string thumbnail_url { get; set; }
		[field: SerializeField] public string viewable { get; set; }
		[field: SerializeField] public int view_count { get; set; }
		[field: SerializeField] public string language { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string duration { get; set; }
		[field: SerializeField] public MutedSegment[] muted_segments { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.stream_id = body[TwitchWords.STREAM_ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.published_at = body[TwitchWords.PUBLISHED_AT].AsString;
			this.url = body[TwitchWords.URL].AsString;
			this.thumbnail_url = body[TwitchWords.THUMBNAIL_URL].AsString;
			this.viewable = body[TwitchWords.VIEWABLE].AsString;
			this.view_count = body[TwitchWords.VIEW_COUNT].AsInteger;
			this.language = body[TwitchWords.LANGUAGE].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.duration = body[TwitchWords.DURATION].AsString;
			this.muted_segments = body[TwitchWords.MUTED_SEGMENTS].AsJsonArray?.ToModelArray<MutedSegment>();
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.Videos;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetVideos;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string ID => TwitchWords.ID;
		public static string USER_ID => TwitchWords.USER_ID;
		public static string GAME_ID => TwitchWords.GAME_ID;
		public static string LANGUAGE => TwitchWords.LANGUAGE;
		public static string PERIOD => TwitchWords.PERIOD;
		public static string SORT => TwitchWords.SORT;
		public static string TYPE => TwitchWords.TYPE;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
		public static string BEFORE => TwitchWords.BEFORE;
	}
}