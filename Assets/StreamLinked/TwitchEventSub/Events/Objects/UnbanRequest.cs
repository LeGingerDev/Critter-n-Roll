using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct UnbanRequest {

		[field: SerializeField] public bool is_approved { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string moderator_message { get; set; }

		public UnbanRequest(JsonValue body) {
			this.is_approved = body[TwitchWords.IS_APPROVED].AsBoolean;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.moderator_message = body[TwitchWords.MODERATOR_MESSAGE].AsString;
		}
	}
}