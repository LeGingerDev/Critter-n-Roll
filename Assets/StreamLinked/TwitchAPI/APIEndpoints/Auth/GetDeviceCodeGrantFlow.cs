using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Auth {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#device-code-grant-flow">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetDeviceCodeGrantFlow : IAuth {

		[field: SerializeField] public string device_code { get; set; }
		[field: SerializeField] public int expires_in { get; set; }
		[field: SerializeField] public int interval { get; set; }
		[field: SerializeField] public string user_code { get; set; }
		[field: SerializeField] public string verification_uri { get; set; }

		public void Initialise(JsonValue body) {
			this.device_code = body[TwitchWords.DEVICE_CODE].AsString;
			this.expires_in = body[TwitchWords.EXPIRES_IN].AsInteger;
			this.interval = body[TwitchWords.INTERVAL].AsInteger;
			this.user_code = body[TwitchWords.USER_CODE].AsString;
			this.verification_uri = body[TwitchWords.VERIFICATION_URI].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.GetDeviceToken;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetDeviceCodeGrantFlow;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string CLIENT_ID => TwitchWords.CLIENT_ID;
		public static string SCOPES => TwitchWords.SCOPES;
	}
}