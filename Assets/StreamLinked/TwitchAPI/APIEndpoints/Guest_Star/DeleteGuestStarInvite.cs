using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-guest-star-invite">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct DeleteGuestStarInvite : IGuest_Star, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.GuestStarInvites;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.DeleteGuestStarInvite;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string SESSION_ID => TwitchWords.SESSION_ID;
		public static string GUEST_ID => TwitchWords.GUEST_ID;

		public readonly void Initialise(JsonValue value) { }
	}
}