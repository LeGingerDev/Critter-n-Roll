using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Badge {

		[field: SerializeField] public string set_id { get; set; }
		/// <summary>
		/// Represents value: <c>ID</c>
		/// </summary>
		[field: SerializeField] public string badge_id { get; set; }
		[field: SerializeField] public string info { get; set; }

		public Badge(JsonValue body) {
			this.set_id = body[TwitchWords.SET_ID].AsString;
			this.badge_id = body[TwitchWords.ID].AsString;
			this.info = body[TwitchWords.INFO].AsString;
		}
	}
}