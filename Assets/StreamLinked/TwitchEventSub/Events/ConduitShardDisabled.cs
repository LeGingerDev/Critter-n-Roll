using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.EventSub.WebSocketMessages;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#conduit-shard-disabled-event">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ConduitShardDisabled : IEvent {

		[field: SerializeField] public string conduit_id { get; set; }
		[field: SerializeField] public string shard_id { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public Transport transport { get; set; }
		[field: SerializeField] public string method { get; set; }
		[field: SerializeField] public string callback { get; set; }
		[field: SerializeField] public string session_id { get; set; }
		[field: SerializeField] public string connected_at { get; set; }
		[field: SerializeField] public string disconnected_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Conduit_Shard_Disabled;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public ConduitShardDisabled(JsonValue body) {
			this.conduit_id = body[TwitchWords.CONDUIT_ID].AsString;
			this.shard_id = body[TwitchWords.SHARD_ID].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			this.transport = new Transport(body[TwitchWords.TRANSPORT]);
			this.method = body[TwitchWords.METHOD].AsString;
			this.callback = body[TwitchWords.CALLBACK].AsString;
			this.session_id = body[TwitchWords.SESSION_ID].AsString;
			this.connected_at = body[TwitchWords.CONNECTED_AT].AsString;
			this.disconnected_at = body[TwitchWords.DISCONNECTED_AT].AsString;
		}
	}
}