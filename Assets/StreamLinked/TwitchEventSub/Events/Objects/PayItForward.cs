using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events.Objects {
	[Serializable]
	public struct PayItForward {

		[field: SerializeField] public bool gifter_is_anonymous { get; set; }
		[field: SerializeField] public string gifter_user_id { get; set; }
		[field: SerializeField] public string gifter_user_name { get; set; }
		[field: SerializeField] public string gifter_user_login { get; set; }

		public PayItForward(JsonValue body) {
			this.gifter_is_anonymous = body[TwitchWords.GIFTER_IS_ANONYMOUS].AsBoolean;
			this.gifter_user_id = body[TwitchWords.GIFTER_USER_ID].AsString;
			this.gifter_user_name = body[TwitchWords.GIFTER_USER_NAME].AsString;
			this.gifter_user_login = body[TwitchWords.GIFTER_USER_LOGIN].AsString;
		}
	}
}