using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-extension-chat-message">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SendExtensionChatMessage : IExtensions, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.SendExtensionChatMessage;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SendExtensionChatMessage;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string text,
												string extension_id,
												string extension_version) {
			JsonObject body = new JsonObject() {
					{ TwitchWords.TEXT, text },
					{ TwitchWords.EXTENSION_ID, extension_id },
					{ TwitchWords.EXTENSION_VERSION, extension_version },
				};
			return JsonWriter.Serialize(body);
		}

		public readonly void Initialise(JsonValue value) { }
	}
}