using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-announcement">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SendChatAnnouncement : IChat, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.SendChatAnnouncement;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SendChatAnnouncement;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_announcements,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(string message,
												string color = null) {
			JsonObject body = new JsonObject() {
					{ TwitchWords.MESSAGE, message }
				};
			if (!string.IsNullOrEmpty(color)) {
				body.Add(TwitchWords.COLOR, color);
			}
			return JsonWriter.Serialize(body);
		}

		public readonly void Initialise(JsonValue value) { }
	}
}