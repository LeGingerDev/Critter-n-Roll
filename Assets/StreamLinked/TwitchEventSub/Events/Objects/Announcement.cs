using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Announcement {

		[field: SerializeField] public string color { get; set; }

		public Announcement(JsonValue body) {
			this.color = body[TwitchWords.COLOR].AsString;
		}
	}
}