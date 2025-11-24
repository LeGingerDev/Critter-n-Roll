using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Images : IShared { // Formerly Image and Images, Twitch really needs to unify these objects so I dont have too...

		[field: SerializeField] public Color dark { get; set; }
		[field: SerializeField] public Color light { get; set; }

		[field: SerializeField] public string url_1x { get; set; }
		[field: SerializeField] public string url_2x { get; set; }
		[field: SerializeField] public string url_4x { get; set; }

		public Images(JsonValue body) {
			this.dark = new Color(body[TwitchWords.DARK]);
			this.light = new Color(body[TwitchWords.LIGHT]);

			this.url_1x = body[TwitchWords.URL_1X].AsString;
			this.url_2x = body[TwitchWords.URL_2X].AsString;
			this.url_4x = body[TwitchWords.URL_4X].AsString;
		}
	}
}
