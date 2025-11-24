using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct MaxPerStream {

		[field: SerializeField] public bool is_enabled { get; set; }
		[field: SerializeField] public int value { get; set; }

		public MaxPerStream(JsonValue body) {
			this.is_enabled = body[TwitchWords.IS_ENABLED].AsBoolean;
			this.value = body[TwitchWords.VALUE].AsInteger;
		}
	}
}