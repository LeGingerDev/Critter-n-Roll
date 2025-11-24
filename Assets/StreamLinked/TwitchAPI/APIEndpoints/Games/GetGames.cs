using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Games {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-games">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetGames : IGames {

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
		public readonly string Endpoint => TwitchAPILinks.GetGames;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetGames;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string ID => TwitchWords.ID;
		public static string NAME => TwitchWords.NAME;
		public static string IGDB_ID => TwitchWords.IGDB_ID;
	}
}