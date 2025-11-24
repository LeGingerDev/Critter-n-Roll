using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using UnityEngine;
using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-extension-bits-product">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateExtensionBitsProduct : IExtensions, IJsonRequest {

		[field: SerializeField] public string sku { get; set; }
		[field: SerializeField] public Cost cost { get; set; }
		[field: SerializeField] public bool in_development { get; set; }
		[field: SerializeField] public string display_name { get; set; }
		[field: SerializeField] public string expiration { get; set; }
		[field: SerializeField] public bool is_broadcast { get; set; }

		public void Initialise(JsonValue body) {
			this.sku = body[TwitchWords.SKU].AsString;
			this.cost = new Cost(body[TwitchWords.COST]);
			this.in_development = body[TwitchWords.IN_DEVELOPMENT].AsBoolean;
			this.display_name = body[TwitchWords.DISPLAY_NAME].AsString;
			this.expiration = body[TwitchWords.EXPIRATION].AsString;
			this.is_broadcast = body[TwitchWords.IS_BROADCAST].AsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PUT;
		public readonly string Endpoint => TwitchAPILinks.ExtensionBitsProducts;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateExtensionBitsProduct;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string sku,
												(string amount, string type) cost,
												string display_name,
												bool? in_development = null,
												string expiration = null,
												bool? is_broadcast = null) {
			JsonObject body = new JsonObject() {
					{ TwitchWords.SKU, sku },
					{
						TwitchWords.COST, new JsonObject() {
							{ TwitchWords.AMOUNT, cost.amount },
							{ TwitchWords.TYPE, cost.type }
						}
					},
					{ TwitchWords.DISPLAY_NAME, display_name }
				};
			if (in_development.HasValue) {
				body.Add(TwitchWords.IN_DEVELOPMENT, in_development);
			}
			if (!string.IsNullOrEmpty(expiration)) {
				body.Add(TwitchWords.EXPIRATION, expiration);
			}
			if (is_broadcast.HasValue) {
				body.Add(TwitchWords.IS_BROADCAST, is_broadcast);
			}
			return JsonWriter.Serialize(body);
		}
	}
}