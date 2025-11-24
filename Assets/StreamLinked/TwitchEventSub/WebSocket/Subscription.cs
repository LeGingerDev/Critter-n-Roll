using System;

using ScoredProductions.StreamLinked.API.EventSub;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages {
	[Serializable]
	public struct Subscription {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string version { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public int cost { get; set; }
		[field: SerializeField] public Condition condition { get; set; }
		[field: SerializeField] public string created_at { get; set; }

		public Subscription(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.version = body[TwitchWords.VERSION].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			this.cost = body[TwitchWords.COST].AsInteger;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.condition = new Condition(body[TwitchWords.CONDITION]);
		}

		public Subscription(CreateEventSubSubscription apiBody) {
			this.id = apiBody.id;
			this.type = apiBody.type;
			this.version = apiBody.version;
			this.status = apiBody.status;
			this.cost = apiBody.cost;
			this.condition = apiBody.condition;
			this.created_at = apiBody.created_at;
		}

		public Subscription(GetEventSubSubscriptions apiBody) {
			this.id = apiBody.id;
			this.type = apiBody.type;
			this.version = apiBody.version;
			this.status = apiBody.status;
			this.cost = apiBody.cost;
			this.condition = apiBody.condition;
			this.created_at = apiBody.created_at;
		}
    }
}