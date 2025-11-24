using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Auth {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#client-credentials-grant-flow">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetClientCredentialsGrantFlow : IAuth {

		[field: SerializeField] public string access_token { get; set; }
		[field: SerializeField] public long expires_in { get; set; }
		[field: SerializeField] public string token_type { get; set; }

		public void Initialise(JsonValue body) {
			this.access_token = body[TwitchWords.ACCESS_TOKEN].AsString;
			this.expires_in = body[TwitchWords.EXPIRES_IN].AsLong;
			this.token_type = body[TwitchWords.TOKEN_TYPE].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.GetAuthToken;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetClientCredentialsGrantFlow;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string CLIENT_ID => TwitchWords.CLIENT_ID;
		public static string CLIENT_SECRET => TwitchWords.CLIENT_SECRET;
		public static string GRANT_TYPE => TwitchWords.GRANT_TYPE;
	}
}