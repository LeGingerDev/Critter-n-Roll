using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	/// <summary>
	/// Named as such to avoid Unity warnings with components
	/// </summary>
	[Serializable]
	public struct ViewComponent : IShared {

		[field: SerializeField] public string viewer_url { get; set; }
		[field: SerializeField] public int aspect_width { get; set; }
		[field: SerializeField] public int aspect_height { get; set; }
		[field: SerializeField] public int aspect_ratio_x { get; set; }
		[field: SerializeField] public int aspect_ratio_y { get; set; }
		[field: SerializeField] public bool autoscale { get; set; }
		[field: SerializeField] public int scale_pixels { get; set; }
		[field: SerializeField] public int target_height { get; set; }
		[field: SerializeField] public int size { get; set; }
		[field: SerializeField] public bool zoom { get; set; }
		[field: SerializeField] public int zoom_pixels { get; set; }
		[field: SerializeField] public bool can_link_external_content { get; set; }

		public ViewComponent(JsonValue body) {
			this.viewer_url = body[TwitchWords.VIEWER_URL].AsString;
			this.aspect_width = body[TwitchWords.ASPECT_WIDTH].AsInteger;
			this.aspect_height = body[TwitchWords.ASPECT_HEIGHT].AsInteger;
			this.aspect_ratio_x = body[TwitchWords.ASPECT_RATIO_X].AsInteger;
			this.aspect_ratio_y = body[TwitchWords.ASPECT_RATIO_Y].AsInteger;
			this.autoscale = body[TwitchWords.AUTOSCALE].AsBoolean;
			this.scale_pixels = body[TwitchWords.SCALE_PIXELS].AsInteger;
			this.target_height = body[TwitchWords.TARGET_HEIGHT].AsInteger;
			this.size = body[TwitchWords.SIZE].AsInteger;
			this.zoom = body[TwitchWords.ZOOM].AsBoolean;
			this.zoom_pixels = body[TwitchWords.ZOOM_PIXELS].AsInteger;
			this.can_link_external_content = body[TwitchWords.CAN_LINK_EXTERNAL_CONTENT].AsBoolean;
		}
	}
}
