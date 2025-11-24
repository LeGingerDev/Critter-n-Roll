using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalend">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelGoalsEnd : IGoals {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public bool is_achieved { get; set; }
		[field: SerializeField] public int current_amount { get; set; }
		[field: SerializeField] public int target_amount { get; set; }
		[field: SerializeField] public int started_at { get; set; }
		[field: SerializeField] public int ended_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Goal_End;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_goals,
		};

		public ChannelGoalsEnd(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.is_achieved = body[TwitchWords.IS_ACHIEVED].AsBoolean;
			this.current_amount = body[TwitchWords.CURRENT_AMOUNT].AsInteger;
			this.target_amount = body[TwitchWords.TARGET_AMOUNT].AsInteger;
			this.started_at = body[TwitchWords.STARTED_AT].AsInteger;
			this.ended_at = body[TwitchWords.ENDED_AT].AsInteger;
		}
	}
}