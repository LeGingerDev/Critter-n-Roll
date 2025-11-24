using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct User : IShared {
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string user_login { get; set; }

		public User(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
		}
	}
}
