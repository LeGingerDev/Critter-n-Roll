using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Marker : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public int position_seconds { get; set; }
		[field: SerializeField] public string URL { get; set; }

		public Marker(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.position_seconds = body[TwitchWords.POSITION_SECONDS].AsInteger;
			this.URL = body[TwitchWords.URL].AsString;
		}
	}
}
