using System;
using System.Collections.Specialized;

using ScoredProductions.StreamLinked.API.Auth;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.AuthContainers {

	/// <summary>
	/// Short term client access token. Client activated with device code and refresh tokens
	/// <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#device-code-grant-flow">Auth Page</see>
	/// </summary>
	public struct DeviceCodeGrantFlow : IUserAccessToken {
		private readonly DateTime _oAuthAquireDate;
		public readonly AuthRequestType TypeEnum => AuthRequestType.DeviceCodeGrantFlow;
		public readonly DateTime OAuthAquireDate => this._oAuthAquireDate;
		public readonly string FlowName => nameof(DeviceCodeGrantFlow);

		public string Access_Token { get; set; }
		public long? Expires_In { get; set; }
		public string Refresh_Token { get; set; }
		public string Token_Type { get; set; }
		public string[] Scope { get; set; }


		public DeviceCodeGrantFlow(NameValueCollection data) {
			string aquireDate = data[TwitchWords.OAUTHAQUIREDATE] ?? "";
			if (!string.IsNullOrWhiteSpace(aquireDate) && DateTime.TryParse(aquireDate, out DateTime value)) {
				this._oAuthAquireDate = value;
			}
			else {
				this._oAuthAquireDate = DateTime.Now;
			}

			this.Access_Token = data[TwitchWords.ACCESS_TOKEN] ?? "";
			this.Expires_In = long.TryParse(data[TwitchWords.EXPIRES_IN], out long t) ? t : null;
			this.Token_Type = data[TwitchWords.TOKEN_TYPE] ?? "";
			this.Refresh_Token = data[TwitchWords.REFRESH_TOKEN] ?? "";
			this.Scope = data[TwitchWords.SCOPE].Split('+');
		}

		public DeviceCodeGrantFlow(JsonValue data) {
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

		public DeviceCodeGrantFlow(GetDeviceCodeGrantAuthorisation data) {
			this._oAuthAquireDate = DateTime.Now;

			this.Access_Token = data.access_token;
			this.Expires_In = data.expires_in;
			this.Refresh_Token = data.refresh_token;
			this.Token_Type = data.token_type;
			this.Scope = data.scope;
		}

		public DeviceCodeGrantFlow(GetTokenRefresh refresh) {
			this._oAuthAquireDate = DateTime.Now;

			this.Access_Token = refresh.access_token;
			this.Expires_In = null;
			this.Refresh_Token = refresh.refresh_token;
			this.Token_Type = refresh.token_type;
			this.Scope = refresh.scope;
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