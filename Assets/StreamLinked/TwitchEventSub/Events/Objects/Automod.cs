using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Automod {

		[field: SerializeField] public string category { get; set; }
		[field: SerializeField] public int level { get; set; }
		[field: SerializeField] public Boundaries[] boundaries { get; set; }

		public Automod(JsonValue body) {
			this.category = body[TwitchWords.SET_ID].AsString;
			this.level = body[TwitchWords.ID].AsInteger;
			this.boundaries = body[TwitchWords.BOUNDARIES].AsJsonArray?.ToModelArray<Boundaries>();
		}
	}
}