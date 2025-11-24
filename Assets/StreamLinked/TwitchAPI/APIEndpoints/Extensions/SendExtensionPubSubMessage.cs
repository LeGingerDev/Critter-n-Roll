using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-extension-pubsub-message">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SendExtensionPubSubMessage : IExtensions, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.SendExtensionPubSubMessage;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SendExtensionPubSubMessage;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string[] target,
												string broadcaster_id,
												string message,
												bool? is_global_broadcast = null) {
			JsonObject body = new JsonObject() {
					{ TwitchWords.TARGET, new JsonArray(target) },
					{ TwitchWords.BROADCASTER_ID, broadcaster_id },
					{ TwitchWords.MESSAGE, message },
				};
			if (is_global_broadcast.HasValue) {
				body.Add(TwitchWords.IS_GLOBAL_BROADCAST, is_global_broadcast.Value);
			}
			return JsonWriter.Serialize(body);
		}

		public readonly void Initialise(JsonValue value) { }
	}
}