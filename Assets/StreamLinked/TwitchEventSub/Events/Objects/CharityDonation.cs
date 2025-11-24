using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct CharityDonation {

		[field: SerializeField] public string charity_name { get; set; }
		[field: SerializeField] public Amount amount { get; set; }

		public CharityDonation(JsonValue body) {
			this.charity_name = body[TwitchWords.CHARITY_NAME].AsString;
			this.amount = new Amount(body[TwitchWords.AMOUNT]);
		}
	}
}