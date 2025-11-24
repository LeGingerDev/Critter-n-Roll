using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Reward : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string prompt { get; set; }
		[field: SerializeField] public int cost { get; set; }

		public Reward(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.prompt = body[TwitchWords.PROMPT].AsString;
			this.cost = body[TwitchWords.COST].AsInteger;
		}
	}
}
