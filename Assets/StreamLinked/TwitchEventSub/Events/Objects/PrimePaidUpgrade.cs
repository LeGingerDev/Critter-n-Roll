using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct PrimePaidUpgrade {

		[field: SerializeField] public string sub_tier { get; set; }

		public PrimePaidUpgrade(JsonValue body) {
			this.sub_tier = body[TwitchWords.SUB_TIER].AsString;
		}
	}
}