using System;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages {

	[Serializable]
	public enum CloseMessageCodes {
		/// <summary>
		/// Indicates a problem with the server (similar to an HTTP 500 status code).
		/// </summary>
		InternalServerError = 4000,
		/// <summary>
		/// Sending outgoing messages to the server is prohibited with the exception of pong messages.
		/// </summary>
		ClientSentInboundTraffic = 4001,
		/// <summary>
		/// You must respond to ping messages with a pong message.
		/// </summary>
		ClientFailedPingPong = 4002,
		/// <summary>
		/// When you connect to the server, you must create a subscription within 10 seconds or the connection is closed. The time limit is subject to change.
		/// </summary>
		ConnectionUnused = 4003,
		/// <summary>
		/// When you receive a <b>session_reconnect</b> message, you have 30 seconds to reconnect to the server and close the old connection.
		/// </summary>
		ReconnectGraceTimeExpired = 4004,
		/// <summary>
		/// Transient network timeout.
		/// </summary>
		NetworkTimeout = 4005,
		/// <summary>
		/// Transient network error.
		/// </summary>
		NetworkError = 4006,
		/// <summary>
		/// The reconnect URL is invalid.
		/// </summary>
		InvalidReconnect = 4007,
	} 
}