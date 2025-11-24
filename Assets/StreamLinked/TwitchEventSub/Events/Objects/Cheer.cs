using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Cheer {

		[field: SerializeField] public int bits { get; set; }

		public Cheer(JsonValue body) {
			this.bits = body[TwitchWords.BITS].AsInteger;
		}
	}
}