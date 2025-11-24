using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#remove-channel-moderator">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct RemoveChannelModerator : IModeration, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.Moderators;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.RemoveChannelModerator;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_moderators,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string USER_ID => TwitchWords.USER_ID;

		public readonly void Initialise(JsonValue value) { }
	}
}