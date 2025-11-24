using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Label : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public string name { get; set; }

		public Label(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.name = body[TwitchWords.NAME].AsString;
		}
	}
}