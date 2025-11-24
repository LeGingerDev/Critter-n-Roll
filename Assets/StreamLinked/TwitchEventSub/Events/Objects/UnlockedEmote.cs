using System;

using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct UnlockedEmote {
		public string id { get; set; }
		public string name { get; set; }

		public UnlockedEmote(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.name = body[TwitchWords.NAME].AsString;
		}
	}
}