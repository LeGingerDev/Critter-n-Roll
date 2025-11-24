using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {

	[Serializable]
	public struct Guest : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_display_name { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public bool is_live { get; set; }
		[field: SerializeField] public int volume { get; set; }
		[field: SerializeField] public string assigned_at { get; set; }
		[field: SerializeField] public AudioSettings audio_settings { get; set; }
		[field: SerializeField] public VideoSettings video_settings { get; set; }

		public Guest(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_display_name = body[TwitchWords.USER_DISPLAY_NAME].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.is_live = body[TwitchWords.IS_LIVE].AsBoolean;
			this.volume = body[TwitchWords.VOLUME].AsInteger;
			this.assigned_at = body[TwitchWords.ASSIGNED_AT].AsString;
			this.audio_settings = new AudioSettings(body[TwitchWords.AUDIO_SETTINGS]);
			this.video_settings = new VideoSettings(body[TwitchWords.VIDEO_SETTINGS]);
		}
	}
}