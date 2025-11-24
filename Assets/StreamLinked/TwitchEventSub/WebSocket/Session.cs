using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages {
	[Serializable]
	public struct Session {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public int keepalive_timeout_seconds { get; set; }
		[field: SerializeField] public string reconnect_url { get; set; }
		[field: SerializeField] public string connected_at { get; set; }

		public Session(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			this.keepalive_timeout_seconds = body[TwitchWords.KEEPALIVE_TIMEOUT_SECONDS].AsInteger;
			this.reconnect_url = body[TwitchWords.RECONNECT_URL].AsString;
			this.connected_at = body[TwitchWords.CONNECTED_AT].AsString;
		}

	}
}