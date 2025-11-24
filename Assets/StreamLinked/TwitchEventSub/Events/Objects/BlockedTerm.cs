using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct BlockedTerm {

		[field: SerializeField] public TermsFound[] terms_found { get; set; }

		public BlockedTerm(JsonValue body) {
			this.terms_found = body[TwitchWords.TERMS_FOUND].AsJsonArray?.ToModelArray<TermsFound>();
		}
	}
}