using System;

using ScoredProductions.StreamLinked.API.Conduits;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API {

	/// <summary>
	/// Returned container from API requests.
	/// </summary>
	/// <typeparam name="T">Twitch API Class</typeparam>
	[Serializable]
	public struct TwitchAPIDataContainer<T> where T : ITwitchAPIDataObject, new() {

		/// <summary>
		/// If request was hit with an error response
		/// </summary>
		[field: SerializeField] public bool HasErrored { get; set; }
		/// <summary>
		/// Error text produced by the API request
		/// </summary>
		[field: SerializeField] public string ErrorText { get; set; }
		/// <summary>
		/// Status code returned from the call
		/// </summary>
		[field: SerializeField] public int StatusCode { get; set; }
		/// <summary>
		/// If the endpoint supports it, errors will be populated here, will require casting
		/// </summary>
		[field: SerializeField] public object[] ErrorObjects { get; set; }

		/// <summary>
		/// The raw information returned from the request. You should only need this if the providing class has not been updated to include your desired value or error information.
		/// </summary>
		[field: SerializeField] public string RawResponse { get; set; }

		/// <summary>
		/// Status value found in message
		/// </summary>
		[field: SerializeField] public int status { get; set; }
		/// <summary>
		/// Message provided with Status value
		/// </summary>
		[field: SerializeField] public string message { get; set; }

		/// <summary>
		/// The returned data from the API call point, usually provided from a <c>JsonArray</c> called 'data', processed into a array of the provided generic.
		/// </summary>
		[field: SerializeField] public T[] data { get; set; }
		[field: SerializeField] public string template { get; set; }
		// Eventsub
		[field: SerializeField] public DateRange date_range { get; set; }
		[field: SerializeField] public Pagination pagination { get; set; }

		[field: SerializeField] public EventSubCosting EventSubData { get; set; }

		/// <summary>
		/// The raw information returned from the request. You should only need this if the providing class has not been updated to include your desired value or error information.
		/// </summary>
		public readonly JsonValue RequestDataJSON => JsonReader.Parse(this.RawResponse);

		public TwitchAPIDataContainer(JsonValue body) {
			JsonValue twitchData = body[TwitchAPIClient.RESPONSE];
			this.RawResponse = JsonWriter.Serialize(twitchData, true);

			JsonValue innerData = twitchData[TwitchWords.DATA];
			bool hasResponse = ITwitchAPIDataObject.HasResponse<T>();
			if (innerData.IsNull) { // Data value container not found so use the response data to populate values
				innerData = twitchData;
			}

			if (innerData.IsNull) {
				this.data = Array.Empty<T>();
			} else {
				if (innerData.IsJsonArray) {
					JsonArray dataArray = innerData.AsJsonArray ?? new JsonArray();
					this.data = new T[dataArray.Count];
					for (int x = 0; x < dataArray.Count; x++) {
						this.data[x] = new T();
						if (hasResponse) {
							this.data[x].Initialise(dataArray[x]);
						}
					}
				} else {
					this.data = new T[] { new T() };
					if (!innerData.IsNull && hasResponse) {
						this.data[0].Initialise(innerData);
					}
				}
			}

			this.template = twitchData[TwitchWords.TEMPLATE].AsString;

			this.date_range = new DateRange(twitchData[TwitchWords.DATE_RANGE]);
			this.pagination = twitchData[TwitchWords.PAGINATION].IsJsonObject
				? new Pagination(twitchData[TwitchWords.PAGINATION]) 
				: new Pagination(twitchData[TwitchWords.PAGINATION].AsString);

			this.EventSubData = new EventSubCosting(twitchData);

			if (typeof(IConduits).IsAssignableFrom(typeof(T))) {
				JsonArray jerrors = twitchData[TwitchWords.ERRORS].AsJsonArray ?? new JsonArray();
				this.ErrorObjects = new object[jerrors.Count];
				int count = 0;
				foreach (JsonValue error in jerrors) {
					this.ErrorObjects[count++] = new Errors(error);
				}
			} else {
				this.ErrorObjects = Array.Empty<object>();
			}

			this.status = twitchData[TwitchWords.STATUS].AsInteger;
			this.message = twitchData[TwitchWords.MESSAGE].AsString;

			this.HasErrored = body[TwitchAPIClient.HAS_ERRORED].AsBoolean;
			this.ErrorText = body[TwitchAPIClient.ERROR_TEXT].AsString;
			this.StatusCode = body[TwitchAPIClient.STATUS_CODE].AsInteger;
		}

		public readonly JsonValue ErrorToJson() {
			JsonObject json = new JsonObject() {
				{ nameof(this.HasErrored), this.HasErrored },
				{ nameof(this.RawResponse), this.RawResponse },
				{ nameof(this.StatusCode), this.StatusCode },
			};
			return json;
		}
	}
}