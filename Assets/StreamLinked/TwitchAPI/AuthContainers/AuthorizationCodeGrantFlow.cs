using System;
using System.Collections.Specialized;

using ScoredProductions.StreamLinked.API.Auth;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.AuthContainers {

	/// <summary>
	/// Two tier request flow, mainly used from Client to Server then Server to Twitch.
	/// <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#authorization-code-grant-flow">Auth Page</see>
	/// </summary>
	public struct AuthorizationCodeGrantFlow : IUserAccessToken {
		private readonly DateTime _oAuthAquireDate;
		public readonly DateTime OAuthAquireDate => this._oAuthAquireDate;
		public readonly AuthRequestType TypeEnum => AuthRequestType.AuthorizationCodeGrantFlow;
		public readonly string FlowName => nameof(AuthorizationCodeGrantFlow);
		public string Access_Token { get; set; }
		/// <summary>
		/// Requires call to Validate to populate with new value
		/// </summary>
		public long? Expires_In { get; set; }
		public string Refresh_Token { get; set; }
		public string[] Scope { get; set; }
		public string Token_Type { get; set; }

		public AuthorizationCodeGrantFlow(NameValueCollection data) {
			string aquireDate = data[TwitchWords.OAUTHAQUIREDATE] ?? "";
			if (!string.IsNullOrWhiteSpace(aquireDate) && DateTime.TryParse(aquireDate, out DateTime value)) {
				this._oAuthAquireDate = value;
			}
			else {
				this._oAuthAquireDate = DateTime.Now;
			}

			this.Access_Token = data[TwitchWords.ACCESS_TOKEN] ?? "";
			this.Refresh_Token = data[TwitchWords.REFRESH_TOKEN] ?? "";
			this.Scope = data[TwitchWords.SCOPE].Split('+');
			this.Token_Type = data[TwitchWords.TOKEN_TYPE] ?? "";
			// Added Later
			this.Expires_In = long.TryParse(data[TwitchWords.EXPIRES_IN], out long t) ? t : null;
		}

		public AuthorizationCodeGrantFlow(JsonValue data) {
			DateTime? aquireDate = data[TwitchWords.OAUTHAQUIREDATE].AsDateTime;
			if (aquireDate.HasValue) {
				this._oAuthAquireDate = aquireDate.Value;
			}
			else {
				this._oAuthAquireDate = DateTime.Now;
			}

			this.Access_Token = data[TwitchWords.ACCESS_TOKEN].AsString;
			this.Expires_In = data[TwitchWords.EXPIRES_IN].AsLong;
			this.Refresh_Token = data[TwitchWords.REFRESH_TOKEN].AsString;
			this.Token_Type = data[TwitchWords.TOKEN_TYPE].AsString;
			this.Scope = data[TwitchWords.SCOPE].AsJsonArray?.CastToStringArray;
		}
		
		public AuthorizationCodeGrantFlow(GetAuthorizationCodeToken data) {
			this._oAuthAquireDate = DateTime.Now;

			this.Access_Token = data.access_token;
			this.Expires_In = data.expires_in;
			this.Refresh_Token = data.refresh_token;
			this.Token_Type = data.token_type;
			this.Scope = data.scope;
		}
		
		public AuthorizationCodeGrantFlow(GetTokenRefresh data, GetValidatedTokenInfo validationInfo) {
			this._oAuthAquireDate = DateTime.Now;

			this.Access_Token = data.access_token;
			this.Refresh_Token = data.refresh_token;
			this.Token_Type = data.token_type;
			this.Scope = data.scope;
			this.Expires_In = validationInfo.expires_in;
		}

		public readonly JsonValue AsJsonValue() {
			return new JsonObject {
					{ TwitchWords.OAUTHAQUIREDATE, this.OAuthAquireDate },
					{ IAuthFlow.FLOWNAME, this.FlowName },
					{ IAuthFlow.AUTHREQUESTENUM, (int)this.TypeEnum },
					{ TwitchWords.ACCESS_TOKEN, this.Access_Token },
					{ TwitchWords.EXPIRES_IN, this.Expires_In },
					{ TwitchWords.REFRESH_TOKEN, this.Refresh_Token },
					{ TwitchWords.SCOPE, new JsonArray(this.Scope) },
					{ TwitchWords.TOKEN_TYPE, this.Token_Type }
				};
		}
	}
}