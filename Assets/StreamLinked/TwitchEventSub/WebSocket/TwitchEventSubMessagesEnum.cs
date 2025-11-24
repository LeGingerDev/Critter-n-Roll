using System;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages.WebSocket {
	/// <summary>
	/// Metadata message_type
	/// </summary>

	[Serializable]
	public enum TwitchEventSubMessagesEnum {
		session_keepalive = 0,
		notification = 1,
		session_reconnect = 2,
		revocation = 3,
		session_welcome = 4,
	}
}