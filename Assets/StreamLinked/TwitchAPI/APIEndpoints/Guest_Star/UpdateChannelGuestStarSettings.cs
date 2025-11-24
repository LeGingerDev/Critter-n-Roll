using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-channel-guest-star-settings">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateChannelGuestStarSettings : IGuest_Star, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PUT;
		public readonly string Endpoint => TwitchAPILinks.ChannelGuestStarSettings;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateChannelGuestStarSettings;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string IS_MODERATOR_SEND_LIVE_ENABLED => TwitchWords.IS_MODERATOR_SEND_LIVE_ENABLED;
		public static string SLOT_COUNT => TwitchWords.SLOT_COUNT;
		public static string IS_BROWSER_SOURCE_AUDIO_ENABLED => TwitchWords.IS_BROWSER_SOURCE_AUDIO_ENABLED;
		public static string GROUP_LAYOUT => TwitchWords.GROUP_LAYOUT;
		public static string REGENERATE_BROWSER_SOURCES => TwitchWords.REGENERATE_BROWSER_SOURCES;

		public readonly void Initialise(JsonValue value) { }
	}
}