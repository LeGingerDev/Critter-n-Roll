using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using UnityEngine;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#warn-chat-user">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct WarnChatUser : IModeration, IJsonRequest {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public string reason { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.reason = body[TwitchWords.REASON].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.WarnUser;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.WarnChatUser;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_warnings,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string user_id, string reason) {
			JsonObject body = new JsonObject();
			JsonObject data = new JsonObject() {
				{ TwitchWords.USER_ID, user_id },
				{ TwitchWords.REASON, reason }
			};
			body.Add(TwitchWords.DATA, data);
			return JsonWriter.Serialize(body);
		}
	}
}