using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Charity {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-charity-campaign-donations">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetCharityCampaignDonations : ICharity {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string campaign_id { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public Amount amount { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.campaign_id = body[TwitchWords.CAMPAIGN_ID].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.amount = new Amount(body[TwitchWords.AMOUNT]);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetCharityCampaignDonations;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetCharityCampaignDonations;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_charity,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}