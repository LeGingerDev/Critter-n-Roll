using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-guest-star-slot">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct DeleteGuestStarSlot : IGuest_Star, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.GuestStarSlot;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.DeleteGuestStarSlot;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string SESSION_ID => TwitchWords.SESSION_ID;
		public static string GUEST_ID => TwitchWords.GUEST_ID;
		public static string SLOT_ID => TwitchWords.SLOT_ID;
		public static string SHOULD_REINVITE_GUEST => TwitchWords.SHOULD_REINVITE_GUEST;

		public readonly void Initialise(JsonValue value) { }
	}
}