using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Users {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#block-user">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct BlockUser : IUsers, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PUT;
		public readonly string Endpoint => TwitchAPILinks.Users;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.BlockUser;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_manage_blocked_users,
		};

		public static string TARGET_USER_ID => TwitchWords.TARGET_USER_ID;
		public static string SOURCE_CONTEXT => TwitchWords.SOURCE_CONTEXT;
		public static string REASON => TwitchWords.REASON;

		public readonly void Initialise(JsonValue value) { }
	}
}