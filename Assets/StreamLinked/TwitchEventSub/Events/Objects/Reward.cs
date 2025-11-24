using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Reward {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public int cost { get; set; }
		[field: SerializeField] public string prompt { get; set; }

		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public UnlockedEmote unlocked_emote { get; set; }

		public Reward(JsonValue body) {
			this.type = body[TwitchWords.TYPE].AsString;
			this.id = body[TwitchWords.ID].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.cost = body[TwitchWords.COST].AsInteger;
			this.prompt = body[TwitchWords.PROMPT].AsString;

			this.unlocked_emote = new UnlockedEmote(body[TwitchWords.UNLOCKED_EMOTE]);
		}
	}
}