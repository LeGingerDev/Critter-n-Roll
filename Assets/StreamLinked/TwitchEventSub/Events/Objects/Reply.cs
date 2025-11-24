using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Reply {

		[field: SerializeField] public string parent_message_id { get; set; }
		[field: SerializeField] public string parent_message_body { get; set; }
		[field: SerializeField] public string parent_user_id { get; set; }
		[field: SerializeField] public string parent_user_name { get; set; }
		[field: SerializeField] public string parent_user_login { get; set; }
		[field: SerializeField] public string thread_message_id { get; set; }
		[field: SerializeField] public string thread_user_id { get; set; }
		[field: SerializeField] public string thread_user_name { get; set; }
		[field: SerializeField] public string thread_user_login { get; set; }

		public Reply(JsonValue body) {
			this.parent_message_id = body[TwitchWords.PARENT_MESSAGE_ID].AsString;
			this.parent_message_body = body[TwitchWords.PARENT_MESSAGE_BODY].AsString;
			this.parent_user_id = body[TwitchWords.PARENT_USER_ID].AsString;
			this.parent_user_name = body[TwitchWords.PARENT_USER_NAME].AsString;
			this.parent_user_login = body[TwitchWords.PARENT_USER_LOGIN].AsString;
			this.thread_message_id = body[TwitchWords.THREAD_MESSAGE_ID].AsString;
			this.thread_user_id = body[TwitchWords.THREAD_USER_ID].AsString;
			this.thread_user_name = body[TwitchWords.THREAD_USER_NAME].AsString;
			this.thread_user_login = body[TwitchWords.THREAD_USER_LOGIN].AsString;
		}
	}
}