using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Channels {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-followers">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChannelFollowers : IChannels {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string followed_at { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.followed_at = body[TwitchWords.FOLLOWED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetChannelFollowers;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChannelFollowers;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_followers,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}