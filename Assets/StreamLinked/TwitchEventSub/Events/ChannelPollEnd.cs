using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelPollEnd : IPoll {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public Choices[] choices { get; set; }
		[field: SerializeField] public BitsVoting bits_voting { get; set; }
		[field: SerializeField] public ChannelPointsVoting channel_points_voting { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string ends_at { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Poll_End;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_polls,
			TwitchScopesEnum.channel_manage_polls,
		};

		public ChannelPollEnd(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.choices = body[TwitchWords.CHOICES].AsJsonArray?.ToModelArray<Choices>();
			this.bits_voting = new BitsVoting(body[TwitchWords.BITS_VOTING]);
			this.channel_points_voting = new ChannelPointsVoting(body[TwitchWords.CHANNEL_POINTS_VOTING]);
			this.status = body[TwitchWords.STATUS].AsString;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.ends_at = body[TwitchWords.ENDS_AT].AsString;
		}
	}
}