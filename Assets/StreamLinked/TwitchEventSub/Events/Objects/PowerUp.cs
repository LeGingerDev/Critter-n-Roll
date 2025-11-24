using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct PowerUp {

		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public Emote emote { get; set; }
		[field: SerializeField] public string message_effect_id { get; set; }

		public PowerUp(JsonValue body) {
			this.type = body[TwitchWords.TYPE].AsString;
			this.emote = new Emote(body[TwitchWords.EMOTE]);
			this.message_effect_id = body[TwitchWords.MESSAGE_EFFECT_ID].AsString;
		}
	}
}