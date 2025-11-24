using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Emote {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string emote_set_id { get; set; }
		[field: SerializeField] public string owner_id { get; set; }
		[field: SerializeField] public string[] format { get; set; }

		[field: SerializeField] public string name { get; set; }

		[field: SerializeField] public int begin { get; set; }
		[field: SerializeField] public int end { get; set; }


		public Emote(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.emote_set_id = body[TwitchWords.EMOTE_SET_ID].AsString;
			this.owner_id = body[TwitchWords.OWNER_ID].AsString;
			this.format = body[TwitchWords.FORMAT].AsJsonArray?.CastToStringArray;

			this.name = body[TwitchWords.NAME].AsString;

			this.begin = body[TwitchWords.BEGIN].AsInteger;
			this.end = body[TwitchWords.END].AsInteger;
		}
	}
}