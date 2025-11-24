using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Global_Cooldown_Setting : IShared {

		[field: SerializeField] public bool is_enabled { get; set; }
		[field: SerializeField] public int global_cooldown_seconds { get; set; }

		public Global_Cooldown_Setting(JsonValue body) {
			this.is_enabled = body[TwitchWords.IS_ENABLED].AsBoolean;
			this.global_cooldown_seconds = body[TwitchWords.GLOBAL_COOLDOWN_SECONDS].AsInteger;
		}
	}
}
