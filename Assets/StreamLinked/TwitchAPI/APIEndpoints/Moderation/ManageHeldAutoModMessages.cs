using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#manage-held-automod-messages">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct ManageHeldAutoMessages : IModeration, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.ManageHeldAutoMessages;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.ManageHeldAutoMessages;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_automod,
		};

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string user_id,
												string msg_id,
												string action) {
			JsonObject body = new JsonObject() {
					{TwitchWords.USER_ID, user_id},
					{TwitchWords.MSG_ID, msg_id},
					{TwitchWords.ACTION, action},
				};
			return JsonWriter.Serialize(body);
		}

		public readonly void Initialise(JsonValue value) { }
	}
}