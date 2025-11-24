using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Cost : IShared {

		[field: SerializeField] public int amount { get; set; }
		[field: SerializeField] public string type { get; set; }

		public Cost(JsonValue body) {
			this.amount = body[TwitchWords.AMOUNT].AsInteger;
			this.type = body[TwitchWords.TYPE].AsString;
		}
	}
}
