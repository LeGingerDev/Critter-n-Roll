using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Whispers {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-whisper">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SendWhisper : IWhispers, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.SendWhisper;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SendWhisper;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_manage_whispers,
		};

		public static string FROM_USER_ID => TwitchWords.FROM_USER_ID;
		public static string TO_USER_ID => TwitchWords.TO_USER_ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(string message) {
			JsonObject body = new JsonObject() {
					{TwitchWords.MESSAGE, message},
				};
			return JsonWriter.Serialize(body);
		}

		public readonly void Initialise(JsonValue value) { }
	}
}