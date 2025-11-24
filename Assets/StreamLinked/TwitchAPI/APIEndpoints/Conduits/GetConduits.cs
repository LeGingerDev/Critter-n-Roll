using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Conduits {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-conduits">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetConduits : IConduits {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public int shard_count { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.shard_count = body[TwitchWords.SHARD_COUNT].AsInteger;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.Conduits;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetConduits;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();
	}
}