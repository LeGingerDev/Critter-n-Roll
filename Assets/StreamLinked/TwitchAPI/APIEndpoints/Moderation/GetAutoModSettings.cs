using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-automod-settings">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetAutoModSettings : IModeration {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public int overall_level { get; set; }
		[field: SerializeField] public int disability { get; set; }
		[field: SerializeField] public int aggression { get; set; }
		[field: SerializeField] public int sexuality_sex_or_gender { get; set; }
		[field: SerializeField] public int misogyny { get; set; }
		[field: SerializeField] public int bullying { get; set; }
		[field: SerializeField] public int swearing { get; set; }
		[field: SerializeField] public int race_ethnicity_or_religion { get; set; }
		[field: SerializeField] public int sex_based_terms { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.overall_level = body[TwitchWords.OVERALL_LEVEL].AsInteger;
			this.disability = body[TwitchWords.DISABILITY].AsInteger;
			this.aggression = body[TwitchWords.AGGRESSION].AsInteger;
			this.sexuality_sex_or_gender = body[TwitchWords.SEXUALITY_SEX_OR_GENDER].AsInteger;
			this.misogyny = body[TwitchWords.MISOGYNY].AsInteger;
			this.bullying = body[TwitchWords.BULLYING].AsInteger;
			this.swearing = body[TwitchWords.SWEARING].AsInteger;
			this.race_ethnicity_or_religion = body[TwitchWords.RACE_ETHNICITY_OR_RELIGION].AsInteger;
			this.sex_based_terms = body[TwitchWords.SEX_BASED_TERMS].AsInteger;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.AutoModSettings;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetAutoModSettings;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_automod_settings,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
	}
}