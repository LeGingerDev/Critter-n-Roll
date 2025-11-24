using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {

	[Serializable]
	public struct Boundaries {

		[field: SerializeField] public int start_pos { get; set; }
		[field: SerializeField] public int end_pos { get; set; }

		public Boundaries(JsonValue body) {
			this.start_pos = body[TwitchWords.START_POS].AsInteger;
			this.end_pos = body[TwitchWords.END_POS].AsInteger;
		}
	}
}