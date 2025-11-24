using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Conduits {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-conduits">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateConduits : IConduits, IJsonRequest {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public int shard_count { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.Conduits;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateConduits;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(string id, int shard_count) {
			JsonObject body = new JsonObject() {
				{ TwitchWords.ID, id },
				{ TwitchWords.SHARD_COUNT, shard_count }
			};
			return JsonWriter.Serialize(body);
		}
	}
}