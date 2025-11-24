using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Vacation : IShared {

		[field: SerializeField] public string start_time { get; set; }
		[field: SerializeField] public string end_time { get; set; }

		public Vacation(JsonValue body) {
			this.start_time = body[TwitchWords.START_TIME].AsString;
			this.end_time = body[TwitchWords.END_TIME].AsString;
		}
	}
}
