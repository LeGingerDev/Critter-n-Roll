using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct TopPredictors : IShared {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public int channel_points_used { get; set; }
		[field: SerializeField] public int channel_points_won { get; set; }

		public TopPredictors(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.channel_points_used = body[TwitchWords.CHANNEL_POINTS_USED].AsInteger;
			this.channel_points_won = body[TwitchWords.CHANNEL_POINTS_WON].AsInteger;
		}
	}
}
