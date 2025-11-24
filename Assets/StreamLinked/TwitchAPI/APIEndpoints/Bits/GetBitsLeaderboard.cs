using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Bits {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-bits-leaderboard">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetBitsLeaderboard : IBits {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public int rank { get; set; }
		[field: SerializeField] public int score { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.rank = body[TwitchWords.RANK].AsInteger;
			this.score = body[TwitchWords.SCORE].AsInteger;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetBitsLeaderboard;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetBitsLeaderboard;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.bits_read,
		};

		public static string COUNT => TwitchWords.COUNT;
		public static string PERIOD => TwitchWords.PERIOD;
		public static string STARTED_AT => TwitchWords.STARTED_AT;
	}
}