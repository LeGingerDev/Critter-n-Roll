using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ScoredProductions.StreamLinked.API.Auth;
using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.Networking;

namespace ScoredProductions.StreamLinked.API {

	[Flags]
	public enum APIScopeWarning {
		None = 0,
		/// <summary>
		/// Default
		/// </summary>
		WarnOnMissing = 1,
		ThrowOnMissing = 2,
		AddMissingScopes = 4,
	}

	public partial class TwitchAPIClient // .TwitchAPIRequest
	{

		public const string ENDPOINT = "Endpoint";
		public const string RESPONSE = "Response";
		public const string STATUS_CODE = "Status_Code";
		public const string HAS_ERRORED = "Has_Errored";
		public const string ERROR_TEXT = "Error_Text";

		// Make compatible with scope warning on Type version functions

		private (string, string) AquireClientIDParams() {
			return (TwitchWords.CLIENTID, this.TwitchClientID);
		}

		/// <summary>
		/// Synchronous call to Twitch API returning the data. Best for testing calls or making calls off the main thread. Will hang the frame until completion. Not recommended for deployment use.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Timeout">Time in milliseconds before the request is cancelled</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="ScopeSettings">Actions to take when the required scopes are missing.</param>
		/// <returns></returns>
		/// <exception cref="TimeoutException"></exception>
		public static TwitchAPIDataContainer<T> MakeTwitchAPIRequest<T>(
												int Timeout,
												TokenInstance Credentials = null,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null,
												APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing)
												where T : ITwitchAPIDataObject, new() {
			if (Timeout < 1) {
				Timeout = 1;
			}
			T requestObject = new T();
			TwitchAPIDataContainer<T> returnedObject = default;

			if (Task.Run(
					async () => {
						returnedObject = await MakeTwitchAPIRequestAsync<T>(Credentials, HeaderValues, QueryParameters, RawData, ScopeSettings, new CancellationTokenSource(Timeout).Token);
					}
				).Wait(Timeout)) {
				return returnedObject;
			}
			else {
				throw new TimeoutException($"MakeTwitchAPIRequest<{typeof(T).Name}> request timed out after {Timeout} milliseconds");
			}
		}

		/// <summary>		
		/// Synchronous call to Twitch API returning the data. Best for testing calls or making calls off the main thread. Will hang the frame until completion. Not recommended for deployment use.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Timeout">Time in milliseconds before the request is cancelled</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns></returns>
		/// <exception cref="TimeoutException"></exception>
		public static JsonValue MakeTwitchAPIRequestJson(
											string Endpoint,
											TwitchAPIRequestMethod Method,
											int Timeout,
											TokenInstance Credentials = null,
											(string, string)[] HeaderValues = null,
											(string, string)[] QueryParameters = null,
											string RawData = null) {
			if (Timeout < 1) {
				Timeout = 1;
			}
			JsonValue returnedObject = JsonValue.Null;

			if (Task.Run(
					async () => {
						returnedObject = await MakeTwitchAPIRequestJsonAsync(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, new CancellationTokenSource(Timeout).Token);
					}
				).Wait(Timeout)) {
				return returnedObject;
			}
			else {
				throw new TimeoutException($"MakeTwitchAPIRequest request timed out after {Timeout} milliseconds");
			}
		}

		/// <summary>		
		/// Synchronous call to Twitch API returning the data. Best for testing calls or making calls off the main thread. Will hang the frame until completion. Not recommended for deployment use.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Timeout">Time in milliseconds before the request is cancelled</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns></returns>
		/// <exception cref="TimeoutException"></exception>
		public static string MakeTwitchAPIRequestRaw(
											string Endpoint,
											TwitchAPIRequestMethod Method,
											int Timeout,
											TokenInstance Credentials = null,
											(string, string)[] HeaderValues = null,
											(string, string)[] QueryParameters = null,
											string RawData = null) {
			if (Timeout < 1) {
				Timeout = 1;
			}
			string returnedObject = string.Empty;

			if (Task.Run(
					async () => {
						returnedObject = await MakeTwitchAPIRequestRawAsync(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, new CancellationTokenSource(Timeout).Token);
					}
				).Wait(Timeout)) {
				return returnedObject;
			}
			else {
				throw new TimeoutException($"MakeTwitchAPIRequest request timed out after {Timeout} milliseconds");
			}
		}

		/// <summary>
		/// Coroutine to make requests to Twitchs API. Access the data via SuccessCallback.
		/// </summary>
		/// <typeparam name="T">ITwitchAPIDataObject datatype inside a TwitchAPIDataContainer</typeparam>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="ScopeSettings">Actions to take when the required scopes are missing.</param>
		/// <exception cref="UnityException"></exception>
		public Coroutine MakeTwitchAPIRequest<T>(
												Action<TwitchAPIDataContainer<T>> SuccessCallback,
												TokenInstance Credentials = null,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null,
												APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing)
												where T : ITwitchAPIDataObject, new() {
			return this.StartCoroutine(MakeTwitchAPIRequest<T>(Credentials, HeaderValues, QueryParameters, RawData, ScopeSettings, SuccessCallback));
		}

		/// <summary>
		/// Coroutine to make requests to Twitchs API. Access the data via returned SuccessCallback.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="IncludeAuthHeaders">Includes Client ID and Auth token for non-standard endpoints</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		public Coroutine MakeTwitchAPIRequestJson(
												string Endpoint,
												TwitchAPIRequestMethod Method,
												Action<JsonValue> SuccessCallback,
												TokenInstance Credentials = null,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null) {
			return this.StartCoroutine(MakeTwitchAPIRequestJson(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, SuccessCallback));
		}

		/// <summary>
		/// Coroutine to make requests to Twitchs API. Access the data via returned SuccessCallback.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="IncludeAuthHeaders">Includes Client ID and Auth token for non-standard endpoints</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		public Coroutine MakeTwitchAPIRequestRaw(
												string Endpoint,
												TwitchAPIRequestMethod Method,
												Action<string> SuccessCallback,
												TokenInstance Credentials = null,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null) {
			return this.StartCoroutine(MakeTwitchAPIRequestRaw(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, SuccessCallback));
		}

		/// <summary>
		/// Enumerator (Coroutine) to make requests to Twitchs API. Access the data via returned IEnumerator or SuccessCallback.
		/// </summary>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="ScopeSettings">Actions to take when the required scopes are missing.</param>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		/// <typeparam name="T">ITwitchAPIDataObject datatype inside a TwitchAPIDataContainer</typeparam>
		public static IEnumerator MakeTwitchAPIRequest<T>(
												TokenInstance Credentials = null,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null,
												APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing,
												Action<TwitchAPIDataContainer<T>> SuccessCallback = null)
												where T : ITwitchAPIDataObject, new() {
			T requestType = new T();
			string Endpoint = requestType.Endpoint;
			TwitchAPIRequestMethod Method = requestType.HTTPMethod;

			IEnumerator internalRun = InternalMakeTwitchAPIRequest(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, requestType, ScopeSettings);
			yield return internalRun;
			TwitchAPIDataContainer<T> objectResult;
			string result = string.Empty;
			try {
				result = (string)internalRun.Current;
				JsonValue parsedResult = JsonReader.Parse(result);
				objectResult = new TwitchAPIDataContainer<T>(parsedResult);
				SuccessCallback?.Invoke(objectResult);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
				objectResult = new TwitchAPIDataContainer<T>() {
					RawResponse = result,
					HasErrored = true,
					ErrorText = ex.Message,
				};
			}
			yield return objectResult;
		}

		/// <summary>
		/// Enumerator (Coroutine) to make requests to Twitchs API. Access the data via returned IEnumerator or SuccessCallback.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		public static IEnumerator MakeTwitchAPIRequestJson<T>(
												TokenInstance Credentials = null,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null,
												APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing,
												Action<JsonValue> SuccessCallback = null)
												where T : ITwitchAPIDataObject, new() {
			T requestType = new T();
			string Endpoint = requestType.Endpoint;
			TwitchAPIRequestMethod Method = requestType.HTTPMethod;
			IEnumerator internalRun = InternalMakeTwitchAPIRequest(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, requestType, ScopeSettings);
			yield return internalRun;
			JsonValue parsedResult = JsonValue.Null;
			try {
				string result = (string)internalRun.Current;
				parsedResult = JsonReader.Parse(result);
				SuccessCallback?.Invoke(parsedResult);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}
			yield return parsedResult;
		}

		/// <summary>
		/// Enumerator (Coroutine) to make requests to Twitchs API. Access the data via returned IEnumerator or SuccessCallback.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		public static IEnumerator MakeTwitchAPIRequestJson(
												string Endpoint,
												TwitchAPIRequestMethod Method,
												TokenInstance Credentials = null,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null,
												Action<JsonValue> SuccessCallback = null) {
			IEnumerator internalRun = InternalMakeTwitchAPIRequest(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData);
			yield return internalRun;
			JsonValue parsedResult = JsonValue.Null;
			try {
				string result = (string)internalRun.Current;
				parsedResult = JsonReader.Parse(result);
				SuccessCallback?.Invoke(parsedResult);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}
			yield return parsedResult;
		}

		/// <summary>
		/// Enumerator (Coroutine) to make requests to Twitchs API. Access the data via returned IEnumerator or SuccessCallback.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		public static IEnumerator MakeTwitchAPIRequestRaw<T>(
												TokenInstance Credentials,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null,
												APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing,
												Action<string> SuccessCallback = null)
												where T : ITwitchAPIDataObject, new() {
			T requestType = new T();
			string Endpoint = requestType.Endpoint;
			TwitchAPIRequestMethod Method = requestType.HTTPMethod;

			IEnumerator internalRun = InternalMakeTwitchAPIRequest(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, requestType, ScopeSettings);
			yield return internalRun;
			string result = string.Empty;
			try {
				result = (string)internalRun.Current;
				SuccessCallback?.Invoke(result);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}
			yield return result;
		}

		/// <summary>
		/// Enumerator (Coroutine) to make requests to Twitchs API. Access the data via returned IEnumerator or SuccessCallback.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="SuccessCallback">Callback to occur on request success</param>
		public static IEnumerator MakeTwitchAPIRequestRaw(
												string Endpoint,
												TwitchAPIRequestMethod Method,
												TokenInstance Credentials,
												(string, string)[] HeaderValues = null,
												(string, string)[] QueryParameters = null,
												string RawData = null,
												Action<string> SuccessCallback = null) {
			IEnumerator internalRun = InternalMakeTwitchAPIRequest(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData);
			yield return internalRun;
			string result = string.Empty;
			try {
				result = (string)internalRun.Current;
				SuccessCallback?.Invoke(result);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}
			yield return result;
		}

		/// <summary>
		/// Main Task to make requests to Twitchs API.
		/// </summary>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <param name="ScopeSettings">Actions to take when the required scopes are missing.</param>
		/// <typeparam name="T">ITwitchAPIDataObject datatype inside a TwitchAPIDataContainer</typeparam>
		/// <returns><code>ITwitchAPIData</code> (If returned from endpoint)</returns>
		public static async Task<TwitchAPIDataContainer<T>> MakeTwitchAPIRequestAsync<T>(
											TokenInstance Credentials = null,
											(string, string)[] HeaderValues = null,
											(string, string)[] QueryParameters = null,
											string RawData = null,
											APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing,
											CancellationToken cancelToken = default)
											where T : ITwitchAPIDataObject, new() {
			cancelToken.ThrowIfCancellationRequested();
			T requestType = new T();

			string Endpoint = requestType.Endpoint;
			TwitchAPIRequestMethod Method = requestType.HTTPMethod;

			TwitchAPIDataContainer<T> returned;
			string result = string.Empty;
			try {
				result = await InternalMakeTwitchAPIRequestAsync(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, cancelToken, requestType, ScopeSettings);
				cancelToken.ThrowIfCancellationRequested();
				JsonValue parsedResult = JsonReader.Parse(result);
				returned = new TwitchAPIDataContainer<T>(parsedResult);
				return returned;
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
				returned = new TwitchAPIDataContainer<T>() {
					RawResponse = result,
					HasErrored = true,
					ErrorText = ex.Message,
				};
			}
			return returned;

		}

		/// <summary>
		/// Main Task to make requests to Twitchs API.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns><code>JsonValue</code> Raw response from server</returns>
		public static async Task<JsonValue> MakeTwitchAPIRequestJsonAsync<T>(
											TokenInstance Credentials = null,
											(string, string)[] HeaderValues = null,
											(string, string)[] QueryParameters = null,
											string RawData = null,
											APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing,
											CancellationToken cancelToken = default)
											where T : ITwitchAPIDataObject, new() {
			cancelToken.ThrowIfCancellationRequested();
			T requestType = new T();

			string Endpoint = requestType.Endpoint;
			TwitchAPIRequestMethod Method = requestType.HTTPMethod;

			JsonValue returned = JsonValue.Null;
			try {
				string result = await InternalMakeTwitchAPIRequestAsync(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, cancelToken, requestType, ScopeSettings);
				cancelToken.ThrowIfCancellationRequested();
				returned = JsonReader.Parse(result);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}
			return returned;
		}

		/// <summary>
		/// Main Task to make requests to Twitchs API.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns><code>JsonValue</code> Raw response from server</returns>
		public static async Task<JsonValue> MakeTwitchAPIRequestJsonAsync(
											string Endpoint,
											TwitchAPIRequestMethod Method,
											TokenInstance Credentials = null,
											(string, string)[] HeaderValues = null,
											(string, string)[] QueryParameters = null,
											string RawData = null,
											CancellationToken cancelToken = default) {
			cancelToken.ThrowIfCancellationRequested();
			JsonValue returned = JsonValue.Null;
			try {
				string result = await InternalMakeTwitchAPIRequestAsync(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, cancelToken);
				cancelToken.ThrowIfCancellationRequested();
				returned = JsonReader.Parse(result);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}
			return returned;
		}

		/// <summary>
		/// Main Task to make requests to Twitchs API. Uses the OAuth token supplied in the API client. Returns the compartmented data sent to standard MakeTwitchAPIRequest methods. Keys available as consts from TwitchAPIClient.
		/// </summary>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns><code>JsonValue</code> Raw response from server</returns>
		public static Task<string> MakeTwitchAPIRequestRawAsync<T>(
											TokenInstance Credentials = null,
											(string, string)[] HeaderValues = null,
											(string, string)[] QueryParameters = null,
											string RawData = null,
											APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing,
											CancellationToken cancelToken = default)
											where T : ITwitchAPIDataObject, new() {
			T requestType = new T();
			string Endpoint = requestType.Endpoint;
			TwitchAPIRequestMethod Method = requestType.HTTPMethod;
			return InternalMakeTwitchAPIRequestAsync(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, cancelToken, requestType, ScopeSettings);
		}

		/// <summary>
		/// Main Task to make requests to Twitchs API. Uses the OAuth token supplied in the API client. Returns the compartmented data sent to standard MakeTwitchAPIRequest methods. Keys available as consts from TwitchAPIClient.
		/// </summary>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns><code>JsonValue</code> Raw response from server</returns>
		public static Task<string> MakeTwitchAPIRequestRawAsync(
											string Endpoint,
											TwitchAPIRequestMethod Method,
											TokenInstance Credentials = null,
											(string, string)[] HeaderValues = null,
											(string, string)[] QueryParameters = null,
											string RawData = null,
											CancellationToken cancelToken = default) {
			return InternalMakeTwitchAPIRequestAsync(Endpoint, Method, Credentials, HeaderValues, QueryParameters, RawData, cancelToken);
		}

		/// <summary>
		/// Main Task to make requests to Twitchs API. Returned in non pretty Json.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns><code>string</code> Raw response from server</returns>
		private static IEnumerator InternalMakeTwitchAPIRequest(
											string Endpoint,
											TwitchAPIRequestMethod Method,
											TokenInstance Credentials,
											(string, string)[] HeaderValues,
											(string, string)[] QueryParameters,
											string RawData,
											ITwitchAPIDataObject reference = null,
											APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing) {
			if (CreateOrGetInstance(out TwitchAPIClient client)) {
				if (Credentials == null) {
					if (client.DefaultAPIToken == null) {
						DebugManager.LogMessage($"TwitchAPIClient no token provided by method or available inside API client, Task Aborted: {Endpoint} {Method}", DebugManager.ErrorLevel.Error);
						yield break;
					}
					Credentials = client.DefaultAPIToken;
				}
			}
			else {
				DebugManager.LogMessage($"TwitchAPIClient does not currently exist, Task Aborted: {Endpoint} {Method}", DebugManager.ErrorLevel.Error);
				yield break;
			}

			bool isAuthRequest = IAuth.EndpointIsAuthRequest(Endpoint);

			bool retryPerformed = false;
			retry:
			if (client.LogDebugLevel != DebugManager.DebugLevel.None) {
				DebugManager.LogMessage($"Attempting Call to Twitch API, Endpoint: {Endpoint}, Method: {Method}, HeaderValues: {HeaderValues.ToJSONString()}, QueryParameters: {QueryParameters.ToJSONString()}, RawData: {RawData}".RichTextColour(Color.cyan));
			}
			StringBuilder builder = new StringBuilder(Endpoint);

			if (Credentials == null) {
				if (client.DefaultAPIToken == null) {
					DebugManager.LogMessage($"TwitchAPIClient no token provided by method or available inside API client, Task Aborted: {Endpoint} {Method}", DebugManager.ErrorLevel.Error);
					yield break;
				}
				Credentials = client.DefaultAPIToken;
			}

			if (reference != null) {
				Credentials.PerformScopeCheck(in ScopeSettings, reference);
			}

			if (isAuthRequest) {
				while (client.currentTokenBeingRefreshed != Credentials) {
					yield return TwitchStatic.OneSecondWait;
				}
			}
			else {
				if (Credentials.CheckRefreshNeeded()) {
					client.GetNewAuthToken(Credentials);
				}

				if (client.GettingNewToken || authRequestOrder.Count > 0 || Credentials.HasToken == false) {
					WaitWhile waiting = new WaitWhile(() => client.GettingNewToken || authRequestOrder.Count > 0 || Credentials.HasToken == false);

					yield return waiting;
				}
			}

			for (int x = 0; x < QueryParameters?.Length; x++) {
				if (x != 0) {
					builder.Append('&');
				}
				else {
					builder.Append('?');
				}
				builder.Append(QueryParameters[x].Item1);
				builder.Append('=');
				builder.Append(QueryParameters[x].Item2);
			}
			string builtEndpoint = builder.ToString();

			using (UnityWebRequest webRequest = new UnityWebRequest(builtEndpoint, Method.ToString())) {
				webRequest.downloadHandler = new DownloadHandlerBuffer();
				if (!string.IsNullOrEmpty(RawData)) {
					webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(RawData)) {
						contentType = "application/json"
					};
				}

				if (!isAuthRequest) {
					string authValue = TwitchStatic.AppendOAuthToBearer(Credentials.OAuthToken.Access_Token);
					webRequest.SetRequestHeader(TwitchWords.AUTHORIZATION, authValue);
					(string clientName, string clientValue) = client.AquireClientIDParams();
					webRequest.SetRequestHeader(clientName, clientValue);
				}

				if (HeaderValues?.Length > 0) {
					for (int x = 0; x < HeaderValues.Length; x++) {
						webRequest.SetRequestHeader(HeaderValues[x].Item1, HeaderValues[x].Item2);
					}
				}

				yield return webRequest.SendWebRequest();

				builder.Clear();
				builder.Append('{');
				AppendValueToBuilder(ref builder, false, ENDPOINT, builtEndpoint, true);

				if (webRequest.result == UnityWebRequest.Result.Success) {
					if (client.LogDebugLevel != DebugManager.DebugLevel.None) {
						DebugManager.LogMessage($"Twitch API call successful, Endpoint: {builtEndpoint}".RichTextColour("green"));
					}

					AppendValueToBuilder(ref builder, true, RESPONSE, webRequest.downloadHandler.text, string.Equals(Endpoint, TwitchAPILinks.GetChanneliCalendar));

					AppendValueToBuilder(ref builder, true, STATUS_CODE, webRequest.responseCode.ToString(), false);

					AppendValueToBuilder(ref builder, true, HAS_ERRORED, webRequest.responseCode < 200 | webRequest.responseCode > 299 ? "true" : "false", false);

					AppendValueToBuilder(ref builder, true, ERROR_TEXT, "", true);
				}
				else {
					if (client.LogDebugLevel != DebugManager.DebugLevel.None) {
						DebugManager.LogMessage(webRequest.error);
					}
					if (webRequest.responseCode == 401 && Credentials.AutoRetrieveNewAuth) {
						client.GetNewAuthToken(Credentials);

						if (!retryPerformed) {
							retryPerformed = true;
							if (client.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
								DebugManager.LogMessage($"Attempting to Retry Call to Twitch API after Auth refresh, Endpoint: {builtEndpoint}".RichTextColour("orange"));
							}
							goto retry;
						}
					}

					AppendValueToBuilder(ref builder, true, RESPONSE, webRequest.downloadHandler.text, true);

					AppendValueToBuilder(ref builder, true, STATUS_CODE, webRequest.responseCode.ToString(), false);

					AppendValueToBuilder(ref builder, true, HAS_ERRORED, "true", false);

					AppendValueToBuilder(ref builder, true, ERROR_TEXT, webRequest.error, true);
				}
				builder.Append('}');

				webRequest.downloadHandler?.Dispose();
				webRequest.uploadHandler?.Dispose();
			}
			yield return builder.ToString();
		}

		/// <summary>
		/// Main Task to make requests to Twitchs API. Returned in non pretty Json.
		/// </summary>
		/// <param name="Endpoint">API URL.</param>
		/// <param name="Method">HTTP Method Type.</param>
		/// <param name="Credentials">OAuth token to use for this request.</param>
		/// <param name="HeaderValues">Values to be loaded into the request Header.</param>
		/// <param name="QueryParameters">Values to be loaded into the UploadHandler.</param>
		/// <param name="RawData">Data body to be sent to Twitch</param>
		/// <returns><code>string</code> Raw response from server</returns>
		private static async Task<string> InternalMakeTwitchAPIRequestAsync(
											string Endpoint,
											TwitchAPIRequestMethod Method,
											TokenInstance Credentials,
											(string, string)[] HeaderValues,
											(string, string)[] QueryParameters,
											string RawData,
											CancellationToken cancelToken,
											ITwitchAPIDataObject reference = null,
											APIScopeWarning ScopeSettings = APIScopeWarning.WarnOnMissing) {
			if (CreateOrGetInstance(out TwitchAPIClient client)) {
				if (Credentials == null) {
					if (client.DefaultAPIToken == null) {
						throw new Exception($"TwitchAPIClient no token provided by method or available inside API client, Task Aborted: {Endpoint} {Method}");
					}
					Credentials = client.DefaultAPIToken;
				}
			}
			else {
				throw new Exception($"TwitchAPIClient does not currently exist, Task Aborted: {Endpoint} {Method}");
			}

			cancelToken.ThrowIfCancellationRequested();

			bool isAuthRequest = IAuth.EndpointIsAuthRequest(Endpoint);

			bool retryPerformed = false;
			retry:
			if (cancelToken == default) {
				cancelToken = client.APICancelToken;
			}
			cancelToken.ThrowIfCancellationRequested();

			if (client.LogDebugLevel != DebugManager.DebugLevel.None) {
				DebugManager.LogMessage($"Attempting Call to Twitch API, Endpoint: {Endpoint}, Method: {Method}, HeaderValues: {HeaderValues.ToJSONString()}, QueryParameters: {QueryParameters.ToJSONString()}, RawData: {RawData}".RichTextColour(Color.cyan));
			}

			if (Credentials == null) {
				if (client.DefaultAPIToken == null) {
					throw new Exception($"TwitchAPIClient no token provided by method or available inside API client, Task Aborted: {Endpoint} {Method}");
				}
				Credentials = client.DefaultAPIToken;
			}

			if (reference != null) {
				Credentials.PerformScopeCheck(in ScopeSettings, reference);
			}

			if (isAuthRequest) {
				while (client.currentTokenBeingRefreshed != Credentials) {
					await Task.Delay(1000, cancelToken);
					cancelToken.ThrowIfCancellationRequested();
				}
			}
			else {
				if (Credentials.CheckRefreshNeeded(threadSafe: false)) {
					client.GetNewAuthToken(Credentials);
				}
				while (client.GettingNewToken || authRequestOrder.Count > 0 || Credentials.HasToken == false) {
					await Task.Delay(1000, cancelToken);
					cancelToken.ThrowIfCancellationRequested();
				}
			}

			StringBuilder builder = new StringBuilder(Endpoint);

			for (int x = 0; x < QueryParameters?.Length; x++) {
				if (x != 0) {
					builder.Append('&');
				}
				else {
					builder.Append('?');
				}
				builder.Append(QueryParameters[x].Item1);
				builder.Append('=');
				builder.Append(QueryParameters[x].Item2);
			}
			string builtEndpoint = builder.ToString();

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(builtEndpoint);
			request.Method = Method.ToString();
			request.ContinueTimeout = 2500;
			request.Timeout = 5000;
			request.ContentType = "application/x-www-form-urlencoded";

			if (!isAuthRequest) {
				string authValue = TwitchStatic.AppendOAuthToBearer(Credentials.OAuthToken.Access_Token);
				request.Headers.Add(TwitchWords.AUTHORIZATION, authValue);
				(string clientName, string clientValue) = client.AquireClientIDParams();
				request.Headers.Add(clientName, clientValue);
			}

			for (int x = 0; x < HeaderValues?.Length; x++) {
				if (HeaderValues[x].Item1.Equals(TwitchWords.CONTENT_TYPE)) {
					request.ContentType = HeaderValues[x].Item2;
					(await request.GetRequestStreamAsync()).Write(Encoding.UTF8.GetBytes(RawData));
					cancelToken.ThrowIfCancellationRequested();
				}
				else {
					request.Headers.Add(HeaderValues[x].Item1, HeaderValues[x].Item2);
				}
			}

			cancelToken.ThrowIfCancellationRequested();
			builder.Clear();
			builder.Append('{');
			AppendValueToBuilder(ref builder, false, ENDPOINT, builtEndpoint, true);

			try {
				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
					cancelToken.ThrowIfCancellationRequested();
					string responseString = reader.ReadToEnd();
					int statusCode = (int)response.StatusCode;

					AppendValueToBuilder(ref builder, true, RESPONSE, responseString, string.Equals(Endpoint, TwitchAPILinks.GetChanneliCalendar));

					AppendValueToBuilder(ref builder, true, STATUS_CODE, statusCode.ToString(), false);

					AppendValueToBuilder(ref builder, true, HAS_ERRORED, statusCode < 200 | statusCode > 299 ? "true" : "false", false);

					AppendValueToBuilder(ref builder, true, ERROR_TEXT, "", true);

					if (client.LogDebugLevel != DebugManager.DebugLevel.None) {
						DebugManager.LogMessage($"Twitch API call successful, Endpoint: {builtEndpoint}".RichTextColour("green"));
					}
				}
			} catch (WebException ex) {
				if (client.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				using (HttpWebResponse response = (HttpWebResponse)ex.Response)
				using (StreamReader reader = new StreamReader(response.GetResponseStream())) {

					cancelToken.ThrowIfCancellationRequested();
					string responseString = reader.ReadToEnd();
					int statusCode = (int)response.StatusCode;

					if (statusCode == 401 && Credentials.AutoRetrieveNewAuth) {
						cancelToken.ThrowIfCancellationRequested();
						client.GetNewAuthToken(Credentials);

						// If auth is incorrect perform a single retry
						if (!retryPerformed) {
							retryPerformed = true;
							if (client.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
								DebugManager.LogMessage($"Attempting to Retry Call to Twitch API after Auth refresh, Endpoint: {builtEndpoint}".RichTextColour("orange"));
							}
							goto retry;
						}
					}

					AppendValueToBuilder(ref builder, true, RESPONSE, responseString, false);

					AppendValueToBuilder(ref builder, true, STATUS_CODE, statusCode.ToString(), false);

					AppendValueToBuilder(ref builder, true, HAS_ERRORED, "true", false);

					AppendValueToBuilder(ref builder, true, ERROR_TEXT, ex.Message, true);
				}
			} catch (Exception ex) {
				if (client.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				cancelToken.ThrowIfCancellationRequested();

				AppendValueToBuilder(ref builder, true, RESPONSE, "", false);

				AppendValueToBuilder(ref builder, true, STATUS_CODE, (-1).ToString(), false);

				AppendValueToBuilder(ref builder, true, HAS_ERRORED, "true", false);

				AppendValueToBuilder(ref builder, true, ERROR_TEXT, ex.Message, true);
			}

			builder.Append('}');
			return builder.ToString();
		}

		private static void AppendValueToBuilder(ref StringBuilder builder, bool addComma, string name, string value, bool isString) {
			if (addComma) {
				builder.Append(',');
			}
			builder.Append('"');
			builder.Append(name);
			builder.Append('"');
			builder.Append(':');
			if (isString) {
				builder.Append('"');
				builder.Append(value);
				builder.Append('"');
			}
			else {
				builder.Append(value);
			}
		}
	}
}
