using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct AutomodTerms {

		[field: SerializeField] public string action { get; set; }
		[field: SerializeField] public string list { get; set; }
		[field: SerializeField] public string terms { get; set; }
		[field: SerializeField] public string from_automod { get; set; }

		public AutomodTerms(JsonValue body) {
			this.action = body[TwitchWords.ACTION].AsString;
			this.list = body[TwitchWords.LIST].AsString;
			this.terms = body[TwitchWords.TERMS].AsString;
			this.from_automod = body[TwitchWords.FROM_AUTOMOD].AsString;
		}
	}
}