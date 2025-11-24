using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Clips {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-clips">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetClips : IClips {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string url { get; set; }
		[field: SerializeField] public string embed_url { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string creator_id { get; set; }
		[field: SerializeField] public string creator_name { get; set; }
		[field: SerializeField] public string video_id { get; set; }
		[field: SerializeField] public string game_id { get; set; }
		[field: SerializeField] public string language { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public int view_count { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string thumbnail_url { get; set; }
		[field: SerializeField] public double? duration { get; set; }
		[field: SerializeField] public int vod_offset { get; set; }
		[field: SerializeField] public bool is_featured { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.url = body[TwitchWords.URL].AsString;
			this.embed_url = body[TwitchWords.EMBED_URL].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.creator_id = body[TwitchWords.CREATOR_ID].AsString;
			this.creator_name = body[TwitchWords.CREATOR_NAME].AsString;
			this.video_id = body[TwitchWords.VIDEO_ID].AsString;
			this.game_id = body[TwitchWords.GAME_ID].AsString;
			this.language = body[TwitchWords.LANGUAGE].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.view_count = body[TwitchWords.VIEW_COUNT].AsInteger;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.thumbnail_url = body[TwitchWords.THUMBNAIL_URL].AsString;
			this.duration = body[TwitchWords.DURATION].AsNumber;
			this.vod_offset = body[TwitchWords.VOD_OFFSET].AsInteger;
			this.is_featured = body[TwitchWords.IS_FEATURED].IsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.Clips;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetClips;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string GAME_ID => TwitchWords.GAME_ID;
		public static string ID => TwitchWords.ID;
		public static string STARTED_AT => TwitchWords.STARTED_AT;
		public static string ENDED_AT => TwitchWords.ENDED_AT;
		public static string FIRST => TwitchWords.FIRST;
		public static string BEFORE => TwitchWords.BEFORE;
		public static string AFTER => TwitchWords.AFTER;
		public static string IS_FEATURED => TwitchWords.IS_FEATURED;
	}
}