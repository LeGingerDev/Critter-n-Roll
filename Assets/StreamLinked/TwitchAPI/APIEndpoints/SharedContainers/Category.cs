using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Category : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string name { get; set; }

		public Category(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.name = body[TwitchWords.NAME].AsString;
		}
	}
}
