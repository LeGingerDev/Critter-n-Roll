using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Games {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-top-games">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetTopGames : IGames {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public string box_art_url { get; set; }
		[field: SerializeField] public string igdb_id { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.name = body[TwitchWords.NAME].AsString;
			this.box_art_url = body[TwitchWords.BOX_ART_URL].AsString;
			this.igdb_id = body[TwitchWords.IGDB_ID].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetTopGames;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetTopGames;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
		public static string BEFORE => TwitchWords.BEFORE;
	}
}