using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#unban-user">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UnbanUser : IModeration, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.Users;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UnbanUser;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_banned_users,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string USER_ID => TwitchWords.USER_ID;

		public readonly void Initialise(JsonValue body) { }
	}
}