using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Resub {

		[field: SerializeField] public int cumulative_months { get; set; }
		[field: SerializeField] public int duration_months { get; set; }
		[field: SerializeField] public int streak_months { get; set; }
		[field: SerializeField] public string sub_tier { get; set; }
		[field: SerializeField] public bool is_prime { get; set; }
		[field: SerializeField] public bool is_gift { get; set; }
		[field: SerializeField] public bool gifter_is_anonymous { get; set; }
		[field: SerializeField] public string gifter_user_id { get; set; }
		[field: SerializeField] public string gifter_user_name { get; set; }
		[field: SerializeField] public string gifter_user_login { get; set; }

		public Resub(JsonValue body) {
			this.cumulative_months = body[TwitchWords.CUMULATIVE_MONTHS].AsInteger;
			this.duration_months = body[TwitchWords.DURATION_MONTHS].AsInteger;
			this.streak_months = body[TwitchWords.STREAK_MONTHS].AsInteger;
			this.sub_tier = body[TwitchWords.SUB_TIER].AsString;
			this.is_prime = body[TwitchWords.IS_PRIME].AsBoolean;
			this.is_gift = body[TwitchWords.IS_GIFT].AsBoolean;
			this.gifter_is_anonymous = body[TwitchWords.GIFTER_IS_ANONYMOUS].AsBoolean;
			this.gifter_user_id = body[TwitchWords.GIFTER_USER_ID].AsString;
			this.gifter_user_name = body[TwitchWords.GIFTER_USER_NAME].AsString;
			this.gifter_user_login = body[TwitchWords.GIFTER_USER_LOGIN].AsString;
		}
	}
}