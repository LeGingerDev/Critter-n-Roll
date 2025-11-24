using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct VideoOverlay : IShared {

		[field: SerializeField] public string viewer_url { get; set; }
		[field: SerializeField] public bool can_link_external_content { get; set; }

		public VideoOverlay(JsonValue body) {
			this.viewer_url = body[TwitchWords.VIEWER_URL].AsString;
			this.can_link_external_content = body[TwitchWords.CAN_LINK_EXTERNAL_CONTENT].AsBoolean;
		}
	}
}
