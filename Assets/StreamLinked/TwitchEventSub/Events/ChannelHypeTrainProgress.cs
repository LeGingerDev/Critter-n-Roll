using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainprogress">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelHypeTrainProgress : IHypeTrain {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public int level { get; set; }
		[field: SerializeField] public int total { get; set; }
		[field: SerializeField] public TopContributions top_contributions { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string ended_at { get; set; }
		[field: SerializeField] public string cooldown_ends_at { get; set; }
		[field: SerializeField] public bool is_golden_kappa_train { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Hype_Train_Progress;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_hype_train,
		};

		public ChannelHypeTrainProgress(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.total = body[TwitchWords.TOTAL].AsInteger;
			this.top_contributions = new TopContributions(body[TwitchWords.TOP_CONTRIBUTIONS]);
			this.level = body[TwitchWords.LEVEL].AsInteger;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.ended_at = body[TwitchWords.ENDED_AT].AsString;
			this.cooldown_ends_at = body[TwitchWords.COOLDOWN_ENDS_AT].AsString;
			this.is_golden_kappa_train = body[TwitchWords.IS_GOLDEN_KAPPA_TRAIN].AsBoolean;
		}
	}
}