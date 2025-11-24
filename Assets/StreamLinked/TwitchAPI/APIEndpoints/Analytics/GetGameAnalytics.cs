using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Analytics {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-game-analytics">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetGameAnalytics : IAnalytics {

		[field: SerializeField] public string game_id { get; set; }
		[field: SerializeField] public string url { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public DateRange date_range { get; set; }

		public void Initialise(JsonValue body) {
			this.game_id = body[TwitchWords.GAME_ID].AsString;
			this.url = body[TwitchWords.URL].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.date_range = new DateRange(body[TwitchWords.DATE_RANGE]);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetGameAnalytics;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetGameAnalytics;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.analytics_read_games,
		};

		public static string GAME_ID => TwitchWords.GAME_ID;
		public static string TYPE => TwitchWords.TYPE;
		public static string STARTED_AT => TwitchWords.STARTED_AT;
		public static string ENDED_AT => TwitchWords.ENDED_AT;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}
