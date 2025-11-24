using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Goals {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-creator-goals">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetCreatorGoals : IGoals {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public int current_amount { get; set; }
		[field: SerializeField] public int target_amount { get; set; }
		[field: SerializeField] public string created_at { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.current_amount = body[TwitchWords.CURRENT_AMOUNT].AsInteger;
			this.target_amount = body[TwitchWords.TARGET_AMOUNT].AsInteger;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetCreatorGoals;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetCreatorGoals;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_goals,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}