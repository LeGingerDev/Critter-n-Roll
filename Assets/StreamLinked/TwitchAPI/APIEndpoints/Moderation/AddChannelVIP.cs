using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#add-channel-vip">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct AddChannelVIP : IModeration, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.VIPs;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.AddChannelVIP;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_vips,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;

		public readonly void Initialise(JsonValue value) { }
	}
}