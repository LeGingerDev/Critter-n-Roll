using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Amount : IShared {

		[field: SerializeField] public int value { get; set; }
		[field: SerializeField] public int decimal_places { get; set; }
		[field: SerializeField] public string currency { get; set; }

		public Amount(JsonValue body) {
			this.value = body[TwitchWords.VALUE].AsInteger;
			this.decimal_places = body[TwitchWords.DECIMAL_PLACES].AsInteger;
			this.currency = body[TwitchWords.CURRENCY].AsString;
		}
	}
}
