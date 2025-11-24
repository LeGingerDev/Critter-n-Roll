using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Boundary {

		[field: SerializeField] public string term_id { get; set; }
		[field: SerializeField] public int end_pos { get; set; }

		public Boundary(JsonValue body) {
			this.term_id = body[TwitchWords.TERM_ID].AsString;
			this.end_pos = body[TwitchWords.END_POS].AsInteger;
		}
	}
}