using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct LastContribution : IShared {

		[field: SerializeField] public int total { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string user { get; set; }

		public LastContribution(JsonValue body) {
			this.total = body[TwitchWords.TOTAL].AsInteger;
			this.type = body[TwitchWords.TYPE].AsString;
			this.user = body[TwitchWords.USER].AsString;
		}
	}
}
