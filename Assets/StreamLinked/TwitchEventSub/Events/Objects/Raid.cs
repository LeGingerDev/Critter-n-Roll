using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Raid {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public int viewer_count { get; set; }

		[field: SerializeField] public string profile_image_url { get; set; }

		public Raid(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.viewer_count = body[TwitchWords.VIEWER_COUNT].AsInteger;

			this.profile_image_url = body[TwitchWords.PROFILE_IMAGE_URL].AsString;
		}
	}
}