namespace ScoredProductions.StreamLinked.EventSub {
	public static class TwitchEventSubStatuses {
		/// <summary>
		/// The subscription is enabled.
		/// </summary>
		public const string enabled = "enabled";
		/// <summary>
		/// The subscription is pending verification of the specified callback URL.
		/// </summary>
		public const string webhook_callback_verification_pending = "webhook_callback_verification_pending";
		/// <summary>
		/// The specified callback URL failed verification.
		/// </summary>
		public const string webhook_callback_verification_failed = "webhook_callback_verification_pending";
		/// <summary>
		/// The notification delivery failure rate was too high.
		/// </summary>
		public const string notification_failures_exceeded = "notification_failures_exceeded";
		/// <summary>
		/// The authorization was revoked for one or more users specified in the <b>Condition</b> object.
		/// </summary>
		public const string authorization_revoked = "authorization_revoked";
		/// <summary>
		/// The moderator that authorized the subscription is no longer one of the broadcaster's moderators.
		/// </summary>
		public const string moderator_removed = "moderator_removed";
		/// <summary>
		/// One of the users specified in the <b>Condition</b> object was removed.
		/// </summary>
		public const string user_removed = "user_removed";
		/// <summary>
		/// The subscribed to subscription type and version is no longer supported.
		/// </summary>
		public const string version_removed = "version_removed";
		/// <summary>
		/// The client closed the connection.
		/// </summary>
		public const string websocket_disconnected = "websocket_disconnected";
		/// <summary>
		/// The client failed to respond to a ping message.
		/// </summary>
		public const string websocket_failed_ping_pong = "websocket_failed_ping_pong";
		/// <summary>
		/// The client sent a non-pong message. Clients may only send pong messages (and only in response to a ping message).
		/// </summary>
		public const string websocket_received_inbound_traffic = "websocket_received_inbound_traffic";
		/// <summary>
		/// The client failed to subscribe to events within the required time.
		/// </summary>
		public const string websocket_connection_unused = "websocket_connection_unused";
		/// <summary>
		/// The Twitch WebSocket server experienced an unexpected error.
		/// </summary>
		public const string websocket_internal_error = "websocket_internal_error";
		/// <summary>
		/// The Twitch WebSocket server timed out writing the message to the client.
		/// </summary>
		public const string websocket_network_timeout = "websocket_network_timeout";
		/// <summary>
		/// The Twitch WebSocket server experienced a network error writing the message to the client.
		/// </summary>
		public const string websocket_network_error = "websocket_network_error";
	}
}
