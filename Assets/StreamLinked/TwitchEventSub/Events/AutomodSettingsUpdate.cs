using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodsettingsupdate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct AutomodSettingsUpdate : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string moderator_user_id { get; set; }
		[field: SerializeField] public string moderator_user_login { get; set; }
		[field: SerializeField] public string moderator_user_name { get; set; }
		[field: SerializeField] public int bullying { get; set; }
		[field: SerializeField] public int overall_level { get; set; }
		[field: SerializeField] public int disability { get; set; }
		[field: SerializeField] public int race_ethnicity_or_religion { get; set; }
		[field: SerializeField] public int misogyny { get; set; }
		[field: SerializeField] public int sexuality_sex_or_gender { get; set; }
		[field: SerializeField] public int aggression { get; set; }
		[field: SerializeField] public int sex_based_terms { get; set; }
		[field: SerializeField] public int swearing { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Automod_Settings_Update;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_automod_settings,
		};

		public AutomodSettingsUpdate(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.moderator_user_id = body[TwitchWords.MODERATOR_USER_ID].AsString;
			this.moderator_user_login = body[TwitchWords.MODERATOR_USER_LOGIN].AsString;
			this.moderator_user_name = body[TwitchWords.MODERATOR_USER_NAME].AsString;
			this.bullying = body[TwitchWords.BULLYING].AsInteger;
			this.overall_level = body[TwitchWords.OVERALL_LEVEL].AsInteger;
			this.disability = body[TwitchWords.DISABILITY].AsInteger;
			this.race_ethnicity_or_religion = body[TwitchWords.RACE_ETHNICITY_OR_RELIGION].AsInteger;
			this.misogyny = body[TwitchWords.MISOGYNY].AsInteger;
			this.sexuality_sex_or_gender = body[TwitchWords.SEXUALITY_SEX_OR_GENDER].AsInteger;
			this.aggression = body[TwitchWords.AGGRESSION].AsInteger;
			this.sex_based_terms = body[TwitchWords.SEX_BASED_TERMS].AsInteger;
			this.swearing = body[TwitchWords.SWEARING].AsInteger;
		}
	}
}