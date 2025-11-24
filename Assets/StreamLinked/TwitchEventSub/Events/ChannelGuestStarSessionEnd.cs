using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionend">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelGuestStarSessionEnd : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string session_id { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string ended_at { get; set; }
		[field: SerializeField] public string host_user_id { get; set; }
		[field: SerializeField] public string host_user_name { get; set; }
		[field: SerializeField] public string host_user_login { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_End;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_guest_star,
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_read_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public ChannelGuestStarSessionEnd(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.session_id = body[TwitchWords.SESSION_ID].AsString;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.ended_at = body[TwitchWords.ENDED_AT].AsString;
			this.host_user_id = body[TwitchWords.HOST_USER_ID].AsString;
			this.host_user_name = body[TwitchWords.HOST_USER_NAME].AsString;
			this.host_user_login = body[TwitchWords.HOST_USER_LOGIN].AsString;
		}
	}
}