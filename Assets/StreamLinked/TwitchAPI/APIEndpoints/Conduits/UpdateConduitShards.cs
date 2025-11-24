using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Conduits {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-conduit-shards">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateConduitShards : IConduits, IJsonRequest {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public Transport transport { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			JsonValue jtransport = body[TwitchWords.TRANSPORT];
			this.transport = new Transport(jtransport);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.ConduitShards;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateConduitShards;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(string conduit_id, Shards[] shards) {
			JsonObject body = new JsonObject() {
				{ TwitchWords.CONDUIT_ID, conduit_id }
			};
			JsonArray array = new JsonArray();
			foreach(Shards shard in shards) {
				array.Add(JsonWriter.StructToJsonValue(shard));
			}
			return JsonWriter.Serialize(body);
		}
	}
}