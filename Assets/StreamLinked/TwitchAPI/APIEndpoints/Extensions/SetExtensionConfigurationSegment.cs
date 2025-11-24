using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#set-extension-configuration-segment">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SetExtensionConfigurationSegment : IExtensions, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PUT;
		public readonly string Endpoint => TwitchAPILinks.ExtensionConfigurationSegment;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SetExtensionConfigurationSegment;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string extension_id,
												string segment,
												string broadcaster_id = null,
												string content = null,
												string version = null) {
			JsonObject body = new JsonObject() {
					{ TwitchWords.EXTENSION_ID, extension_id },
					{ TwitchWords.SEGMENT, segment }
				};
			if (!string.IsNullOrEmpty(broadcaster_id)) {
				body.Add(TwitchWords.BROADCASTER_ID, broadcaster_id);
			}
			if (!string.IsNullOrEmpty(content)) {
				body.Add(TwitchWords.CONTENT, content);
			}
			if (!string.IsNullOrEmpty(version)) {
				body.Add(TwitchWords.VERSION, version);
			}
			return JsonWriter.Serialize(body);
		}

		public readonly void Initialise(JsonValue value) { }
	}
}