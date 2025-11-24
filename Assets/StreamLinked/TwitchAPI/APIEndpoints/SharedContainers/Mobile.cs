using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Mobile : IShared {

		[field: SerializeField] public string viewer_url { get; set; }

		public Mobile(JsonValue body) {
			this.viewer_url = body[TwitchWords.VIEWER_URL].AsString;
		}
	}
}
