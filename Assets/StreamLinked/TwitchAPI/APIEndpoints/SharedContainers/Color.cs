using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Color : IShared {

		[field: SerializeField] public Size animated { get; set; }
		[field: SerializeField] public Size @static { get; set; }

		public Color(JsonValue body) {
			this.animated = new Size(body[TwitchWords.ANIMATED]);
			this.@static = new Size(body[TwitchWords.STATIC]);
		}
	}
}
