using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct MutedSegment : IShared {

		[field: SerializeField] public int duration { get; set; }
		[field: SerializeField] public int offset { get; set; }

		public MutedSegment(JsonValue body) {
			this.duration = body[TwitchWords.DURATION].AsInteger;
			this.offset = body[TwitchWords.OFFSET].AsInteger;
		}
	}
}
