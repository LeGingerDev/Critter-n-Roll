using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct TermsFound {

		[field: SerializeField] public string term_id { get; set; }
		[field: SerializeField] public Boundary boundary { get; set; }
		[field: SerializeField] public string owner_broadcaster_user_id { get; set; }
		[field: SerializeField] public string owner_broadcaster_user_login { get; set; }
		[field: SerializeField] public string owner_broadcaster_user_name { get; set; }

		public TermsFound(JsonValue body) {
			this.term_id = body[TwitchWords.TERM_ID].AsString;
			this.boundary = new Boundary(body[TwitchWords.BOUNDARY]);
			this.owner_broadcaster_user_id = body[TwitchWords.OWNER_BROADCASTER_USER_ID].AsString;
			this.owner_broadcaster_user_login = body[TwitchWords.OWNER_BROADCASTER_USER_LOGIN].AsString;
			this.owner_broadcaster_user_name = body[TwitchWords.OWNER_BROADCASTER_USER_NAME].AsString;
		}
	}
}