using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct CommunitySubGift {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public int total { get; set; }
		[field: SerializeField] public string sub_tier { get; set; }
		[field: SerializeField] public int cumulative_total { get; set; }

		public CommunitySubGift(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.total = body[TwitchWords.TOTAL].AsInteger;
			this.sub_tier = body[TwitchWords.SUB_TIER].AsString;
			this.cumulative_total = body[TwitchWords.CUMULATIVE_TOTAL].AsInteger;
		}
	}
}