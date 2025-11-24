using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Followers {

		[field: SerializeField] public int follow_duration_minutes { get; set; }

		public Followers(JsonValue body) {
			this.follow_duration_minutes = body[TwitchWords.FOLLOW_DURATION_MINUTES].AsInteger;
		}
	}
}