using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Views : IShared {

		[field: SerializeField] public Mobile mobile { get; set; }
		[field: SerializeField] public Panel panel { get; set; }
		[field: SerializeField] public VideoOverlay video_overlay { get; set; }
		[field: SerializeField] public ViewComponent component { get; set; }

		public Views(JsonValue body) {
			this.mobile = new Mobile(body[TwitchWords.MOBILE]);
			this.panel = new Panel(body[TwitchWords.PANEL]);
			this.video_overlay = new VideoOverlay(body[TwitchWords.VIDEO_OVERLAY]);
			this.component = new ViewComponent(body[TwitchWords.COMPONENT]);
		}
	}
}
