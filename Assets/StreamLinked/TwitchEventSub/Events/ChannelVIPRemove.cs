using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipremove">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelVIPRemove : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_VIP_Remove;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_vips,
			TwitchScopesEnum.channel_manage_vips,
		};

		public ChannelVIPRemove(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
		}
	}
}