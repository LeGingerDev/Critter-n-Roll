using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-chat-color">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetUserChatColor : IChat {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string color { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.color = body[TwitchWords.COLOR].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.UserChatColor;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetUserChatColor;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string USER_ID => TwitchWords.USER_ID;

	}
}