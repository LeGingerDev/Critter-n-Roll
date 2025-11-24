using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Raids {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#start-a-raid">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct StartARaid : IRaids {

		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public bool is_mature { get; set; }

		public void Initialise(JsonValue body) {
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.is_mature = body[TwitchWords.IS_MATURE].AsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.Raids;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.StartARaid;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_raids,
		};

		public static string FROM_BROADCASTER_ID => TwitchWords.FROM_BROADCASTER_ID;
		public static string TO_BROADCASTER_ID => TwitchWords.TO_BROADCASTER_ID;
	}
}