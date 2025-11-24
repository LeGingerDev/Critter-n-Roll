using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-guest-star-slot">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateGuestStarSlot : IGuest_Star, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.GuestStarSlot;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateGuestStarSlot;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string SESSION_ID => TwitchWords.SESSION_ID;
		public static string SOURCE_SLOT_ID => TwitchWords.SOURCE_SLOT_ID;
		public static string DESTINATION_SLOT_ID => TwitchWords.DESTINATION_SLOT_ID;

		public readonly void Initialise(JsonValue value) { }
	}
}