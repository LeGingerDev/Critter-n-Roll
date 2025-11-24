using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct DateRange : IShared {

		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string ended_at { get; set; }

		public DateRange(JsonValue body) {
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.ended_at = body[TwitchWords.ENDED_AT].AsString;
		}
	}
}
