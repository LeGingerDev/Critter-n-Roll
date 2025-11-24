using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Secrets : IShared {

		[field: SerializeField] public string content { get; set; }
		[field: SerializeField] public string active_at { get; set; }
		[field: SerializeField] public string expires_at { get; set; }

		public Secrets(JsonValue body) {
			this.content = body[TwitchWords.CONTENT].AsString;
			this.active_at = body[TwitchWords.ACTIVE_AT].AsString;
			this.expires_at = body[TwitchWords.EXPIRES_AT].AsString;
		}
	}
}
