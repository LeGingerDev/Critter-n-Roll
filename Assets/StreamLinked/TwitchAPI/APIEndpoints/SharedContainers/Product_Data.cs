using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Product_Data : IShared {

		[field: SerializeField] public string sku { get; set; }
		[field: SerializeField] public string domain { get; set; }
		[field: SerializeField] public Cost cost { get; set; }
		[field: SerializeField] public bool inDevelopment { get; set; }
		[field: SerializeField] public string displayName { get; set; }
		[field: SerializeField] public string expiration { get; set; }
		[field: SerializeField] public bool broadcast { get; set; }

		public Product_Data(JsonValue body) {
			this.sku = body[TwitchWords.SKU].AsString;
			this.domain = body[TwitchWords.DOMAIN].AsString;
			this.cost = new Cost(body[TwitchWords.COST]);
			this.inDevelopment = body[TwitchWords.INDEVELOPMENT].AsBoolean;
			this.displayName = body[TwitchWords.DISPLAYNAME].AsString;
			this.expiration = body[TwitchWords.EXPIRATION].AsString;
			this.broadcast = body[TwitchWords.BROADCAST].AsBoolean;
		}
	}

}
