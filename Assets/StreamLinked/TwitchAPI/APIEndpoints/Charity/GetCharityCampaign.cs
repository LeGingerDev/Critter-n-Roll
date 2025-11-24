using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Charity {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-charity-campaign">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetCharityCampaign : ICharity {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string charity_name { get; set; }
		[field: SerializeField] public string charity_description { get; set; }
		[field: SerializeField] public string charity_logo { get; set; }
		[field: SerializeField] public string charity_website { get; set; }
		[field: SerializeField] public Amount current_amount { get; set; }
		[field: SerializeField] public Amount target_amount { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.charity_name = body[TwitchWords.CHARITY_NAME].AsString;
			this.charity_description = body[TwitchWords.CHARITY_DESCRIPTION].AsString;
			this.charity_logo = body[TwitchWords.CHARITY_LOGO].AsString;
			this.charity_website = body[TwitchWords.CHARITY_WEBSITE].AsString;
			this.current_amount = new Amount(body[TwitchWords.CURRENT_AMOUNT]);
			this.target_amount = new Amount(body[TwitchWords.TARGET_AMOUNT]);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetCharityCampaign;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetCharityCampaign;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_charity,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}