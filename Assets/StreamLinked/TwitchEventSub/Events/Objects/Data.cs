using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Data {
		[field: SerializeField] public string organization_id { get; set; }
		[field: SerializeField] public string category_id { get; set; }
		[field: SerializeField] public string category_name { get; set; }
		[field: SerializeField] public string campaign_id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string entitlement_id { get; set; }
		[field: SerializeField] public string benefit_id { get; set; }
		[field: SerializeField] public string created_at { get; set; }

		public Data(JsonValue body) {
			this.organization_id = body[TwitchWords.ORGANIZATION_ID].AsString;
			this.category_id = body[TwitchWords.CATEGORY_ID].AsString;
			this.category_name = body[TwitchWords.CATEGORY_NAME].AsString;
			this.campaign_id = body[TwitchWords.CAMPAIGN_ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.entitlement_id = body[TwitchWords.ENTITLEMENT_ID].AsString;
			this.benefit_id = body[TwitchWords.BENEFIT_ID].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
		}
	}
}