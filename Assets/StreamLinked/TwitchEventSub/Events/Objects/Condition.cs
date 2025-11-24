using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct Condition {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string moderator_user_id { get; set; }
		[field: SerializeField] public string reward_id { get; set; }
		[field: SerializeField] public string from_broadcaster_user_id { get; set; }
		[field: SerializeField] public string to_broadcaster_user_id { get; set; }
		[field: SerializeField] public string organization_id { get; set; }
		[field: SerializeField] public string category_id { get; set; }
		[field: SerializeField] public string campaign_id { get; set; }
		[field: SerializeField] public string extension_client_id { get; set; }

		public Condition(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.moderator_user_id = body[TwitchWords.MODERATOR_USER_ID].AsString;
			this.reward_id = body[TwitchWords.REWARD_ID].AsString;
			this.from_broadcaster_user_id = body[TwitchWords.FROM_BROADCASTER_USER_ID].AsString;
			this.to_broadcaster_user_id = body[TwitchWords.TO_BROADCASTER_USER_ID].AsString;
			this.organization_id = body[TwitchWords.ORGANIZATION_ID].AsString;
			this.category_id = body[TwitchWords.CATEGORY_ID].AsString;
			this.campaign_id = body[TwitchWords.CAMPAIGN_ID].AsString;
			this.extension_client_id = body[TwitchWords.EXTENSION_CLIENT_ID].AsString;
		}
	}
}