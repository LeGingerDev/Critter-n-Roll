using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Video : IShared {

		[field: SerializeField] public string video_id { get; set; }
		[field: SerializeField] public Marker[] markers { get; set; }

		public Video(JsonValue body) {
			this.video_id = body[TwitchWords.VIDEO_ID].AsString;
			this.markers = body[TwitchWords.MARKERS].AsJsonArray?.ToModelArray<Marker>();
		}
	}
}
