using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.AuthContainers {

	//https://docs.unity3d.com/ScriptReference/ScriptableObject.html

	[Serializable]
	[CreateAssetMenu(menuName = "StreamLinked/OAuth Token")]
	public partial class TokenInstance : ScriptableObject {


		// Device code settings
		public const string AuthPending = "authorization_pending";
		public const string InvalidDeviceCode = "invalid device code";
		public const string InvalidRefreshToken = "Invalid refresh token";

		private bool TokenUpdateRequired;

		[SerializeField]
		private AuthRequestType authenticationType;
		public AuthRequestType AuthenticationType
		{
			get => this.authenticationType;
			set
			{
				if (this.authenticationType != value) {
					this.authenticationType = value;
					this.TokenUpdateRequired = true;
					this.OAuthToken = null;
				}
			}
		}

		[SerializeField]
		private FlaggedEnum<TwitchScopesEnum> requestScopes = new FlaggedEnum<TwitchScopesEnum>();
		public FlaggedEnum<TwitchScopesEnum> RequestScopes => new FlaggedEnum<TwitchScopesEnum>(this.requestScopes);

		[Tooltip("Turn on if you would like Unity to host a local server to receive the OAuth request. Turn off if the RedirectURI address provided manages it for you and can return it to Unity.")]
		[SerializeField]
		private bool createLocalHostServer = true;
		public bool CreateLocalHostServer
		{
			get => this.createLocalHostServer;
			set { this.createLocalHostServer = value; }
		}

		[SerializeField]
		private string redirectURI = "http://localhost:3000/";
		/// <summary>
		/// Set in the Twitch Dev Console: 
		/// Will receive the result of all client authorizations: 
		/// either an access token or a failure message. 
		/// This must exactly match the redirect_uri parameter passed to the authorization endpoint. 
		/// When testing locally, you can set this to http://localhost. 
		/// A maximum of 10 redirect URLs is supported.
		/// </summary>
		public string RedirectURI
		{
			get => redirectURI;
			set
			{ this.redirectURI = value; }
		}

		[SerializeField]
		private bool autoRetrieveNewAuth = true;
		public bool AutoRetrieveNewAuth
		{
			get => autoRetrieveNewAuth;
			set { this.autoRetrieveNewAuth = value; }
		}

		[SerializeField]
		private string userProvidedWebResponse = TwitchAPIClient.WebResponseBackup;
		public string UserProvidedWebResponse
		{
			get => this.userProvidedWebResponse;
			set { this.userProvidedWebResponse = value; }
		}

		[SerializeField]
		private string userProvidedJSCode;
		public string UserProvidedJSCode
		{
			get => this.userProvidedJSCode;
			set { this.userProvidedJSCode = value; }
		}

		[SerializeField]
		private bool manualRetrieval = false;
		public bool ManualRetrieval
		{
			get => this.manualRetrieval;
			set { this.manualRetrieval = value; }
		}

		[SerializeField, Tooltip("In Milliseconds")]
		private int pingInterval = 10000; // 10 seconds
		/// <summary>
		/// Milliseconds
		/// </summary>
		public int PingInterval
		{
			get => this.pingInterval;
			set { this.pingInterval = value; }
		}

		[SerializeField]
		private int pingRetries = 6; // 60 seconds
		public int PingRetries
		{
			get => this.pingRetries;
			set { this.pingRetries = value; }
		}

		[SerializeField]
		private bool startNewOnRefreshFail = true;
		public bool StartNewOnRefreshFail
		{
			get => this.startNewOnRefreshFail;
			set { this.startNewOnRefreshFail = value; }
		}

		[field: NonSerialized] public string ExpectedDeviceCode { get; set; }

		[SerializeField]
		private string tokenID = Guid.NewGuid().ToString();
		public string TokenID
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.tokenID)) {
					this.tokenID = Guid.NewGuid().ToString();
				}
				return tokenID;
			}
		}

		/// <summary>
		/// Checks if the backing OAuthToken is null without attempting to load it in
		/// </summary>
		public bool HasToken => this.oAuthToken != null;

		private IAuthFlow oAuthToken;
		/// <summary>
		/// Warning, Not thread safe, do not check if this value is null if you know its null, use <c>HasToken</c> instead
		/// </summary>
		public IAuthFlow OAuthToken
		{
			get {
				if (this.oAuthToken == null) {
					this.LoadTokenFromSettings();
				}
				return this.oAuthToken;
			}
			set
			{
				this.TokenUpdateRequired = true; // Set true, if check fails it stays true
				if (value == null) {
					oAuthToken = null;
					this.ClearTokenFromSettings();
					return;
				}

				if (value.ExpiryDate > DateTime.Now
					&& !string.IsNullOrWhiteSpace(value.FlowName)
					&& !string.IsNullOrWhiteSpace(value.Token_Type)
					&& !string.IsNullOrWhiteSpace(value.Access_Token)) {
					switch (value) {
						case IAppAccessToken:
							if (authenticationType != AuthRequestType.ClientCredentialsGrantFlow) {
								throw new ArgumentException("Provided auth doesnt match TokenInstances provided settings and has been rejected");
							}
							break;
						case IUserAccessToken iuat:
							// scope check
							TwitchScopesEnum[] tokenScopes = new TwitchScopesEnum[iuat.Scope.Length];
							for (int x = 0; x < tokenScopes.Length; x++) {
								tokenScopes[x] = TwitchScopes.GetLinkedStringToEnum(iuat.Scope[x]);
							}
							TwitchScopesEnum[] storedScopes = this.RequestScopes.GetAllFlaggedAsArray();
							for (int x = 0; x < storedScopes.Length; x++) {
								bool found = false;
								for (int y = 0; y < tokenScopes.Length; y++) {
									if (storedScopes[x] == tokenScopes[y]) {
										found = true;
										break;
									}
								}
								if (!found) {
									throw new ArgumentException("Provided auth doesnt match TokenInstances provided settings and has been rejected");
								}
							}

							switch (iuat) {
								case ImplicitGrantFlow:
									if (authenticationType != AuthRequestType.ImplicitGrantFlow) {
										throw new ArgumentException("Provided auth doesnt match TokenInstances provided settings and has been rejected");
									}
									break;
								case AuthorizationCodeGrantFlow:
									if (authenticationType != AuthRequestType.AuthorizationCodeGrantFlow) {
										throw new ArgumentException("Provided auth doesnt match TokenInstances provided settings and has been rejected");
									}
									break;
								case DeviceCodeGrantFlow:
									if (authenticationType != AuthRequestType.DeviceCodeGrantFlow) {
										throw new ArgumentException("Provided auth doesnt match TokenInstances provided settings and has been rejected");
									}
									break;
							}
							break;
						default:
							throw new ArgumentException("Provided auth doesnt match TokenInstances provided settings and has been rejected");
					}
					// Success
					this.oAuthToken = value;
					this.TokenUpdateRequired = false;
				}
				else {
					throw new ArgumentException("Provided auth has been rejected, it is either incomplete or is already expired.");
				}
			}
		}

		private void OnEnable() {
			this.LoadTokenFromSettings();
		}

		public bool CheckRefreshNeeded(bool log = false, bool threadSafe = true) {
			if (this.TokenUpdateRequired) {
				return true;
			}
			if (this.oAuthToken == null && threadSafe) {
				this.LoadTokenFromSettings(log);
			}

			if (this.oAuthToken != null 
				&& this.oAuthToken.ExpiryDate > DateTime.Now
				&& !string.IsNullOrWhiteSpace(this.oAuthToken.FlowName)
				&& !string.IsNullOrWhiteSpace(this.oAuthToken.Token_Type)
				&& !string.IsNullOrWhiteSpace(this.oAuthToken.Access_Token)) {

				switch (this.oAuthToken) {
					case IAppAccessToken:
						if (authenticationType != AuthRequestType.ClientCredentialsGrantFlow) {
							return this.TokenUpdateRequired = true;
						}
						break;
					case IUserAccessToken iuat:
						// scope check
						TwitchScopesEnum[] tokenScopes = new TwitchScopesEnum[iuat.Scope.Length];
						for (int x = 0; x < tokenScopes.Length; x++) {
							tokenScopes[x] = TwitchScopes.GetLinkedStringToEnum(iuat.Scope[x]);
						}
						TwitchScopesEnum[] storedScopes = this.RequestScopes.GetAllFlaggedAsArray();
						for (int x = 0; x < storedScopes.Length; x++) {
							bool found = false;
							for (int y = 0; y < tokenScopes.Length; y++) {
								if (storedScopes[x] == tokenScopes[y]) {
									found = true;
									break;
								}
							}
							if (!found) {
								return this.TokenUpdateRequired = true;
							}
						}

						switch (iuat) {
							case ImplicitGrantFlow:
								if (authenticationType != AuthRequestType.ImplicitGrantFlow) {
									return this.TokenUpdateRequired = true;
								}
								break;
							case AuthorizationCodeGrantFlow:
								if (authenticationType != AuthRequestType.AuthorizationCodeGrantFlow) {
									return this.TokenUpdateRequired = true;
								}
								break;
							case DeviceCodeGrantFlow:
								if (authenticationType != AuthRequestType.DeviceCodeGrantFlow) {
									return this.TokenUpdateRequired = true;
								}
								break;
						}
						break;
					default:
						return this.TokenUpdateRequired = true;
						
				}
				this.TokenUpdateRequired = false;
			}
			else {
				this.TokenUpdateRequired = true;
			}
			return this.TokenUpdateRequired;
		}

		/// <summary>
		/// Method to add scope, notifies to get a new token on new scope added
		/// </summary>
		public void AddScope(params TwitchScopesEnum[] scopes) {
			if (this.RequestScopes.Add(scopes)) {
				this.TokenUpdateRequired = true;
			}
		}

		/// <summary>
		/// Method to add scope, notifies to get a new token on new scope added
		/// </summary>
		public void AddScope(FlaggedEnum<TwitchScopesEnum> scopes) {
			if (this.RequestScopes.Add(scopes)) {
				this.TokenUpdateRequired = true;
			}
		}

		/// <summary>
		/// Removes scopes from current scopes
		/// </summary>
		public void RemoveScope(params TwitchScopesEnum[] scopes) {
			this.RequestScopes.Subtract(scopes);
		}

		/// <summary>
		/// Removes scopes from current scopes
		/// </summary>
		public void RemoveScope(FlaggedEnum<TwitchScopesEnum> scopes) {
			this.RequestScopes.Subtract(scopes);
		}

		public JsonValue RetrieveTokenAsJson() {
			return new JsonObject() {
				{ TwitchWords.ID, this.TokenID },
				{ TwitchWords.DATA, this.oAuthToken?.AsJsonValue() ?? "{ }" },
			};
		}

		public bool LoadTokenFromSettings(bool log = false) {
			if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchAuthenticationTokens, out string tokens, log)) {
				JsonValue parsed = JsonReader.Parse(tokens);
				JsonValue arrayValue = parsed[TwitchWords.DATA];
				if (arrayValue.IsJsonArray) {
					JsonArray arrayContainer = arrayValue.AsJsonArray;
					foreach (JsonValue container in arrayContainer) {
						if (container[TwitchWords.ID] == this.tokenID) {
							this.oAuthToken = this.AuthenticationType switch {
								AuthRequestType.ImplicitGrantFlow => new ImplicitGrantFlow(container[TwitchWords.DATA]),
								AuthRequestType.AuthorizationCodeGrantFlow => new AuthorizationCodeGrantFlow(container[TwitchWords.DATA]),
								AuthRequestType.DeviceCodeGrantFlow => new DeviceCodeGrantFlow(container[TwitchWords.DATA]),
								AuthRequestType.ClientCredentialsGrantFlow => new ClientCredentialsFlow(container[TwitchWords.DATA]),
								_ => throw new NotImplementedException("Authentication type not recognised")
							};
							return true;
						}
					}
				}
			}
			return false;
		}

		public void ClearTokenFromSettings(bool log = false) {
			if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchAuthenticationTokens, out string tokens, log)) {
				JsonValue arrayValue = JsonReader.Parse(tokens);
				JsonValue? foundValue = null;
				if (arrayValue.IsJsonArray) {
					JsonArray arrayContainer = arrayValue.AsJsonArray;

					int x = 0;
					for (; x < arrayContainer.Count; x++) {
						JsonValue value = arrayContainer[x];
						if (value[TwitchWords.ID] == this.tokenID) {
							foundValue = value;
							break;
						}
					}

					if (foundValue.HasValue) {
						arrayContainer.Remove(x);
						InternalSettingsStore.EditSetting(SavedSettings.TwitchAuthenticationTokens, arrayContainer.ToString(), log);
					}
				}
			}
		}

		public void PerformScopeCheck(in APIScopeWarning ScopeSettings, in IScope reference) {
			if (ScopeSettings != APIScopeWarning.None && !this.RequestScopes.HasFlag(reference.Scopes)) {
				if (ScopeSettings.HasFlag(APIScopeWarning.AddMissingScopes)) {
					this.AddScope(reference.Scopes);
				}
				if (ScopeSettings.HasFlag(APIScopeWarning.ThrowOnMissing)) {
					throw new Exception($"Scopes are missing from the provided credentials when making a request to {reference.GetType().Name}, request cancelled.");
				}
				if (ScopeSettings.HasFlag(APIScopeWarning.WarnOnMissing)) {
					DebugManager.LogMessage($"Scopes are missing from the provided credentials when making a request to {reference.GetType().Name}", DebugManager.ErrorLevel.Warning);
				}
			}
		}
	}
}
