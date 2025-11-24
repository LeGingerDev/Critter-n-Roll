using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-guest-star-invites">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetGuestStarInvites : IGuest_Star {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string invited_at { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public bool is_audio_enabled { get; set; }
		[field: SerializeField] public bool is_video_enabled { get; set; }
		[field: SerializeField] public bool is_audio_available { get; set; }
		[field: SerializeField] public bool is_video_available { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.invited_at = body[TwitchWords.INVITED_AT].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			this.is_audio_enabled = body[TwitchWords.IS_AUDIO_ENABLED].AsBoolean;
			this.is_video_enabled = body[TwitchWords.IS_VIDEO_ENABLED].AsBoolean;
			this.is_audio_available = body[TwitchWords.IS_AUDIO_AVAILABLE].AsBoolean;
			this.is_video_available = body[TwitchWords.IS_VIDEO_AVAILABLE].AsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GuestStarInvites;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetGuestStarInvites;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_guest_star,
			TwitchScopesEnum.moderator_read_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string SESSION_ID => TwitchWords.SESSION_ID;
	}
}