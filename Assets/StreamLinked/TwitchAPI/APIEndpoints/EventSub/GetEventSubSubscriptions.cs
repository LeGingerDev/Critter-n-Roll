using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.WebSocketMessages;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.EventSub {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-eventsub-subscriptions">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetEventSubSubscriptions : IEventSub {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string version { get; set; }
		[field: SerializeField] public Condition condition { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public Transport transport { get; set; }
		[field: SerializeField] public int cost { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.version = body[TwitchWords.VERSION].AsString;
			this.condition = new Condition(body[TwitchWords.CONDITION]);
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.transport = new Transport(body[TwitchWords.TRANSPORT]);
			this.cost = body[TwitchWords.COST].AsInteger;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.EventSubSubscription;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetEventSubSubscriptions;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string STATUS => TwitchWords.STATUS;
		public static string TYPE => TwitchWords.TYPE;
		public static string USER_ID => TwitchWords.USER_ID;
		public static string SUBSCRIPTION_ID => TwitchWords.SUBSCRIPTION_ID;
		public static string AFTER => TwitchWords.AFTER;
	}
}