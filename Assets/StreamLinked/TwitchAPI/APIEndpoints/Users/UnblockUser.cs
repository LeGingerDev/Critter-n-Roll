using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Users {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#unblock-user">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UnblockUser : IUsers, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.UserBlockList;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UnblockUser;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_manage_blocked_users,
		};

		public static string TARGET_USER_ID => TwitchWords.TARGET_USER_ID;

		public readonly void Initialise(JsonValue value) { }
	}
}