using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Product {

		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public int bits { get; set; }
		[field: SerializeField] public string sku { get; set; }
		[field: SerializeField] public bool in_development { get; set; }

		public Product(JsonValue body) {
			this.name = body[TwitchWords.NAME].AsString;
			this.bits = body[TwitchWords.BITS].AsInteger;
			this.sku = body[TwitchWords.SKU].AsString;
			this.in_development = body[TwitchWords.IN_DEVELOPMENT].AsBoolean;
		}
	}
}