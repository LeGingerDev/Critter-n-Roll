using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Guest_Star {
	/// <summary>
	/// <c>BETA</c>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-guest-star-settings">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChannelGuestStarSettings : IGuest_Star {

		[field: SerializeField] public bool is_moderator_send_live_enabled { get; set; }
		[field: SerializeField] public int slot_count { get; set; }
		[field: SerializeField] public bool is_browser_source_audio_enabled { get; set; }
		[field: SerializeField] public string layout { get; set; }
		[field: SerializeField] public string browser_source_token { get; set; }

		public void Initialise(JsonValue body) {
			this.is_moderator_send_live_enabled = body[TwitchWords.IS_MODERATOR_SEND_LIVE_ENABLED].AsBoolean;
			this.slot_count = body[TwitchWords.SLOT_COUNT].AsInteger;
			this.is_browser_source_audio_enabled = body[TwitchWords.IS_BROWSER_SOURCE_AUDIO_ENABLED].AsBoolean;
			this.layout = body[TwitchWords.LAYOUT].AsString;
			this.browser_source_token = body[TwitchWords.BROWSER_SOURCE_TOKEN].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.ChannelGuestStarSettings;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChannelGuestStarSettings;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_guest_star,
			TwitchScopesEnum.moderator_read_guest_star,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
	}
}