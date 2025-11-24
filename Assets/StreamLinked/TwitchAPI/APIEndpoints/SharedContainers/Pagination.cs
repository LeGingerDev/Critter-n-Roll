using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Pagination : IShared {
		[field: SerializeField] public string cursor { get; set; }

		public Pagination(JsonValue body) {
			this.cursor = body[TwitchWords.PAGINATION].AsString;
		}

		public Pagination(string data) {
			this.cursor = data;
		}
	}
}
