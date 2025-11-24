using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Max_Per_User_Per_Stream_Setting : IShared {

		[field: SerializeField] public bool is_enabled { get; set; }
		[field: SerializeField] public int max_per_user_per_stream { get; set; }

		public Max_Per_User_Per_Stream_Setting(JsonValue body) {
			this.is_enabled = body[TwitchWords.IS_ENABLED].AsBoolean;
			this.max_per_user_per_stream = body[TwitchWords.MAX_PER_USER_PER_STREAM].AsInteger;
		}
	}
}
