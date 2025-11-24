using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using UnityEngine;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-automod-settings">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateAutoModSettings : IModeration, IJsonRequest {

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

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PUT;
		public readonly string Endpoint => TwitchAPILinks.AutoModSettings;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateAutoModSettings;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_automod_settings,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(int? aggression = null,
												int? bullying = null,
												int? disability = null,
												int? misogyny = null,
												int? overall_level = null,
												int? race_ethnicity_or_religion = null,
												int? sex_based_terms = null,
												int? sexuality_sex_or_gender = null,
												int? swearing = null) {
			JsonObject body = new JsonObject();
			if (aggression.HasValue) {
				body.Add(TwitchWords.AGGRESSION, aggression);
			}
			if (bullying.HasValue) {
				body.Add(TwitchWords.BULLYING, bullying);
			}
			if (disability.HasValue) {
				body.Add(TwitchWords.DISABILITY, disability);
			}
			if (misogyny.HasValue) {
				body.Add(TwitchWords.MISOGYNY, misogyny);
			}
			if (overall_level.HasValue) {
				body.Add(TwitchWords.OVERALL_LEVEL, overall_level);
			}
			if (race_ethnicity_or_religion.HasValue) {
				body.Add(TwitchWords.RACE_ETHNICITY_OR_RELIGION, race_ethnicity_or_religion);
			}
			if (sex_based_terms.HasValue) {
				body.Add(TwitchWords.SEX_BASED_TERMS, sex_based_terms);
			}
			if (sexuality_sex_or_gender.HasValue) {
				body.Add(TwitchWords.SEXUALITY_SEX_OR_GENDER, sexuality_sex_or_gender);
			}
			if (swearing.HasValue) {
				body.Add(TwitchWords.SWEARING, swearing);
			}
			return body.Count > 0 ? JsonWriter.Serialize(body) : string.Empty;
		}
	}
}