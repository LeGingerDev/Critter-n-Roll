using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Bits {

	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-extension-transactions">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetExtensionTransactions : IBits {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string timestamp { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string product_type { get; set; }
		[field: SerializeField] public Product_Data product_data { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.timestamp = body[TwitchWords.TIMESTAMP].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.product_type = body[TwitchWords.PRODUCT_TYPE].AsString;
			this.product_data = new Product_Data(body[TwitchWords.PRODUCT_DATA]);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetExtensionTransactions;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetExtensionTransactions;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string EXTENSION_ID => TwitchWords.EXTENSION_ID;
		public static string ID => TwitchWords.ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}