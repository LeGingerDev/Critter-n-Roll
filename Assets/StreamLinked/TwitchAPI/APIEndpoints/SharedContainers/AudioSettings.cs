using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct AudioSettings : IShared {

		[field: SerializeField] public bool is_available { get; set; }
		[field: SerializeField] public bool is_host_enabled { get; set; }
		[field: SerializeField] public bool is_guest_enabled { get; set; }

		public AudioSettings(JsonValue body) {
			this.is_available = body[TwitchWords.IS_AVAILABLE].AsBoolean;
			this.is_host_enabled = body[TwitchWords.IS_HOST_ENABLED].AsBoolean;
			this.is_guest_enabled = body[TwitchWords.IS_GUEST_ENABLED].AsBoolean;
		}
	}
}
