using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Tiers : IShared {

		[field: SerializeField] public int min_bits { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string color { get; set; }
		[field: SerializeField] public Images images { get; set; }
		[field: SerializeField] public bool can_cheer { get; set; }
		[field: SerializeField] public bool show_in_bits_card { get; set; }

		public Tiers(JsonValue body) {
			this.min_bits = body[TwitchWords.MIN_BITS].AsInteger;
			this.id = body[TwitchWords.ID].AsString;
			this.color = body[TwitchWords.COLOR];
			this.images = new Images(body[TwitchWords.IMAGES]);
			this.can_cheer = body[TwitchWords.CAN_CHEER].AsBoolean;
			this.show_in_bits_card = body[TwitchWords.SHOW_IN_BITS_CARD].AsBoolean;
		}
	}
}
