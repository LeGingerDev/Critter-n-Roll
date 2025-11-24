using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelad_breakbegin">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelAdBreakBegin : IChannel {

		[field: SerializeField] public int duration_seconds { get; set; }
		[field: SerializeField] public string timestamp { get; set; }
		[field: SerializeField] public bool is_automatic { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string requester_user_id { get; set; }
		[field: SerializeField] public string requester_user_login { get; set; }
		[field: SerializeField] public string requester_user_name { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Ad_Break_Begin;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_ads,
		};

		public ChannelAdBreakBegin(JsonValue body) {
			this.duration_seconds = body[TwitchWords.DURATION_SECONDS].AsInteger;
			this.timestamp = body[TwitchWords.TIMESTAMP].AsString;
			this.is_automatic = body[TwitchWords.IS_AUTOMATIC].AsBoolean;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.requester_user_id = body[TwitchWords.REQUESTER_USER_ID].AsString;
			this.requester_user_login = body[TwitchWords.REQUESTER_USER_LOGIN].AsString;
			this.requester_user_name = body[TwitchWords.REQUESTER_USER_NAME].AsString;
		}
	}
}