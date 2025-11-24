using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Size : IShared {

		[field: SerializeField] public string one { get; set; }
		[field: SerializeField] public string onehalf { get; set; } // ...Twitch come on
		[field: SerializeField] public string two { get; set; }
		[field: SerializeField] public string three { get; set; }
		[field: SerializeField] public string four { get; set; }

		public Size(JsonValue body) {
			this.one = body[TwitchWords.ONE].AsString;
			this.onehalf = body[TwitchWords.ONEHALF].AsString;
			this.two = body[TwitchWords.TWO].AsString;
			this.three = body[TwitchWords.THREE].AsString;
			this.four = body[TwitchWords.FOUR].AsString;
		}
	}
}
