using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelbitsuse">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelBitsUse : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public int bits { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public Message message { get; set; }
		[field: SerializeField] public PowerUp power_up { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Bits_Use;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.bits_read
		};

		public ChannelBitsUse(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.bits = body[TwitchWords.BITS].AsInteger;
			this.type = body[TwitchWords.TYPE].AsString;
			this.message = new Message(body[TwitchWords.MESSAGE]);
			this.power_up = new PowerUp(body[TwitchWords.POWER_UP]);
		}
	}
}