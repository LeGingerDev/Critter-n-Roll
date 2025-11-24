using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct BitsBadgeTier {

		[field: SerializeField] public int tier { get; set; }

		public BitsBadgeTier(JsonValue body) {
			this.tier = body[TwitchWords.TIER].AsInteger;
		}
	}
}