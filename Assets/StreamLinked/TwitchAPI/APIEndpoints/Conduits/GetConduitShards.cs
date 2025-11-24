using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Conduits {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-conduit-shards">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetConduitShards : IConduits {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public Transport transport { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			JsonValue jtransport = body[TwitchWords.TRANSPORT];
			this.transport = new Transport(jtransport);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.ConduitShards;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetConduitShards;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string CONDUIT_ID => TwitchWords.CONDUIT_ID;
		public static string STATUS => TwitchWords.STATUS;
		public static string AFTER => TwitchWords.AFTER;
	}
}