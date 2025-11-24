using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Channel_Points {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-custom-reward-redemption">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetCustomRewardRedemption : IChannel_Points {

		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string user_input { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public string redeemed_at { get; set; }
		[field: SerializeField] public Reward reward { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.id = body[TwitchWords.ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.user_input = body[TwitchWords.USER_INPUT].AsString;
			this.status = body[TwitchWords.STATUS].AsString;
			this.redeemed_at = body[TwitchWords.REDEEMED_AT].AsString;
			this.reward = new Reward(body[TwitchWords.REWARD]);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.CustomRewardRedemption;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetCustomRewardRedemption;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_redemptions,
			TwitchScopesEnum.channel_manage_redemptions,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string REWARD_ID => TwitchWords.REWARD_ID;
		public static string STATUS => TwitchWords.STATUS;
		public static string ID => TwitchWords.ID;
	}
}