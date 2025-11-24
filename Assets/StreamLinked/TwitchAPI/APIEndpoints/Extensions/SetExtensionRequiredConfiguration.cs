using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#set-extension-required-configuration">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SetExtensionRequiredConfiguration : IExtensions, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PUT;
		public readonly string Endpoint => TwitchAPILinks.SetExtensionRequiredConfiguration;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SetExtensionRequiredConfiguration;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string extension_id,
												string extension_version,
												string required_configuration) {
			JsonObject body = new JsonObject() {
					{ TwitchWords.EXTENSION_ID, extension_id },
					{ TwitchWords.EXTENSION_VERSION, extension_version },
					{ TwitchWords.REQUIRED_CONFIGURATION, required_configuration }
				};
			return JsonWriter.Serialize(body);
		}

		public readonly void Initialise(JsonValue value) { }
	}
}