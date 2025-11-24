using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-blocked-terms">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetBlockedTerms : IModeration {

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

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.BlockedTerms;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetBlockedTerms;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_blocked_terms,
			TwitchScopesEnum.moderator_manage_blocked_terms,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}