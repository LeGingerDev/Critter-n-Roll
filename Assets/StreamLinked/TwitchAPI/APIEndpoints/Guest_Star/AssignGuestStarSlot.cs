using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#assign-guest-star-slot">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct AssignGuestStarSlot : IGuest_Star {

		[field: SerializeField] public string code { get; set; }

		public void Initialise(JsonValue body) {
			this.code = body[TwitchWords.CODE].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.GuestStarSlot;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.AssignGuestStarSlot;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string SESSION_ID => TwitchWords.SESSION_ID;
		public static string GUEST_ID => TwitchWords.GUEST_ID;
		public static string SLOT_ID => TwitchWords.SLOT_ID;
	}
}