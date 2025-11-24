using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Ads {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-ad-schedule">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SnoozeNextAd : IAds {

		[field: SerializeField] public int snooze_count { get; set; }
		[field: SerializeField] public string snooze_refresh_at { get; set; }
		[field: SerializeField] public string next_ad_at { get; set; }

		public void Initialise(JsonValue body) {
			this.snooze_count = body[TwitchWords.SNOOZE_COUNT].AsInteger;
			this.snooze_refresh_at = body[TwitchWords.SNOOZE_REFRESH_AT].AsString;
			this.next_ad_at = body[TwitchWords.NEXT_AD_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.SnoozeNextAd;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SnoozeNextAd;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_ads,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}
