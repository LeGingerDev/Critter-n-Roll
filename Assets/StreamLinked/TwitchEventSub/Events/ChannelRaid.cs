using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelraid">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelRaid : IEvent {

		[field: SerializeField] public string from_broadcaster_user_id { get; set; }
		[field: SerializeField] public string from_broadcaster_user_login { get; set; }
		[field: SerializeField] public string from_broadcaster_user_name { get; set; }
		[field: SerializeField] public string to_broadcaster_user_id { get; set; }
		[field: SerializeField] public string to_broadcaster_user_login { get; set; }
		[field: SerializeField] public string to_broadcaster_user_name { get; set; }
		[field: SerializeField] public int viewers { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Raid;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public ChannelRaid(JsonValue body) {
			this.from_broadcaster_user_id = body[TwitchWords.FROM_BROADCASTER_USER_ID].AsString;
			this.from_broadcaster_user_login = body[TwitchWords.FROM_BROADCASTER_USER_LOGIN].AsString;
			this.from_broadcaster_user_name = body[TwitchWords.FROM_BROADCASTER_USER_NAME].AsString;
			this.to_broadcaster_user_id = body[TwitchWords.TO_BROADCASTER_USER_ID].AsString;
			this.to_broadcaster_user_login = body[TwitchWords.TO_BROADCASTER_USER_LOGIN].AsString;
			this.to_broadcaster_user_name = body[TwitchWords.TO_BROADCASTER_USER_NAME].AsString;
			this.viewers = body[TwitchWords.VIEWERS].AsInteger;
		}
	}
}