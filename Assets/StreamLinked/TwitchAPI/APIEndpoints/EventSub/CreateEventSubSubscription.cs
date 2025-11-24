using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.WebSocketMessages;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.EventSub {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-eventsub-subscription">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateEventSubSubscription : IEventSub, IJsonRequest {

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

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.EventSubSubscription;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateEventSubSubscription;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string type,
												string version,
												Condition conditions,
												Transport transport) {
			JsonObject body = new JsonObject() {
					{ TwitchWords.TYPE, type },
					{ TwitchWords.VERSION, version },
					{ TwitchWords.CONDITION, JsonWriter.StructToJsonValue(conditions) },
					{ TwitchWords.TRANSPORT, JsonWriter.StructToJsonValue(transport) },
				};
			return JsonWriter.Serialize(body);
		}

	}
}