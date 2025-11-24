using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignstart">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelCharityCampaignStart : ICharityCampaign {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string charity_name { get; set; }
		[field: SerializeField] public string charity_description { get; set; }
		[field: SerializeField] public string charity_logo { get; set; }
		[field: SerializeField] public string charity_website { get; set; }
		[field: SerializeField] public Amount current_amount { get; set; }
		[field: SerializeField] public Amount target_amount { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Charity_Campaign_Start;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_charity,
		};

		public ChannelCharityCampaignStart(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.charity_name = body[TwitchWords.CHARITY_NAME].AsString;
			this.charity_description = body[TwitchWords.CHARITY_DESCRIPTION].AsString;
			this.charity_logo = body[TwitchWords.CHARITY_LOGO].AsString;
			this.charity_website = body[TwitchWords.CHARITY_WEBSITE].AsString;
			this.current_amount = new Amount(body[TwitchWords.CURRENT_AMOUNT]);
			this.target_amount = new Amount(body[TwitchWords.TARGET_AMOUNT]);

		}
	}
}