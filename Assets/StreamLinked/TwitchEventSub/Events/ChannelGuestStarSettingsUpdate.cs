using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_settingsupdate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelGuestStarSettingsUpdate : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public bool is_moderator_send_live_enabled { get; set; }
		[field: SerializeField] public int slot_count { get; set; }
		[field: SerializeField] public bool is_browser_source_audio_enabled { get; set; }
		[field: SerializeField] public string group_layout { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Settings_Update;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_guest_star,
			TwitchScopesEnum.channel_manage_guest_star,
			TwitchScopesEnum.moderator_read_guest_star,
			TwitchScopesEnum.moderator_manage_guest_star,
		};

		public ChannelGuestStarSettingsUpdate(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.is_moderator_send_live_enabled = body[TwitchWords.IS_MODERATOR_SEND_LIVE_ENABLED].AsBoolean;
			this.slot_count = body[TwitchWords.SLOT_COUNT].AsInteger;
			this.is_browser_source_audio_enabled = body[TwitchWords.IS_BROWSER_SOURCE_AUDIO_ENABLED].AsBoolean;
			this.group_layout = body[TwitchWords.GROUP_LAYOUT].AsString;
		}
	}
}