using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Entitlements {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-drops-entitlements">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateDropsEntitlements : IEntitlements, IJsonRequest {

		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public string[] ids { get; set; }

		public void Initialise(JsonValue body) {
			this.status = body[TwitchWords.STATUS].AsString;
			this.ids = body[TwitchWords.IDS].AsJsonArray?.CastToStringArray;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.DropsEntitlements;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateDropsEntitlements;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string fulfillment_status = null,
												string[] entitlement_ids = null) {
			JsonObject body = new JsonObject();
			if (!string.IsNullOrEmpty(fulfillment_status)) {
				body.Add(TwitchWords.FULFILLMENT_STATUS, fulfillment_status);
			}
			if (entitlement_ids != null) {
				body.Add(TwitchWords.ENTITLEMENT_IDS, new JsonArray(entitlement_ids));
			}
			return body.Count > 0 ? JsonWriter.Serialize(body) : string.Empty;
		}

	}
}