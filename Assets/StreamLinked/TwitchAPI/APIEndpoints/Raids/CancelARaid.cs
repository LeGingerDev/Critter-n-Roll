using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Raids {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#cancel-a-raid">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CancelARaid : IRaids, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.Raids;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CancelARaid;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_raids,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;

		public readonly void Initialise(JsonValue value) { }
	}
}