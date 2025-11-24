using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct SubGift {

		[field: SerializeField] public int duration_months { get; set; }
		[field: SerializeField] public int cumulative_total { get; set; }
		[field: SerializeField] public string recipient_user_id { get; set; }
		[field: SerializeField] public string recipient_user_name { get; set; }
		[field: SerializeField] public string recipient_user_login { get; set; }
		[field: SerializeField] public string sub_tier { get; set; }
		[field: SerializeField] public string community_gift_id { get; set; }

		public SubGift(JsonValue body) {
			this.duration_months = body[TwitchWords.DURATION_MONTHS].AsInteger;
			this.cumulative_total = body[TwitchWords.CUMULATIVE_TOTAL].AsInteger;
			this.recipient_user_id = body[TwitchWords.RECIPIENT_USER_ID].AsString;
			this.recipient_user_name = body[TwitchWords.RECIPIENT_USER_NAME].AsString;
			this.recipient_user_login = body[TwitchWords.RECIPIENT_USER_LOGIN].AsString;
			this.sub_tier = body[TwitchWords.SUB_TIER].AsString;
			this.community_gift_id = body[TwitchWords.COMMUNITY_GIFT_ID].AsString;
		}
	}
}