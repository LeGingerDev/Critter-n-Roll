using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Auth {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/authentication/validate-tokens">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetValidatedTokenInfo : IAuth {

		[field: SerializeField] public string client_id { get; set; }
		[field: SerializeField] public string login { get; set; }
		[field: SerializeField] public string[] scope { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public long expires_in { get; set; }

		public void Initialise(JsonValue body) {
			this.client_id = body[TwitchWords.CLIENT_ID].AsString;
			this.login = body[TwitchWords.LOGIN].AsString;
			this.scope = body[TwitchWords.SCOPE].AsJsonArray?.CastToStringArray;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.expires_in = body[TwitchWords.EXPIRES_IN].AsLong;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetTokenValidation;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetValidatedTokenInfo;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string AUTHORIZATION => TwitchWords.AUTHORIZATION;
	}
}