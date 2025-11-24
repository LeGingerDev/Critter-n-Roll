using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct ChannelPointsVoting {

		[field: SerializeField] public bool is_enabled { get; set; }
		[field: SerializeField] public int amount_per_vote { get; set; }

		public ChannelPointsVoting(JsonValue body) {
			this.is_enabled = body[TwitchWords.IS_ENABLED].AsBoolean;
			this.amount_per_vote = body[TwitchWords.AMOUNT_PER_VOTE].AsInteger;
		}
	}
}