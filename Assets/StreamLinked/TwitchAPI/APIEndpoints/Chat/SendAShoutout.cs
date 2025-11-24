using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-a-shoutout">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SendAShoutout : IChat, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.SendAShoutout;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SendAShoutout;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_shoutouts,
		};

		public static string FROM_BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string TO_BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;

		public readonly void Initialise(JsonValue value) { }
	}
}