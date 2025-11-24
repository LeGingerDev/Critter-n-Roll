using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_guestupdate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelGuestStarGuestUpdate : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string session_id { get; set; }
		[field: SerializeField] public string moderator_user_id { get; set; }
		[field: SerializeField] public string moderator_user_login { get; set; }
		[field: SerializeField] public string moderator_user_name { get; set; }
		[field: SerializeField] public string guest_user_id { get; set; }
		[field: SerializeField] public string guest_user_name { get; set; }
		[field: SerializeField] public string guest_user_login { get; set; }
		[field: SerializeField] public string slot_id { get; set; }
		[field: SerializeField] public string state { get; set; }
		[field: SerializeField] public string host_user_id { get; set; }
		[field: SerializeField] public string host_user_name { get; set; }
		[field: SerializeField] public string host_user_login { get; set; }
		[field: SerializeField] public string host_video_enabled { get; set; }
		[field: SerializeField] public string host_audio_enabled { get; set; }
		[field: SerializeField] public string host_volume { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Guest_Update;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_guest_star,
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_read_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public ChannelGuestStarGuestUpdate(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.session_id = body[TwitchWords.SESSION_ID].AsString;
			this.moderator_user_id = body[TwitchWords.MODERATOR_USER_ID].AsString;
			this.moderator_user_login = body[TwitchWords.MODERATOR_USER_LOGIN].AsString;
			this.moderator_user_name = body[TwitchWords.MODERATOR_USER_NAME].AsString;
			this.guest_user_id = body[TwitchWords.GUEST_USER_ID].AsString;
			this.guest_user_name = body[TwitchWords.GUEST_USER_NAME].AsString;
			this.guest_user_login = body[TwitchWords.GUEST_USER_LOGIN].AsString;
			this.slot_id = body[TwitchWords.SLOT_ID].AsString;
			this.state = body[TwitchWords.STATE].AsString;
			this.host_user_id = body[TwitchWords.HOST_USER_ID].AsString;
			this.host_user_name = body[TwitchWords.HOST_USER_NAME].AsString;
			this.host_user_login = body[TwitchWords.HOST_USER_LOGIN].AsString;
			this.host_video_enabled = body[TwitchWords.HOST_VIDEO_ENABLED].AsString;
			this.host_audio_enabled = body[TwitchWords.HOST_AUDIO_ENABLED].AsString;
			this.host_volume = body[TwitchWords.HOST_VOLUME].AsString;
		}
	}
}