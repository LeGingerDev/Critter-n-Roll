using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Map : IShared {
		[field: SerializeField] public bool active { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string version { get; set; }
		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public int x { get; set; }
		[field: SerializeField] public int y { get; set; }

		public Map(JsonValue body) {
			this.active = body[TwitchWords.ACTIVE].AsBoolean;
			this.id = body[TwitchWords.ID].AsString;
			this.version = body[TwitchWords.VERSION].AsString;
			this.name = body[TwitchWords.NAME].AsString;
			this.x = body[TwitchWords.X].AsInteger;
			this.y = body[TwitchWords.Y].AsInteger;
		}
	}
}
