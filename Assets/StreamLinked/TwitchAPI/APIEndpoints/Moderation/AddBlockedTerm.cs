using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#add-blocked-term">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct AddBlockedTerm : IModeration, IJsonRequest {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string text { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string updated_at { get; set; }
		[field: SerializeField] public string expires_at { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.id = body[TwitchWords.ID].AsString;
			this.text = body[TwitchWords.TEXT].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.updated_at = body[TwitchWords.UPDATED_AT].AsString;
			this.expires_at = body[TwitchWords.EXPIRES_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.BlockedTerms;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.AddBlockedTerm;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_blocked_terms,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string text) {
			JsonObject body = new JsonObject() {
					{TwitchWords.TEXT, text},
				};
			return JsonWriter.Serialize(body);
		}
	}
}