using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Image {

		[field: SerializeField] public string url_1x { get; set; }
		[field: SerializeField] public string url_2x { get; set; }
		[field: SerializeField] public string url_4x { get; set; }

		public Image(JsonValue body) {
			this.url_1x = body[TwitchWords.URL_1X].AsString;
			this.url_2x = body[TwitchWords.URL_2X].AsString;
			this.url_4x = body[TwitchWords.URL_4X].AsString;
		}
	}
}