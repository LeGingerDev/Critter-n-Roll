using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Conduits {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-conduit">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct DeleteConduits : IConduits, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.Conduits;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.DeleteConduits;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string ID => TwitchWords.ID;

		public readonly void Initialise(JsonValue body) { }

	}
}