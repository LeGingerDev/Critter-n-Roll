using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Subscriptions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#check-user-subscription">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CheckUserSubscription : ISubscriptions {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string gifter_id { get; set; }
		[field: SerializeField] public string gifter_login { get; set; }
		[field: SerializeField] public string gifter_name { get; set; }
		[field: SerializeField] public bool is_gift { get; set; }
		[field: SerializeField] public string tier { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.gifter_id = body[TwitchWords.GIFTER_ID].AsString;
			this.gifter_login = body[TwitchWords.GIFTER_LOGIN].AsString;
			this.gifter_name = body[TwitchWords.GIFTER_NAME].AsString;
			this.is_gift = body[TwitchWords.IS_GIFT].AsBoolean;
			this.tier = body[TwitchWords.TIER].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.CheckUserSubscription;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CheckUserSubscription;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_subscriptions,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string USER_ID => TwitchWords.USER_ID;
	}
}