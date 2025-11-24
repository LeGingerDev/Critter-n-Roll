using System;
using System.Collections.Specialized;

using ScoredProductions.StreamLinked.API.Auth;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.AuthContainers {

	/// <summary>
	/// App access token, basic access without scopes.
	/// <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#client-credentials-grant-flow">Auth Page</see>
	/// </summary>
	public struct ClientCredentialsFlow : IAppAccessToken {
		private readonly DateTime _oAuthAquireDate;
		public readonly DateTime OAuthAquireDate => this._oAuthAquireDate;
		public readonly string FlowName => nameof(ClientCredentialsFlow);
		public readonly AuthRequestType TypeEnum => AuthRequestType.ClientCredentialsGrantFlow;

		public string Access_Token { get; set; }
		public long? Expires_In { get; set; }
		public string Token_Type { get; set; }
		public readonly string[] Scope { 
			get => Array.Empty<string>();
			set { }
		}

		public ClientCredentialsFlow(NameValueCollection data) {
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
		}

		public ClientCredentialsFlow(JsonValue data) {
			DateTime? aquireDate = data[TwitchWords.OAUTHAQUIREDATE].AsDateTime;
			if (aquireDate.HasValue) {
				this._oAuthAquireDate = aquireDate.Value;
			}
			else {
				this._oAuthAquireDate = DateTime.Now;
			}

			this.Access_Token = data[TwitchWords.ACCESS_TOKEN].AsString;
			this.Expires_In = data[TwitchWords.EXPIRES_IN].AsLong;
			this.Token_Type = data[TwitchWords.TOKEN_TYPE].AsString;
		}

		public ClientCredentialsFlow(GetClientCredentialsGrantFlow data) {
			this._oAuthAquireDate = DateTime.Now;

			this.Access_Token = data.access_token;
			this.Expires_In = data.expires_in;
			this.Token_Type = data.token_type;
		}

		public readonly JsonValue AsJsonValue() {
			return new JsonObject {
					{ TwitchWords.OAUTHAQUIREDATE, this.OAuthAquireDate },
					{ IAuthFlow.FLOWNAME, this.FlowName },
					{ IAuthFlow.AUTHREQUESTENUM, (int)this.TypeEnum },
					{ TwitchWords.ACCESS_TOKEN, this.Access_Token },
					{ TwitchWords.EXPIRES_IN, this.Expires_In },
					{ TwitchWords.TOKEN_TYPE, this.Token_Type }
				};
		}
	}
}