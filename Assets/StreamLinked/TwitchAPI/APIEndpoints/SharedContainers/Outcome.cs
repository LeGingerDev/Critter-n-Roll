using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Outcome : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public int users { get; set; }
		[field: SerializeField] public int channel_points { get; set; }
		[field: SerializeField] public TopPredictors top_predictors { get; set; }
		[field: SerializeField] public string color { get; set; }

		public Outcome(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.users = body[TwitchWords.USERS].AsInteger;
			this.channel_points = body[TwitchWords.CHANNEL_POINTS].AsInteger;
			this.top_predictors = new TopPredictors(body[TwitchWords.TOP_PREDICTORS]);
			this.color = body[TwitchWords.COLOR].AsString;
		}
	}
}
