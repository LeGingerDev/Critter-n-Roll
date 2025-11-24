using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Search {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#search-channels">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SearchChannels : ISearch {

		[field: SerializeField] public string broadcaster_language { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string display_name { get; set; }
		[field: SerializeField] public string game_id { get; set; }
		[field: SerializeField] public string game_name { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public bool is_live { get; set; }
		[field: SerializeField] public string[] tags { get; set; }
		[field: SerializeField] public string thumbnail_url { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string started_at { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_language = body[TwitchWords.BROADCASTER_LANGUAGE].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.display_name = body[TwitchWords.DISPLAY_NAME].AsString;
			this.game_id = body[TwitchWords.GAME_ID].AsString;
			this.game_name = body[TwitchWords.GAME_NAME].AsString;
			this.id = body[TwitchWords.GAME_NAME].AsString;
			this.is_live = body[TwitchWords.IS_LIVE].AsBoolean;
			this.tags = body[TwitchWords.TAGS].AsJsonArray?.CastToStringArray;
			this.thumbnail_url = body[TwitchWords.THUMBNAIL_URL].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.SearchChannels;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SearchChannels;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string QUERY => TwitchWords.QUERY;
		public static string LIVE_ONLY => TwitchWords.LIVE_ONLY;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}