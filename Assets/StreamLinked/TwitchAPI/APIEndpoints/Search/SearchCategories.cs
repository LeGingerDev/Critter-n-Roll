using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Search {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#search-categories">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SearchCategories : ISearch {

		[field: SerializeField] public string box_art_url { get; set; }
		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public string id { get; set; }

		public void Initialise(JsonValue body) {
			this.box_art_url = body[TwitchWords.BOX_ART_URL].AsString;
			this.name = body[TwitchWords.NAME].AsString;
			this.id = body[TwitchWords.ID].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.SearchCategories;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SearchCategories;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string QUERY => TwitchWords.QUERY;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}