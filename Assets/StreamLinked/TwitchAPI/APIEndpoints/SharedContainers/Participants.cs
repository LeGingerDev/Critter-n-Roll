using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Participants : IShared {

		[field: SerializeField] public string broadcaster_id { get; set; }

		public Participants(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
		}
	}
}
