using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modebegin">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelShieldModeBegin : IShield {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string moderator_user_id { get; set; }
		[field: SerializeField] public string moderator_user_login { get; set; }
		[field: SerializeField] public string moderator_user_name { get; set; }
		[field: SerializeField] public string started_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Shield_Mode_Begin;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_shield_mode,
			TwitchScopesEnum.moderator_manage_shield_mode,
		};

		public ChannelShieldModeBegin(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.moderator_user_id = body[TwitchWords.MODERATOR_USER_ID].AsString;
			this.moderator_user_login = body[TwitchWords.MODERATOR_USER_LOGIN].AsString;
			this.moderator_user_name = body[TwitchWords.MODERATOR_USER_NAME].AsString;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
		}
	}
}