using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {

	[Serializable]
	public struct Fragment {

		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string text { get; set; }
		[field: SerializeField] public Cheermote cheermote { get; set; }
		[field: SerializeField] public Emote emote { get; set; }
		[field: SerializeField] public Mention mention { get; set; }

		public Fragment(JsonValue body) {
			this.type = body[TwitchWords.TYPE].AsString;
			this.text = body[TwitchWords.TEXT].AsString;
			this.cheermote = new Cheermote(body[TwitchWords.CHEERMOTE]);
			this.emote = new Emote(body[TwitchWords.EMOTE]);
			this.mention = new Mention(body[TwitchWords.MENTION]);
		}
	}
}