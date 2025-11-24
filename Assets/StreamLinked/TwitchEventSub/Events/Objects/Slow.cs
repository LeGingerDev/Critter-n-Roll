using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Slow {

		[field: SerializeField] public int wait_time_seconds { get; set; }

		public Slow(JsonValue body) {
			this.wait_time_seconds = body[TwitchWords.WAIT_TIME_SECONDS].AsInteger;
		}
	}
}