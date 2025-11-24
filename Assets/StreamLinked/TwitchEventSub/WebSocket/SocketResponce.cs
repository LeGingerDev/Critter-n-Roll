using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages {
	[Serializable]
	public struct SocketResponse {

		[field: SerializeField] public Metadata metadata { get; set; }
		[field: SerializeField] public Payload payload { get; set; }

		public SocketResponse(JsonValue body) {
			this.metadata = new Metadata(body[TwitchWords.METADATA]);
			this.payload = new Payload(body[TwitchWords.PAYLOAD]);
		}
	}
}