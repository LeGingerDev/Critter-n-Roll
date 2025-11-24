using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages {
	[Serializable]
	public struct Metadata {

		[field: SerializeField] public string message_id { get; set; }
		[field: SerializeField] public string message_type { get; set; }
		[field: SerializeField] public string message_timestamp { get; set; }
		[field: SerializeField] public string subscription_type { get; set; }
		[field: SerializeField] public string subscription_version { get; set; }

		public Metadata(JsonValue body) {
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
			this.message_type = body[TwitchWords.MESSAGE_TYPE].AsString;
			this.message_timestamp = body[TwitchWords.MESSAGE_TIMESTAMP].AsString;
			this.subscription_type = body[TwitchWords.SUBSCRIPTION_TYPE].AsString;
			this.subscription_version = body[TwitchWords.SUBSCRIPTION_VERSION].AsString;
		}

		public readonly JsonValue ToJSON() {
			return new JsonObject() {
				{ TwitchWords.MESSAGE_ID, this.message_id },
				{ TwitchWords.MESSAGE_TYPE, this.message_type },
				{ TwitchWords.MESSAGE_TIMESTAMP, this.message_timestamp },
				{ TwitchWords.SUBSCRIPTION_TYPE, this.subscription_type },
				{ TwitchWords.SUBSCRIPTION_VERSION, this.subscription_version },
			};
		}
	}
}