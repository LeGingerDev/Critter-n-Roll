using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-extension-bits-products">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetExtensionBitsProducts : IExtensions {

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

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.ExtensionBitsProducts;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetExtensionBitsProducts;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string SHOULD_INCLUDE_ALL => TwitchWords.SHOULD_INCLUDE_ALL;

	}
}