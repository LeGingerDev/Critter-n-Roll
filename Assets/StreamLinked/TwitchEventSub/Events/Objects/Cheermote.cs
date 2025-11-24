using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Cheermote {

		[field: SerializeField] public string prefix { get; set; }
		[field: SerializeField] public int bits { get; set; }
		[field: SerializeField] public int tier { get; set; }

		public Cheermote(JsonValue body) {
			this.prefix = body[TwitchWords.PREFIX].AsString;
			this.bits = body[TwitchWords.BITS].AsInteger;
			this.tier = body[TwitchWords.TIER].AsInteger;
		}
	}
}