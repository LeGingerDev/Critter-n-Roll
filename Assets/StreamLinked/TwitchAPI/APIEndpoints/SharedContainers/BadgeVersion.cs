using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct BadgeVersion : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string image_url_1x { get; set; }
		[field: SerializeField] public string image_url_2x { get; set; }
		[field: SerializeField] public string image_url_4x { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public string click_action { get; set; }
		[field: SerializeField] public string click_url { get; set; }

		public BadgeVersion(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.image_url_1x = body[TwitchWords.IMAGE_URL_1X].AsString;
			this.image_url_2x = body[TwitchWords.IMAGE_URL_2X].AsString;
			this.image_url_4x = body[TwitchWords.IMAGE_URL_4X].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.click_action = body[TwitchWords.CLICK_ACTION].AsString;
			this.click_url = body[TwitchWords.CLICK_URL].AsString;
		}
	}
}
