using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-guest-star-session">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateGuestStarSession : IGuest_Star {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public Guest[] guests { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.guests = body[TwitchWords.GUESTS].AsJsonArray?.ToModelArray<Guest>();
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.GuestStarSession;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateGuestStarSession;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}