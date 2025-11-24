using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Conduits {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-conduits">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateConduits : IConduits {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public int shard_count { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.Conduits;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateConduits;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(int shard_count) {
			JsonObject body = new JsonObject() {
				{ TwitchWords.SHARD_COUNT, shard_count }
			};
			return JsonWriter.Serialize(body);
		}
	}
}