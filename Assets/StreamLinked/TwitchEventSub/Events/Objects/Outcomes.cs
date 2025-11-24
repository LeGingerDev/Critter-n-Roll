using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Outcomes {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string color { get; set; }
		[field: SerializeField] public int users { get; set; }
		[field: SerializeField] public int channel_points { get; set; }
		[field: SerializeField] public TopPredictors[] top_predictors { get; set; }

		public Outcomes(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.color = body[TwitchWords.COLOR].AsString;
			this.users = body[TwitchWords.USERS].AsInteger;
			this.channel_points = body[TwitchWords.CHANNEL_POINTS].AsInteger;
			this.top_predictors = body[TwitchWords.TOP_PREDICTORS].AsJsonArray?.ToModelArray<TopPredictors>();
		}
	}
}
