using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ScoredProductions.StreamLinked.API.Auth;
using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace ScoredProductions.StreamLinked.API {

	/// <summary>
	/// Primary singleton for API functionality.
	/// Handles OAuth management and requests.
	/// Contains the methods to make API requests.
	/// </summary>
	[DefaultExecutionOrder(-0x8)]
	[ExecuteAlways]
	public partial class TwitchAPIClient : SingletonDispatcher<TwitchAPIClient> {

		public const string WebResponseBackup = "<b>Unity Application (StreamLinked) has received the response from Twitch.\nApp will now continue.</b><br>(You can close this window now)";

		public static bool APIOAuthAvailable => GetInstance(out TwitchAPIClient instance) && instance.CheckOAuthExistsAndInDate();

		[SerializeField, HideInInspector, Delayed]
		private string _twitchClientID;
		public string TwitchClientID => this._twitchClientID;

		[SerializeField, HideInInspector, Delayed]
		private string _twitchSecret;
		public string TwitchSecret => this._twitchSecret;

		public CancellationToken APICancelToken => this.RequestAPICancellationToken?.Token ?? default;

		public bool GettingNewToken => this.currentTokenBeingRefreshed != null;

		public bool UsePlayerPrefsFirst = false;

		public bool LogAny => this.LogDebugLevel != DebugManager.DebugLevel.None;
		public bool LogAll => this.LogDebugLevel == DebugManager.DebugLevel.Full;

		// Default token settings to use
		[Tooltip("The default token used by the API to make requests if no specific one is provided.")]
		public TokenInstance DefaultAPIToken; // (Property Drawer) Need button to create one in inspector + button to create one if asset doesnt exist in AssetDatabase

		// Queue of tokens to update
		private Queue<TokenInstance> tokenUpdateQueue; // Be very unlikely to excede 1 at a time, but it doubles when needed so start low
		private Queue<TokenInstance> getTokenUpdateQueue => tokenUpdateQueue ??= new Queue<TokenInstance>(1); // only allocate when needed
		public bool CheckTokenIsInQueue(TokenInstance token) => currentTokenBeingRefreshed == token || (tokenUpdateQueue != null && tokenUpdateQueue.Contains(token));

		// Current token being updated
		private TokenInstance currentTokenBeingRefreshed;

		/// <summary>
		/// Returns after the API starts aquiring a new token
		/// </summary>
		[HideInInspector]
		public UnityEvent OnAuthenticationRefreshStarted;

		/// <summary>
		/// Returns after a successful call to Twitch API
		/// </summary>
		[HideInInspector]
		public UnityEvent OnAuthenticationSuccess;

		/// <summary>
		/// Returns after a failed call to Twitch API
		/// </summary>
		[HideInInspector]
		public UnityEvent<Exception> OnAuthenticationFailure;

		[Tooltip("Time auth webserver is active for Twitch to send information back to the app.")]
		public int AuthWebserverActiveTime = 60000; // 1 min

		[NonSerialized]
		private string TwitchState;

		[SerializeField]
		private bool persistBetweenScenes = true;
		public override bool PersistBetweenScenes => this.persistBetweenScenes;

		private CancellationTokenSource HttpListenerCancellationToken; // For webserver shutdown
		private CancellationTokenSource RequestAPICancellationToken; // For general API state

		private AsyncCallback implicitGrantFlowReader;
		private AsyncCallback ImplicitGrantFlowReader => implicitGrantFlowReader ??= new AsyncCallback(this.ImplicitGrantFlowCallbackInitialResponse);

		private AsyncCallback authorizationCodeGrantFlowReader;
		private AsyncCallback AuthorizationCodeGrantFlowReader => authorizationCodeGrantFlowReader ??= new AsyncCallback(this.AuthorizationCodeGrantFlowCallback);

		private static readonly List<Guid> authRequestOrder = new List<Guid>();

		private TwitchAPIClient() { }

		protected override void Awake() {
			if (this.EstablishSingleton(true)) {
				this.BuildCancelTokens();

				if (string.IsNullOrWhiteSpace(this._twitchClientID)) {
					this.LoadClientID(false);
				}

				if (string.IsNullOrWhiteSpace(this._twitchSecret)) {
					this.LoadClientSecret(false);
				}

				if (Application.isPlaying && this.UsePlayerPrefsFirst) {
					this.CheckOAuthExistsAndInDate();
				}
			}
		}

		private void OnEnable() {
			this.BuildCancelTokens();
		}

		private void OnDestroy() {
			this.OnAuthenticationRefreshStarted?.RemoveAllListeners();
			this.OnAuthenticationSuccess?.RemoveAllListeners();
			this.OnAuthenticationFailure?.RemoveAllListeners();
			this.EndCancelTokens();
		}

		private void OnDisable() {
			this.EndCancelTokens();
		}

		protected override void OnApplicationQuit() {
			this.EndCancelTokens();
			base.OnApplicationQuit();
		}

		protected override void LateUpdate() {
			if (!GetInstance(out _)) { // Protection vs Editor data wipes
				this.EstablishSingleton(true);
			}

			if (Application.isPlaying && this.DefaultAPIToken != null && this.DefaultAPIToken.CheckRefreshNeeded()) {
				this.GetNewAuthToken(this.DefaultAPIToken);
			}

			if (this.currentTokenBeingRefreshed == null && this.getTokenUpdateQueue.TryDequeue(out TokenInstance token)) {
				this.BeginTokenAquisition(token);
			}

			base.LateUpdate();
		}

		/// <summary>
		/// Stops all currently running API requests and clears the API Token request queue
		/// </summary>
		public void CancelAPIRequestsAndReset() {
			this.EndCancelTokens();
			this.BuildCancelTokens();
			this.StopAllCoroutines();
			this.tokenUpdateQueue.Clear();
			this.currentTokenBeingRefreshed = null;
		}

		/// <summary>
		/// Get the type of the currently used auth token, null if no token
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public AuthRequestType? GetCurrentAuthenticationType() {
			if (this.GettingNewToken || this.DefaultAPIToken == null) {
				return null;
			}
			return this.DefaultAPIToken.AuthenticationType;
		}

		/// <summary>
		/// Change authentication type to new value
		/// </summary>
		public void SetAuthenticationType(AuthRequestType type, TokenInstance token = null, bool aquireNewToken = true) {
			if (token == null && this.DefaultAPIToken == null) {
				return;
			}
			else if (token == null) {
				token = this.DefaultAPIToken;
			}

			token.AuthenticationType = type;

			if (aquireNewToken) {
				this.GetNewAuthToken(token);
			}
		}

		public void BuildCancelTokens() {
			this.RequestAPICancellationToken ??= new CancellationTokenSource();
		}

		/// <summary>
		/// Shuts down any running requests by canceling the tokens running them
		/// </summary>
		private void EndCancelTokens() {
			if (this.RequestAPICancellationToken != null) {
				try {
					this.RequestAPICancellationToken.Cancel();
					this.RequestAPICancellationToken.Dispose();
					this.RequestAPICancellationToken = null;
				} catch {
					if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
						DebugManager.LogMessage("RequestAPICancellationToken was Canceled".RichTextColour("orange"));
					}
				}
			}

			if (this.HttpListenerCancellationToken != null) {
				try {
					this.HttpListenerCancellationToken.Cancel();
					this.HttpListenerCancellationToken.Dispose();
					this.HttpListenerCancellationToken = null;
				} catch {
					if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
						DebugManager.LogMessage("HttpListenerCancellationToken was Canceled".RichTextColour("orange"));
					}
				}
			}
		}

		public bool HasSettingsToGetOAuth(AuthRequestType? AuthType = null) {
			if (!AuthType.HasValue && this.DefaultAPIToken != null) {
				AuthType = this.DefaultAPIToken.AuthenticationType;
			}
			return AuthType switch {
				AuthRequestType.ImplicitGrantFlow
				or AuthRequestType.DeviceCodeGrantFlow => !string.IsNullOrWhiteSpace(this.TwitchClientID),
				AuthRequestType.AuthorizationCodeGrantFlow
				or AuthRequestType.ClientCredentialsGrantFlow => !string.IsNullOrWhiteSpace(this.TwitchClientID) && !string.IsNullOrWhiteSpace(this.TwitchSecret),
				_ => false,
			};
		}

		public string[] ConvertScopesToStringArray(TokenInstance token, bool escape) {
			List<TwitchScopesEnum> scopes = token.RequestScopes.GetAllFlagged();
			string[] array = new string[scopes.Count];

			for (int x = 0; x < scopes.Count; x++) {
				array[x] = escape ? Uri.EscapeDataString(scopes[x].GetLinkedEnumToString()) : scopes[x].GetLinkedEnumToString();
			}

			return array;
		}

		/// <summary>
		/// Loads the Client ID from PlayerPrefs, setting of the value is influenced by <c>UsePlayerPrefsFirst</c>
		/// </summary>
		/// <returns>Found value in PlayerPrefs is different to the current one</returns>
		public bool LoadClientID(bool log) {
			bool returnValue = false;
			if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchClientID, out string twitchClientID, log)) {
				if (this.UsePlayerPrefsFirst || string.IsNullOrWhiteSpace(this.TwitchClientID)) {
					returnValue = this._twitchClientID != twitchClientID;
					this._twitchClientID = twitchClientID;
				}
				else if (this._twitchClientID != twitchClientID) {
					return true;
				}
			}
			return returnValue;
		}

		public bool LoadClientID() {
			return this.LoadClientID(this.LogDebugLevel > DebugManager.DebugLevel.Necessary);
		}

		/// <summary>
		/// Loads the Client Secret from PlayerPrefs, setting of the value is influenced by <c>UsePlayerPrefsFirst</c>
		/// </summary>
		/// <returns>Found value in PlayerPrefs is different to the current one</returns>
		public bool LoadClientSecret(bool log) {
			bool returnValue = false;
			if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchClientSecret, out string twitchSecret, log)) {
				if (this.UsePlayerPrefsFirst || string.IsNullOrWhiteSpace(this.TwitchSecret)) {
					returnValue = this._twitchSecret != twitchSecret;
					this._twitchSecret = twitchSecret;
				}
				else if (this._twitchSecret != twitchSecret) {
					return true;
				}
			}
			return returnValue;
		}

		public bool LoadClientSecret() {
			return this.LoadClientSecret(this.LogDebugLevel > DebugManager.DebugLevel.Necessary);
		}

		/// <summary>
		/// API will check and perform a get on the provided <c>TokenInstance</c>, if non is provided it will check the one provided for the API to use.
		/// </summary>
		/// <param name="tokenInstance"></param>
		public bool CheckOAuthExistsAndInDate(TokenInstance tokenInstance = null) {
			if (tokenInstance == null) {
				if (this.DefaultAPIToken == null) {
					return false;
				}
				tokenInstance = this.DefaultAPIToken;
			}
			if (this.currentTokenBeingRefreshed == tokenInstance) {
				return false;
			}

			bool check = tokenInstance.CheckRefreshNeeded(this.LogDebugLevel > DebugManager.DebugLevel.Necessary);

			if (check
				&& tokenInstance.AutoRetrieveNewAuth
				&& !this.GettingNewToken
				&& this.HasSettingsToGetOAuth()
				&& InstanceIsAlive) {
				this.GetNewAuthToken(tokenInstance);
			}
			return !check;
		}

		public void WriteTokenToSettings(TokenInstance token, bool log = false) {
			JsonValue retrieveToken = token.RetrieveTokenAsJson();

			JsonArray arrayContainer;
			JsonObject parsed;
			if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchAuthenticationTokens, out string tokens, log)) {
				parsed = JsonReader.Parse(tokens);

				JsonValue data = parsed[TwitchWords.DATA];
				if (data.IsJsonArray
					&& parsed[TwitchWords.CLIENT_ID].AsString == this._twitchClientID
					&& parsed[TwitchWords.CLIENT_SECRET].AsString == this._twitchSecret) {
					arrayContainer = data.AsJsonArray;
				}
				else {
					arrayContainer = new JsonArray();
				}
			}
			else {
				arrayContainer = new JsonArray();
				parsed = new JsonObject();
			}

			parsed[TwitchWords.CLIENT_ID] = this._twitchClientID;
			parsed[TwitchWords.CLIENT_SECRET] = this._twitchSecret;

			int x = 0;
			for (; x < arrayContainer.Count; x++) {
				if (arrayContainer[x][TwitchWords.ID] == token.TokenID) {
					break;
				}
			}
			if (x == arrayContainer.Count) {
				arrayContainer.Add(retrieveToken);
			}
			else {
				arrayContainer[x] = retrieveToken;
			}
			parsed[TwitchWords.DATA] = arrayContainer;

			InternalSettingsStore.EditSetting(SavedSettings.TwitchAuthenticationTokens, parsed.ToString(), log);
		}

		public bool CheckStoreHasTokens(bool log = false) {
			if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchAuthenticationTokens, out string tokens, log)) {
				JsonObject parsed = JsonReader.Parse(tokens);

				if (parsed[TwitchWords.DATA] != JsonValue.Null) {
					JsonArray arrayContainer = parsed[TwitchWords.DATA];
					if (arrayContainer.Count > 0) {
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Saves the ClientID and ClientSecret to PlayerPrefs
		/// </summary>
		public void SaveCurrentToPlayerPrefs() {
			InternalSettingsStore.EditSetting(SavedSettings.TwitchClientID, this.TwitchClientID, this.LogDebugLevel > DebugManager.DebugLevel.None);
			InternalSettingsStore.EditSetting(SavedSettings.TwitchClientSecret, this.TwitchSecret, this.LogDebugLevel > DebugManager.DebugLevel.None);
		}

		/// <summary>
		/// Clears the ClientID and ClientSecret from PlayerPrefs
		/// </summary>
		public void ClearCurrentPlayerPrefs() {
			InternalSettingsStore.EditSetting(SavedSettings.TwitchClientID, null, this.LogDebugLevel > DebugManager.DebugLevel.None);
			InternalSettingsStore.EditSetting(SavedSettings.TwitchClientSecret, null, this.LogDebugLevel > DebugManager.DebugLevel.None);
		}

		public void CleanPlayerPrefTokens(bool log, params string[] IDs) {
			if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchAuthenticationTokens, out string tokens, log)) {
				JsonObject parsed = JsonReader.Parse(tokens);
				if (IDs.IsNullOrEmpty()) {
					parsed[TwitchWords.DATA] = new JsonArray();
					InternalSettingsStore.EditSetting(SavedSettings.TwitchAuthenticationTokens, parsed.ToString(), log);
					return;
				}
				JsonValue data = parsed[TwitchWords.DATA];
				JsonValue foundValue = JsonValue.Null;
				if (data.IsJsonArray) {
					JsonArray arrayContainer = data.AsJsonArray;
					bool arrayChanged = false;

					for (int x = 0; x < IDs.Length; x++) {
						string id = IDs[x];
						int y = 0;
						for (; y < arrayContainer.Count; y++) {
							JsonValue value = arrayContainer[y];
							if (value[TwitchWords.ID] == id) {
								foundValue = value;
								arrayChanged = true;
								break;
							}
						}

						if (foundValue != JsonValue.Null) {
							arrayContainer.Remove(y);
						}
					}

					parsed[TwitchWords.DATA] = arrayContainer;

					if (arrayChanged) {
						InternalSettingsStore.EditSetting(SavedSettings.TwitchAuthenticationTokens, parsed.ToString(), log);
					}
				}
			}
		}

		public void CleanPlayerPrefTokens(params string[] IDs) {
			this.CleanPlayerPrefTokens(this.LogDebugLevel != DebugManager.DebugLevel.None, IDs);
		}

		public void UpdateClientIDAndSecret(string clientID, string secret) {
			this.UpdateClientIDAndSecret(clientID, secret, this.LogDebugLevel > DebugManager.DebugLevel.None);
		}

		/// <summary>
		/// Updates both values, if one value hasnt changed please still include it here.
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="secret"></param>
		/// <param name="log"></param>
		public void UpdateClientIDAndSecret(string clientID, string secret, bool log) {
			if (this._twitchClientID != clientID || this._twitchSecret != secret) {
				this._twitchClientID = clientID;
				this._twitchSecret = secret;
				this.CleanPlayerPrefTokens(log);
				if (this.DefaultAPIToken != null && this.DefaultAPIToken.AutoRetrieveNewAuth && this.HasSettingsToGetOAuth() && InstanceIsAlive) {
					this.GetNewAuthToken(this.DefaultAPIToken);
				}

				this.SaveCurrentToPlayerPrefs();
			}
		}

		#region Twitch OAuth

		/// <summary>
		/// Start up webserver from Unity to receive some Auth token responses
		/// </summary>
		private void StartLocalWebServer(AuthRequestType requestType) {
			if (this.currentTokenBeingRefreshed == null) { // dont start the server if there is no token
				return;
			}

			HttpListener httpListener = new HttpListener();
			this.HttpListenerCancellationToken?.Cancel();
			this.HttpListenerCancellationToken = new CancellationTokenSource();

			httpListener.Prefixes.Add(this.currentTokenBeingRefreshed.RedirectURI);

			httpListener.Start();
			switch (requestType) {
				case AuthRequestType.ImplicitGrantFlow:
					httpListener.BeginGetContext(this.ImplicitGrantFlowReader, httpListener);
					break;
				case AuthRequestType.AuthorizationCodeGrantFlow:
					httpListener.BeginGetContext(this.AuthorizationCodeGrantFlowReader, httpListener);
					break;
				default:
					throw new NotSupportedException($"Auth request type {{{requestType}}} does not support Webserver retrieval, please consult the Twitch API for instructions.");
			}

			// Timeout task
			Task.Delay(this.AuthWebserverActiveTime, this.HttpListenerCancellationToken.Token)
				.ContinueWith(e => {
					if (!e.IsCanceled) {
						this.currentTokenBeingRefreshed = null;
						if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
							DebugManager.LogMessage("Webserver listening for token response timed out. Token request must be restarted.".RichTextColour("yellow"), DebugManager.ErrorLevel.Warning);
						}
					}
					if (httpListener != null && httpListener.IsListening) {
						httpListener.Stop();
					}
				}).ConfigureAwait(false);
		}

		/// <summary>
		/// HTML/Javascript body and script to send up the webserver for clients after a response has been received
		/// </summary>
		/// <param name="type"></param>
		/// <param name="body"></param>
		/// <returns></returns>
		private string ResponseBuilder(AuthRequestType type) {
			StringBuilder builder = new StringBuilder("<html><body>");

			if (string.IsNullOrWhiteSpace(this.currentTokenBeingRefreshed.UserProvidedWebResponse)) {
				builder.Append(WebResponseBackup);
			}
			else {
				builder.Append(this.currentTokenBeingRefreshed.UserProvidedWebResponse);
			}

			// URL is the response, retreive the URL and send it back down with Javascript
			if (type == AuthRequestType.ImplicitGrantFlow && this.currentTokenBeingRefreshed != null) {
				builder.Append($@"
					<script type=""text/javascript"">
					var xhr = new XMLHttpRequest();
					xhr.open(""POST"", ""{UnityWebRequest.EscapeURL(this.currentTokenBeingRefreshed.RedirectURI)}"");
					xhr.send(window.location);
					{this.currentTokenBeingRefreshed.UserProvidedJSCode}
					</script>
				");
			}

			builder.Append("</body></html>");
			return builder.ToString();
		}

		/// <summary>
		/// Gets a new Auth token of the current type
		/// </summary>
		public void GetNewAuthToken(TokenInstance tokenSettings) {
			if (!this.CheckTokenIsInQueue(tokenSettings)) {
				this.getTokenUpdateQueue.Enqueue(tokenSettings);
			}
		}

		private void BeginTokenAquisition(TokenInstance tokenSettings) {
			if (tokenSettings != null) {
				this.currentTokenBeingRefreshed = tokenSettings;

				switch (tokenSettings.AuthenticationType) {
					case AuthRequestType.ImplicitGrantFlow:
						this.GetImplicitGrantFlowToken();
						break;
					case AuthRequestType.AuthorizationCodeGrantFlow:
						this.GetAuthorizationGrantFlowToken();
						break;
					case AuthRequestType.DeviceCodeGrantFlow:
						this.GetDeviceCodeGrantFlowToken();
						break;
					case AuthRequestType.ClientCredentialsGrantFlow:
						this.GetClientCredentialsFlowToken();
						break;
				}
			}
		}

		/// <summary>
		/// Extracts query values from a URI
		/// </summary>
		private NameValueCollection ExtractURIQueryValues(string uri) {
			NameValueCollection query = new NameValueCollection();
			int index = uri.IndexOf('#') + 1;
			string urlQuery = index == 0 ? uri : uri[index..];
			string[] queryParams = urlQuery.Split('&');
			foreach (string item in queryParams) {
				int indexOf = item.IndexOf('=');
				if (indexOf <= 0) {
					continue;
				}
				string name = item[..indexOf];
				string value = item[(indexOf + 1)..];
				if (name.Equals(TwitchWords.SCOPE)) {
					value = value.Replace("%3A", ":");
				}
				query.Add(name, value);
			}
			return query;
		}

		/// <summary>
		/// Build query parameters for URL
		/// </summary>
		private string BuildQueryPiece(string name, params string[] value) {
			int count = value.Length;
			string build = "";
			if (count > 0) {
				int x = 0;
				build = value[x++];
				for (; x < value.Length; x++) {
					build += "+" + value[x];
				}
			}
			return $"&{name}={build}";
		}

		/// <summary>
		/// Converts the scopes array into a query value
		/// </summary>
		/// <returns></returns>
		private string ScopesToString(TokenInstance token) {
			StringBuilder builder = new StringBuilder();
			string[] scopes = this.ConvertScopesToStringArray(token, true);
			int max = scopes.Length;
			int appendCheck = max - 1;
			for (int x = 0; x < max; x++) {
				builder.Append(scopes[x]);
				if (x < appendCheck) {
					builder.Append("+");
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Runs when token is awaiting to be received from URI
		/// </summary>
		private void ImplicitGrantFlowCallbackInitialResponse(IAsyncResult result) {
			HttpListener httpListener = (HttpListener)result.AsyncState;
			httpListener.BeginGetContext(new AsyncCallback(this.ImplicitGrantFlowCallbackBuildToken), httpListener);

			HttpListenerContext httpContext = httpListener.EndGetContext(result);
			HttpListenerResponse httpResponse = httpContext.Response;

			byte[] buffer = Encoding.UTF8.GetBytes(this.ResponseBuilder(AuthRequestType.ImplicitGrantFlow));

			// send the output to the client browser
			httpResponse.ContentLength64 = buffer.Length;
			Stream output = httpResponse.OutputStream;
			output.Write(buffer, 0, buffer.Length);
			output.Close();
		}

		/// <summary>
		/// Runs after Javascript aquires the URI and sends it back down to Unity from the browser
		/// </summary>
		private void ImplicitGrantFlowCallbackBuildToken(IAsyncResult result) {
			HttpListener httpListener = (HttpListener)result.AsyncState;
			HttpListenerContext httpContext = httpListener.EndGetContext(result);
			HttpListenerRequest httpRequest = httpContext.Request;

			string returnedURL;
			using (StreamReader reader = new StreamReader(httpRequest.InputStream, httpRequest.ContentEncoding)) {
				returnedURL = reader.ReadToEnd();
			}

			httpListener.Stop();
			this.HttpListenerCancellationToken?.Cancel();

			try {
				if (!string.IsNullOrWhiteSpace(returnedURL)) {
					NameValueCollection query = this.ExtractURIQueryValues(returnedURL);

					string state = query.Get(TwitchWords.STATE);

					if (this.TwitchState.Equals(state)) {
						ImplicitGrantFlow igf = new ImplicitGrantFlow(query);

						TwitchAPIDataContainer<GetValidatedTokenInfo> tokenValidatedInfo = MakeTwitchAPIRequest<GetValidatedTokenInfo>(int.MaxValue,
												this.currentTokenBeingRefreshed,
												new (string, string)[] {
													(GetValidatedTokenInfo.AUTHORIZATION, TwitchStatic.AppendOAuthToBearer(igf.Access_Token))
												});

						if (tokenValidatedInfo.HasErrored) {
							throw new Exception(tokenValidatedInfo.ErrorToJson());
						}
						else {
							GetValidatedTokenInfo tokenData = tokenValidatedInfo.data[0];

							if (tokenData.expires_in > int.MinValue) {
								igf.Expires_In = tokenData.expires_in;
							}
						}

						if (this.currentTokenBeingRefreshed != null) {
							this.currentTokenBeingRefreshed.OAuthToken = igf;
							TokenInstance token = this.currentTokenBeingRefreshed;
							MainThreadDispatchQueue.Enqueue(() => this.WriteAuthenticationSuccess(token));
						}
					}
					else {
						string message = "State returned from Twitch did not match locally sent value, State has errored";
						if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
							DebugManager.LogMessage(message.RichTextColour("red"), DebugManager.ErrorLevel.Exception);
						}
					}
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
			} finally {
				this.currentTokenBeingRefreshed = null;
				this.TwitchState = null;
			}
		}

		/// <summary>
		/// Data received from Twitch to continue and build the next part of the token
		/// </summary>
		private void AuthorizationCodeGrantFlowCallback(IAsyncResult result) {
			HttpListener httpListener = (HttpListener)result.AsyncState;
			HttpListenerContext httpContext = httpListener.EndGetContext(result);
			HttpListenerResponse httpResponse = httpContext.Response;

			NameValueCollection query = httpContext.Request.QueryString;

			// Respond after message received and close
			byte[] outbuffer = Encoding.UTF8.GetBytes(this.ResponseBuilder(AuthRequestType.AuthorizationCodeGrantFlow));

			httpResponse.ContentLength64 = outbuffer.Length;
			Stream output = httpResponse.OutputStream;
			output.Write(outbuffer, 0, outbuffer.Length);
			output.Close();

			string state = query.Get(TwitchWords.STATE);

			httpListener.Stop();
			this.HttpListenerCancellationToken?.Cancel();

			if (this.currentTokenBeingRefreshed == null) {
				return;
			}

			try {
				if (!this.TwitchState.Equals(state)) {
					if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
						DebugManager.LogMessage("State returned from Twitch did not match locally sent value, State has errored".RichTextColour("red"), DebugManager.ErrorLevel.Exception);
					}
				}
				else {
					TwitchAPIDataContainer<GetAuthorizationCodeToken> returnData = MakeTwitchAPIRequestAsync<GetAuthorizationCodeToken>(
							this.currentTokenBeingRefreshed,
							new (string, string)[] {
									(GetAuthorizationCodeToken.CLIENT_ID, this.TwitchClientID),
									(GetAuthorizationCodeToken.CLIENT_SECRET, this.TwitchSecret),
									(GetAuthorizationCodeToken.CODE, query[TwitchWords.CODE]),
									GetAuthorizationCodeToken.GRANT_TYPE,
									(GetAuthorizationCodeToken.REDIRECT_URI, this.currentTokenBeingRefreshed.RedirectURI)
							},
							cancelToken: this.RequestAPICancellationToken.Token).Result; // Its already on a seperate thread and as its a callback it cant be a task

					if (returnData.HasErrored) {
						throw new Exception(returnData.ErrorToJson());
					}
					else {
						if (this.currentTokenBeingRefreshed != null) {
							this.currentTokenBeingRefreshed.OAuthToken = new AuthorizationCodeGrantFlow(returnData.data[0]);
							TokenInstance token = this.currentTokenBeingRefreshed;
							MainThreadDispatchQueue.Enqueue(() => this.WriteAuthenticationSuccess(token));
						}
					}
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
			} finally {
				this.currentTokenBeingRefreshed = null;
				this.TwitchState = null;
			}
		}

		/// <summary>
		/// Build method to produce a URI on the Authorise URI
		/// </summary>
		private string BuildGrantFlowLink(string client_id, bool? force_verify, string redirect_uri, string response_type, string[] scope, string state) {
			if (string.IsNullOrWhiteSpace(client_id)) {
				throw new ArgumentException("Required input, client_id was empty, please make sure its provided before making this call.");
			}
			if (string.IsNullOrWhiteSpace(redirect_uri)) {
				throw new ArgumentException("Required input, redirect_uri was empty, please make sure its provided before making this call.");
			}
			if (scope == null || scope.Length == 0) {
				throw new ArgumentException("Required input, scope was empty, please make sure the required scopes are provided before making this call.");
			}

			StringBuilder sb = new StringBuilder(TwitchAPILinks.GetAuthData);
			sb.Append('?');

			sb.Append(this.BuildQueryPiece(TwitchWords.CLIENT_ID, client_id));

			if (force_verify.HasValue) {
				sb.Append(this.BuildQueryPiece(TwitchWords.FORCE_VERIFY, force_verify.ToString()));
			}

			sb.Append(this.BuildQueryPiece(TwitchWords.REDIRECT_URI, redirect_uri));
			sb.Append(this.BuildQueryPiece(TwitchWords.RESPONSE_TYPE, response_type));
			sb.Append(this.BuildQueryPiece(TwitchWords.SCOPE, scope));

			if (!string.IsNullOrWhiteSpace(state)) {
				sb.Append(this.BuildQueryPiece(TwitchWords.STATE, state));

				this.TwitchState = state;
			}

			return sb.ToString();
		}

		/// <summary>
		/// Start of Implicit Grant Flow token aquisition process
		/// </summary>
		private void GetImplicitGrantFlowToken() {
			if (string.IsNullOrWhiteSpace(this.TwitchClientID)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch ClientID, please provide one and try again.".RichTextColour("red"));
				}
				return;
			}
			if (!this.GettingNewToken) {
				return;
			}

			try {
				this.InvokeRefreshStarted();

				string request = this.BuildGrantFlowLink(this.TwitchClientID,
						null,
						this.currentTokenBeingRefreshed.RedirectURI,
						TwitchWords.TOKEN,
						this.ConvertScopesToStringArray(this.currentTokenBeingRefreshed, true),
						Guid.NewGuid().ToString("N"));
				if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
					DebugManager.LogMessage($"Starting Auth Request: {request}".RichTextColour("blue"));
				}

				if (this.currentTokenBeingRefreshed.CreateLocalHostServer) {
					this.StartLocalWebServer(AuthRequestType.ImplicitGrantFlow);
				}

				MainThreadDispatchQueue.Enqueue(() => Application.OpenURL(request));
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				this.currentTokenBeingRefreshed = null;
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
			}
		}

		/// <summary>
		/// Start of Authorization Grant Flow token aquisition process
		/// </summary>
		private void GetAuthorizationGrantFlowToken() {
			if (string.IsNullOrWhiteSpace(this.TwitchSecret)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch Secret, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (string.IsNullOrWhiteSpace(this.TwitchClientID)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch ClientID, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (!this.GettingNewToken) {
				return;
			}

			try {
				this.InvokeRefreshStarted();

				if (this.currentTokenBeingRefreshed.OAuthToken is AuthorizationCodeGrantFlow ACGF && !string.IsNullOrEmpty(ACGF.Refresh_Token)) {
					Task.Run(this.GetAuthorizationGrantFlowRefreshToken);
				}
				else {
					string request = this.BuildGrantFlowLink(this.TwitchClientID,
							null,
							this.currentTokenBeingRefreshed.RedirectURI,
							TwitchWords.CODE,
							this.ConvertScopesToStringArray(this.currentTokenBeingRefreshed, true),
							Guid.NewGuid().ToString("N"));

					if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
						DebugManager.LogMessage($"Starting Auth Request: {request}".RichTextColour("blue"));
					}

					if (this.currentTokenBeingRefreshed.CreateLocalHostServer) {
						this.StartLocalWebServer(AuthRequestType.AuthorizationCodeGrantFlow);
					}

					MainThreadDispatchQueue.Enqueue(() => Application.OpenURL(request));
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				this.currentTokenBeingRefreshed = null;
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
			}
		}

		/// <summary>
		/// Start of Device Code Grant Flow token aquisition process
		/// </summary>
		private void GetDeviceCodeGrantFlowToken() {
			if (string.IsNullOrWhiteSpace(this.TwitchClientID)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch ClientID, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (!this.GettingNewToken) {
				return;
			}

			try {
				this.InvokeRefreshStarted();

				if (this.currentTokenBeingRefreshed.OAuthToken is DeviceCodeGrantFlow DCGF && !string.IsNullOrEmpty(DCGF.Refresh_Token)) {
					Task.Run(this.GetDeviceCodeGrantFlowRefresh);
				}
				else {
					Task.Run(this.GetDeviceCodeGrantFlowInitial);
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				this.currentTokenBeingRefreshed = null;
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
			}
		}

		private void InvokeRefreshStarted() {
			if (this.OnAuthenticationRefreshStarted != null) {
				MainThreadDispatchQueue.Enqueue(this.OnAuthenticationRefreshStarted.Invoke);
			}
		}

		/// <summary>
		/// Uses the tokens Refresh details to refresh the Auth token
		/// </summary>
		/// <returns></returns>
		private async Task GetAuthorizationGrantFlowRefreshToken() {
			if (string.IsNullOrWhiteSpace(this.TwitchSecret)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch Secret, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (string.IsNullOrWhiteSpace(this.TwitchClientID)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch ClientID, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (!this.GettingNewToken) {
				return;
			}

			try {
				if (this.currentTokenBeingRefreshed.OAuthToken is AuthorizationCodeGrantFlow ACGF) {
					TwitchAPIDataContainer<GetTokenRefresh> tokenRefresh = await MakeTwitchAPIRequestAsync<GetTokenRefresh>(
							this.currentTokenBeingRefreshed,
							new (string, string)[] {
								GetTokenRefresh.GRANT_TYPE,
								(GetTokenRefresh.CLIENT_ID, this.TwitchClientID),
								(GetTokenRefresh.CLIENT_SECRET, this.TwitchSecret),
								(GetTokenRefresh.REFRESH_TOKEN, ACGF.Refresh_Token)
							},
							cancelToken: this.RequestAPICancellationToken.Token);

					if (tokenRefresh.HasErrored) {
						throw new Exception(tokenRefresh.ErrorToJson());
					}
					else {
						GetTokenRefresh token = tokenRefresh.data[0];

						TwitchAPIDataContainer<GetValidatedTokenInfo> tokenValidatedInfo = await MakeTwitchAPIRequestAsync<GetValidatedTokenInfo>(
							this.currentTokenBeingRefreshed,
							new (string, string)[] {
								(GetValidatedTokenInfo.AUTHORIZATION, TwitchStatic.AppendOAuthToBearer(token.access_token))
							},
							cancelToken: this.RequestAPICancellationToken.Token);

						if (tokenValidatedInfo.HasErrored) {
							throw new Exception(tokenValidatedInfo.ErrorToJson());
						}
						else if (this.currentTokenBeingRefreshed != null) {
							this.currentTokenBeingRefreshed.OAuthToken = new AuthorizationCodeGrantFlow(token, tokenValidatedInfo.data[0]);
							TokenInstance tokenInst = this.currentTokenBeingRefreshed;
							MainThreadDispatchQueue.Enqueue(() => this.WriteAuthenticationSuccess(tokenInst));
						}
					}
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
			} finally {
				this.currentTokenBeingRefreshed = null;
			}
		}

		/// <summary>
		/// Method to aquire App Access Token
		/// </summary>
		public void GetClientCredentialsFlowToken() {
			if (string.IsNullOrWhiteSpace(this.TwitchSecret)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch Secret, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (string.IsNullOrWhiteSpace(this.TwitchClientID)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch ClientID, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (!this.GettingNewToken) {
				return;
			}

			try {
				this.MakeTwitchAPIRequest<GetClientCredentialsGrantFlow>(
					this.GetClientCredentialsResponse,
					null,
					QueryParameters: new (string, string)[] {
						(GetClientCredentialsGrantFlow.CLIENT_ID, this.TwitchClientID),
						(GetClientCredentialsGrantFlow.CLIENT_SECRET, this.TwitchSecret),
						(GetClientCredentialsGrantFlow.GRANT_TYPE, TwitchWords.CLIENT_CREDENTIALS)
					});
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
				this.currentTokenBeingRefreshed = null;
			}
		}

		private void GetClientCredentialsResponse(TwitchAPIDataContainer<GetClientCredentialsGrantFlow> tokenRequest) {
			if (tokenRequest.HasErrored) {
				throw new Exception(tokenRequest.ErrorToJson());
			}
			else {
				this.currentTokenBeingRefreshed.OAuthToken = new ClientCredentialsFlow(tokenRequest.data[0]);
				TokenInstance token = this.currentTokenBeingRefreshed;
				MainThreadDispatchQueue.Enqueue(() => this.WriteAuthenticationSuccess(token));
			}

			this.currentTokenBeingRefreshed = null;
		}

		/// <summary>
		/// Method to aquire a new Device Code Grant Flow Token
		/// </summary>
		private async Task GetDeviceCodeGrantFlowInitial() {
			if (string.IsNullOrWhiteSpace(this.TwitchClientID)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch ClientID, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (!this.GettingNewToken) {
				return;
			}

			try {
				TwitchAPIDataContainer<GetDeviceCodeGrantFlow> initialTokenRequest = await MakeTwitchAPIRequestAsync<GetDeviceCodeGrantFlow>(
					this.currentTokenBeingRefreshed,
					QueryParameters: new (string, string)[] {
						(GetDeviceCodeGrantFlow.CLIENT_ID, this.TwitchClientID),
						(GetDeviceCodeGrantFlow.SCOPES, this.ScopesToString(this.currentTokenBeingRefreshed))
					},
					cancelToken: this.RequestAPICancellationToken.Token);
				if (!initialTokenRequest.HasErrored && this.currentTokenBeingRefreshed != null) {
					GetDeviceCodeGrantFlow tempDeviceContainer = initialTokenRequest.data[0];
					this.DefaultAPIToken.ExpectedDeviceCode = tempDeviceContainer.device_code;

					if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
						DebugManager.LogMessage($"Starting Device Code Confirm Request Code: {{{tempDeviceContainer.user_code}}} URL: {{{tempDeviceContainer.verification_uri}}}".RichTextColour("blue"));
					}

					MainThreadDispatchQueue.Enqueue(() => Application.OpenURL(tempDeviceContainer.verification_uri));

					if (this.DefaultAPIToken.ManualRetrieval) {
						DebugManager.LogMessage($"Device Code Retrieval settings has been set to Manual, flow has ended as is awaiting the token to be supplied to the API Client via ReceiveDeviceCodeGrantFlowManually(GetDeviceCodeGrantAuthorisation data).".RichTextColour("blue"));
					}
					else {
						int retryCount = 0;
						do {
							await Task.Delay(this.DefaultAPIToken.PingInterval);

							TwitchAPIDataContainer<GetDeviceCodeGrantAuthorisation> tokenRequestAuthorisation = await MakeTwitchAPIRequestAsync<GetDeviceCodeGrantAuthorisation>(
								this.currentTokenBeingRefreshed,
								QueryParameters: new (string, string)[] {
									(GetDeviceCodeGrantAuthorisation.CLIENT_ID, this.TwitchClientID),
									(GetDeviceCodeGrantAuthorisation.SCOPES, this.ScopesToString(this.currentTokenBeingRefreshed)),
									(GetDeviceCodeGrantAuthorisation.DEVICE_CODE, tempDeviceContainer.device_code),
									GetDeviceCodeGrantAuthorisation.GRANT_TYPE,
								},
							cancelToken: this.RequestAPICancellationToken.Token);

							if (!this.GettingNewToken) {
								return;
							}

							if (tokenRequestAuthorisation.HasErrored) {
								JsonValue errorBody = JsonReader.Parse(tokenRequestAuthorisation.RawResponse);
								if (tokenRequestAuthorisation.status == 400) {
									switch (errorBody[TwitchWords.MESSAGE].AsString) {
										case TokenInstance.InvalidRefreshToken:
											throw new Exception($"API call GetDeviceCodeGrantAuthorisation failed due to an invalid refresh token: {tokenRequestAuthorisation.ErrorText}");
										case TokenInstance.InvalidDeviceCode:
											throw new Exception($"API call GetDeviceCodeGrantAuthorisation failed due to an invalid device code submission: {tokenRequestAuthorisation.ErrorText}");
										case TokenInstance.AuthPending:
											if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
												DebugManager.LogMessage("DeviceCode Authorisation is still pending, queuing retry".RichTextColour("orange"));
											}
											continue;
									}
								}
								throw new Exception($"API call GetDeviceCodeGrantAuthorisation failed: {tokenRequestAuthorisation.ErrorToJson()}");
							}
							else {
								GetDeviceCodeGrantAuthorisation container = tokenRequestAuthorisation.data[0];

								if (this.currentTokenBeingRefreshed != null) {
									this.currentTokenBeingRefreshed.OAuthToken = new DeviceCodeGrantFlow(container);
									TokenInstance token = this.currentTokenBeingRefreshed;
									MainThreadDispatchQueue.Enqueue(() => this.WriteAuthenticationSuccess(token));
								}
								this.currentTokenBeingRefreshed = null;
								return;
							}
						} while (++retryCount < this.DefaultAPIToken.PingRetries);
						throw new Exception("Ping count for automatic retry of GetDeviceCodeGrantAuthorisation as been exceeded without success, please request a new token or attempt to manually retreive on client confirmation.");
					}
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
					DebugManager.LogMessage(ex);
				}
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));

				this.currentTokenBeingRefreshed = null;
			}
		}

		/// <summary>
		/// Function to supply the Device Code Grant Flow token Directly to the API after being manually set to. Will error if not expecting code.
		/// </summary>
		/// <param name="data"></param>
		public void ReceiveDeviceCodeGrantFlowManually(GetDeviceCodeGrantAuthorisation data) {
			try {
				if (!this.GettingNewToken || string.IsNullOrWhiteSpace(this.DefaultAPIToken.ExpectedDeviceCode)) {
					throw new Exception("Twitch Client API was not expecting a Device Code Grant Flow token. The API will only accept these when needed during a manual token wait. Please review your flow.");
				}
				if (!this.GettingNewToken) {
					throw new Exception("Twitch Client API is no longer expecting a Device Code Grant Flow token and has no instance to populate.");
				}

				this.currentTokenBeingRefreshed.OAuthToken = new DeviceCodeGrantFlow(data);
				TokenInstance token = this.currentTokenBeingRefreshed;
				MainThreadDispatchQueue.Enqueue(() => this.WriteAuthenticationSuccess(token));
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));
			}
			this.currentTokenBeingRefreshed = null;
		}

		/// <summary>
		/// Starts process to refresh current Device token
		/// </summary>
		private Task GetDeviceCodeGrantFlowRefresh() {
			if (this.currentTokenBeingRefreshed.OAuthToken is DeviceCodeGrantFlow DCGF) {
				return this.GetDeviceCodeGrantFlowAuthoirisation(DCGF.Refresh_Token);
			}
			throw new ArgumentException("UserAccessToken is not of type DeviceCodeGrantFlow, Refresh failed");
		}

		/// <summary>
		/// Authorises Device Code Grant Flow Token to be used and aquires token
		/// </summary>
		private async Task GetDeviceCodeGrantFlowAuthoirisation(string code) {
			if (string.IsNullOrWhiteSpace(this.TwitchClientID)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("API Attempted to get a new OAuth without a Twitch ClientID, please provide one and try again.".RichTextColour("red"));
				}
				this.currentTokenBeingRefreshed = null;
				return;
			}
			if (!this.GettingNewToken) {
				return;
			}

			try {
				(string, string)[] QueryParameters;
				if (string.IsNullOrEmpty(this.TwitchSecret)) {
					QueryParameters = new (string, string)[] {
						(GetTokenRefresh.CLIENT_ID, this.TwitchClientID),
						(GetTokenRefresh.REFRESH_TOKEN, code),
						GetTokenRefresh.GRANT_TYPE,
					};
				}
				else {
					QueryParameters = new (string, string)[] {
						(GetTokenRefresh.CLIENT_ID, this.TwitchClientID),
						(GetTokenRefresh.REFRESH_TOKEN, code),
						(GetTokenRefresh.CLIENT_SECRET, this.TwitchSecret),
						GetTokenRefresh.GRANT_TYPE,
					};
				}

				TwitchAPIDataContainer<GetTokenRefresh> tokenRequest = await MakeTwitchAPIRequestAsync<GetTokenRefresh>(
					this.currentTokenBeingRefreshed,
					QueryParameters: QueryParameters,
					cancelToken: this.RequestAPICancellationToken.Token);

				if (!this.GettingNewToken) {
					return;
				}

				if (tokenRequest.HasErrored) {
					throw new Exception(tokenRequest.ErrorToJson());
				}
				else {
					GetTokenRefresh container = tokenRequest.data[0];

					DeviceCodeGrantFlow refreshedToken = new DeviceCodeGrantFlow(container);

					TwitchAPIDataContainer<GetValidatedTokenInfo> tokenValidatedInfo = await MakeTwitchAPIRequestAsync<GetValidatedTokenInfo>(
						this.currentTokenBeingRefreshed,
						new (string, string)[] {
								(GetValidatedTokenInfo.AUTHORIZATION, TwitchStatic.AppendOAuthToBearer(refreshedToken.Access_Token))
						},
						cancelToken: this.RequestAPICancellationToken.Token);

					if (tokenValidatedInfo.HasErrored) {
						throw new Exception(tokenValidatedInfo.ErrorToJson());
					}
					else {
						GetValidatedTokenInfo tokenData = tokenValidatedInfo.data[0];

						if (tokenData.expires_in > int.MinValue) {
							refreshedToken.Expires_In = tokenData.expires_in;
						}

						if (this.currentTokenBeingRefreshed != null) {
							this.currentTokenBeingRefreshed.OAuthToken = refreshedToken;
							TokenInstance token = this.currentTokenBeingRefreshed;
							MainThreadDispatchQueue.Enqueue(() => this.WriteAuthenticationSuccess(token));
						}
					}
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				MainThreadDispatchQueue.Enqueue(() => this.OnAuthenticationFailure?.Invoke(ex));

				if (this.DefaultAPIToken.StartNewOnRefreshFail) {
					await this.GetDeviceCodeGrantFlowInitial();
				}
			} finally {
				this.currentTokenBeingRefreshed = null;
			}
		}

		private void WriteAuthenticationSuccess(TokenInstance token) {
			this.WriteTokenToSettings(token, this.LogDebugLevel != DebugManager.DebugLevel.None);
			this.OnAuthenticationSuccess?.Invoke();
		}

		#endregion

	}
}