using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Message {

		[field: SerializeField] public string message_id { get; set; }
		[field: SerializeField] public string text { get; set; }
		[field: SerializeField] public Fragment[] fragments { get; set; }

		[field: SerializeField] public Emote[] emotes { get; set; }

		public Message(JsonValue body) {
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
			this.text = body[TwitchWords.TEXT].AsString;
			this.fragments = body[TwitchWords.FRAGMENTS].AsJsonArray?.ToModelArray<Fragment>();

			this.emotes = body[TwitchWords.EMOTE].AsJsonArray?.ToModelArray<Emote>();
		}
	}
}