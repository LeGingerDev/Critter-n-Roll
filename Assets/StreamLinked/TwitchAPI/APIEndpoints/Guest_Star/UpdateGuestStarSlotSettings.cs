using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-guest-star-slot-settings">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateGuestStarSlotSettings : IGuest_Star, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.UpdateGuestStarSlotSettings;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateGuestStarSlotSettings;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string SESSION_ID => TwitchWords.SESSION_ID;
		public static string SLOT_ID => TwitchWords.SLOT_ID;
		public static string IS_AUDIO_ENABLED => TwitchWords.IS_AUDIO_ENABLED;
		public static string IS_VIDEO_ENABLED => TwitchWords.IS_VIDEO_ENABLED;
		public static string IS_LIVE => TwitchWords.IS_LIVE;
		public static string VOLUME => TwitchWords.VOLUME;

		public readonly void Initialise(JsonValue value) { }
	}
}