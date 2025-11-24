using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Sub {

		[field: SerializeField] public string sub_tier { get; set; }
		[field: SerializeField] public bool is_prime { get; set; }
		[field: SerializeField] public int duration_months { get; set; }

		public Sub(JsonValue body) {
			this.sub_tier = body[TwitchWords.SUB_TIER].AsString;
			this.is_prime = body[TwitchWords.IS_PRIME].AsBoolean;
			this.duration_months = body[TwitchWords.DURATION_MONTHS].AsInteger;
		}
	}
}