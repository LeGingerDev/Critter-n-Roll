using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Subscriptions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-broadcaster-subscriptions">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetBroadcasterSubscriptions : ISubscriptions {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string gifter_id { get; set; }
		[field: SerializeField] public string gifter_login { get; set; }
		[field: SerializeField] public string gifter_name { get; set; }
		[field: SerializeField] public bool is_gift { get; set; }
		[field: SerializeField] public string tier { get; set; }
		[field: SerializeField] public string plan_name { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string user_login { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.gifter_id = body[TwitchWords.GIFTER_ID].AsString;
			this.gifter_login = body[TwitchWords.GIFTER_LOGIN].AsString;
			this.gifter_name = body[TwitchWords.GIFTER_NAME].AsString;
			this.is_gift = body[TwitchWords.IS_GIFT].AsBoolean;
			this.tier = body[TwitchWords.TIER].AsString;
			this.plan_name = body[TwitchWords.PLAN_NAME].AsString;
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetBroadcasterSubscriptions;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetBroadcasterSubscriptions;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_subscriptions,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string USER_ID => TwitchWords.USER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
		public static string BEFORE => TwitchWords.BEFORE;
	}
}