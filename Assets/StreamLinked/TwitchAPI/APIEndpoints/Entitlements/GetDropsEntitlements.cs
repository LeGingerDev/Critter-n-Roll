using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Entitlements {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-drops-entitlements">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetDropsEntitlements : IEntitlements {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string benefit_id { get; set; }
		[field: SerializeField] public string timestamp { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string game_id { get; set; }
		[field: SerializeField] public string fulfillment_status { get; set; }
		[field: SerializeField] public string last_updated { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.benefit_id = body[TwitchWords.BENEFIT_ID].AsString;
			this.timestamp = body[TwitchWords.TIMESTAMP].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.game_id = body[TwitchWords.GAME_ID].AsString;
			this.fulfillment_status = body[TwitchWords.FULFILLMENT_STATUS].AsString;
			this.last_updated = body[TwitchWords.LAST_UPDATED].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.DropsEntitlements;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetDropsEntitlements;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

	}
}