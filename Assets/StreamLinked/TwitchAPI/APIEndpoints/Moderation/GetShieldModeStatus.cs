using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-shield-mode-status">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetShieldModeStatus : IModeration {

		[field: SerializeField] public bool is_active { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public string moderator_name { get; set; }
		[field: SerializeField] public string moderator_login { get; set; }
		[field: SerializeField] public string last_activated_at { get; set; }

		public void Initialise(JsonValue body) {
			this.is_active = body[TwitchWords.IS_ACTIVE].AsBoolean;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.moderator_name = body[TwitchWords.MODERATOR_NAME].AsString;
			this.moderator_login = body[TwitchWords.MODERATOR_LOGIN].AsString;
			this.last_activated_at = body[TwitchWords.LAST_ACTIVATED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.ShieldModeStatus;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetShieldModeStatus;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_shield_mode,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
	}
}