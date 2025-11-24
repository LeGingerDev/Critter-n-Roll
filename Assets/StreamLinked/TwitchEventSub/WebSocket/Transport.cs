using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages {
	[Serializable]
	public struct Transport {

		[field: SerializeField] public string method { get; set; }
		[field: SerializeField] public string callback { get; set; }
		[field: SerializeField] public string secret { get; set; }
		[field: SerializeField] public string session_id { get; set; }
		[field: SerializeField] public string connected_at { get; set; }
		[field: SerializeField] public string disconnected_at { get; set; }

		public Transport(JsonValue body) {
			this.method = body[TwitchWords.METHOD].AsString;
			this.callback = body[TwitchWords.CALLBACK].AsString;
			this.secret = body[TwitchWords.SECRET].AsString;
			this.session_id = body[TwitchWords.SESSION_ID].AsString;
			this.connected_at = body[TwitchWords.CONNECTED_AT].AsString;
			this.disconnected_at = body[TwitchWords.DISCONNECTED_AT].AsString;
		}
	}
}