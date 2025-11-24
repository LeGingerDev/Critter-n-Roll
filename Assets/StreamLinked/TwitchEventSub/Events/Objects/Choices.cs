using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Choices {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public int bits_votes { get; set; }
		[field: SerializeField] public int channel_points_votes { get; set; }
		[field: SerializeField] public int votes { get; set; }

		public Choices(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.bits_votes = body[TwitchWords.BITS_VOTES].AsInteger;
			this.channel_points_votes = body[TwitchWords.CHANNEL_POINTS_VOTES].AsInteger;
			this.votes = body[TwitchWords.VOTES].AsInteger;
		}
	}
}