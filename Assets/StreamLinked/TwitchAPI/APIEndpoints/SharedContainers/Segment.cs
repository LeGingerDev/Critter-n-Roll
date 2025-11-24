using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Segment : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string start_time { get; set; }
		[field: SerializeField] public string end_time { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string canceled_until { get; set; }
		[field: SerializeField] public Category category { get; set; }
		[field: SerializeField] public bool is_recurring { get; set; }

		public Segment(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.start_time = body[TwitchWords.START_TIME].AsString;
			this.end_time = body[TwitchWords.END_TIME].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.canceled_until = body[TwitchWords.CANCELED_UNTIL].AsString;
			this.category = new Category(body[TwitchWords.CATEGORY]);
			this.is_recurring = body[TwitchWords.IS_RECURRING].AsBoolean;
		}
	}
}
