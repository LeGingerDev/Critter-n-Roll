using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Auth {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#device-code-grant-flow">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetDeviceCodeGrantAuthorisation : IAuth {

		[field: SerializeField] public string access_token { get; set; }
		[field: SerializeField] public int expires_in { get; set; }
		[field: SerializeField] public string refresh_token { get; set; }
		[field: SerializeField] public string[] scope { get; set; }
		[field: SerializeField] public string token_type { get; set; }

		public void Initialise(JsonValue body) {
			this.access_token = body[TwitchWords.ACCESS_TOKEN].AsString;
			this.expires_in = body[TwitchWords.EXPIRES_IN].AsInteger;
			this.refresh_token = body[TwitchWords.REFRESH_TOKEN].AsString;
			this.scope = body[TwitchWords.SCOPE].AsJsonArray?.CastToStringArray;
			this.token_type = body[TwitchWords.TOKEN_TYPE].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.GetAuthToken;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetDeviceCodeGrantAuthorisation;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string LOCATION => TwitchWords.LOCATION;
		public static string CLIENT_ID => TwitchWords.CLIENT_ID;
		public static string SCOPES => TwitchWords.SCOPES;
		public static string DEVICE_CODE => TwitchWords.DEVICE_CODE;
		public static (string, string) GRANT_TYPE => (TwitchWords.GRANT_TYPE, Grant_Type_RFC);

		public const string Grant_Type_RFC = "urn:ietf:params:oauth:grant-type:device_code";
	}
}