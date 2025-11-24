using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct EventData : IShared {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string cooldown_end_time { get; set; }
		[field: SerializeField] public string expires_at { get; set; }
		[field: SerializeField] public int goal { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public LastContribution last_contribution { get; set; }
		[field: SerializeField] public int level { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public TopContribution[] top_contributions { get; set; }
		[field: SerializeField] public int total { get; set; }

		public EventData(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.cooldown_end_time = body[TwitchWords.COOLDOWN_END_TIME].AsString;
			this.expires_at = body[TwitchWords.EXPIRES_AT].AsString;
			this.goal = body[TwitchWords.GOAL].AsInteger;
			this.id = body[TwitchWords.ID].AsString;
			this.last_contribution = new LastContribution(body[TwitchWords.LAST_CONTRIBUTION]);
			this.level = body[TwitchWords.LEVEL].AsInteger;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.top_contributions = body[TwitchWords.TOP_CONTRIBUTIONS].AsJsonArray?.ToModelArray<TopContribution>();
			this.total = body[TwitchWords.TOTAL].AsInteger;
		}
	}
}
