using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	// Not a object used by Twitch, something to help classify the data as its for a specific purpose

	[Serializable]
	public struct EventSubCosting : IShared {

		[field: SerializeField] public int total { get; set; }
		[field: SerializeField] public int total_cost { get; set; }
		[field: SerializeField] public int max_total_cost { get; set; }
		[field: SerializeField] public string points { get; set; }

		public EventSubCosting(JsonValue body) {
			this.total = body[TwitchWords.TOTAL].AsInteger;
			this.total_cost = body[TwitchWords.TOTAL_COST].AsInteger;
			this.max_total_cost = body[TwitchWords.MAX_TOTAL_COST].AsInteger;
			this.points = body[TwitchWords.POINTS].AsString;
		}
	}
}
