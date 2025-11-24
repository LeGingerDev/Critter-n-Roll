using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct GlobalCooldown {

		[field: SerializeField] public bool is_enabled { get; set; }
		[field: SerializeField] public int seconds { get; set; }

		public GlobalCooldown(JsonValue body) {
			this.is_enabled = body[TwitchWords.IS_ENABLED].AsBoolean;
			this.seconds = body[TwitchWords.SECONDS].AsInteger;
		}
	}
}