using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-user-chat-color">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateUserChatColor : IChat, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.UserChatColor;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateUserChatColor;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_manage_chat_color,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string COLOR => TwitchWords.COLOR;

		public readonly void Initialise(JsonValue value) { }
	}
}