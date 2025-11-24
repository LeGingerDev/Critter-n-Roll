using System;

using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Whisper {
		public string text { get; set; }

		public Whisper(JsonValue body) {
			this.text = body[TwitchWords.TEXT].AsString;
		}
	}
}