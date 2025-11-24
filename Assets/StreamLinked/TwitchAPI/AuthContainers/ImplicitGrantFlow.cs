using System;
using System.Collections.Specialized;

using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.AuthContainers {

	/// <summary>
	/// Client access token. Webserver return request and refresh.
	/// <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#implicit-grant-flow">Auth Page</see>
	/// </summary>
	public struct ImplicitGrantFlow : IUserAccessToken {
		private readonly DateTime _oAuthAquireDate;
		public readonly DateTime OAuthAquireDate => this._oAuthAquireDate;
		public readonly AuthRequestType TypeEnum => AuthRequestType.ImplicitGrantFlow;
		public readonly string FlowName => nameof(ImplicitGrantFlow);

		public string Access_Token { get; set; }
		/// <summary>
		/// Requires call to Validate to populate with new value
		/// </summary>
		public long? Expires_In { get; set; }
		public string[] Scope { get; set; }
		public string Token_Type { get; set; }

		public ImplicitGrantFlow(NameValueCollection data) {
			string aquireDate = data[TwitchWords.OAUTHAQUIREDATE] ?? "";
			if (!string.IsNullOrWhiteSpace(aquireDate) && DateTime.TryParse(aquireDate, out DateTime value)) {
				this._oAuthAquireDate = value;
			} else {
				this._oAuthAquireDate = DateTime.Now;
			}

			this.Access_Token = data[TwitchWords.ACCESS_TOKEN] ?? "";
			this.Scope = data[TwitchWords.SCOPE].Split('+');
			this.Token_Type = data[TwitchWords.TOKEN_TYPE] ?? "";

			// Added Later
			this.Expires_In = long.TryParse(data[TwitchWords.EXPIRES_IN], out long t) ? t : null;
		}

		public ImplicitGrantFlow(JsonValue data) {
			DateTime? aquireDate = data[TwitchWords.OAUTHAQUIREDATE].AsDateTime;
			if (aquireDate.HasValue) {
				this._oAuthAquireDate = aquireDate.Value;
			} else {
				this._oAuthAquireDate = DateTime.Now;
			}

			this.Access_Token = data[TwitchWords.ACCESS_TOKEN].AsString;
			this.Expires_In = data[TwitchWords.EXPIRES_IN].AsLong;
			this.Token_Type = data[TwitchWords.TOKEN_TYPE].AsString;
			this.Scope = data[TwitchWords.SCOPE].AsJsonArray?.CastToStringArray;
		}

		public readonly JsonValue AsJsonValue() {
			return new JsonObject {
					{ TwitchWords.OAUTHAQUIREDATE, this.OAuthAquireDate },
					{ IAuthFlow.FLOWNAME, this.FlowName },
					{ IAuthFlow.AUTHREQUESTENUM, (int)this.TypeEnum },
					{ TwitchWords.ACCESS_TOKEN, this.Access_Token },
					{ TwitchWords.EXPIRES_IN, this.Expires_In },
					{ TwitchWords.SCOPE, new JsonArray(this.Scope) },
					{ TwitchWords.TOKEN_TYPE, this.Token_Type }
				};
		}
	}
}