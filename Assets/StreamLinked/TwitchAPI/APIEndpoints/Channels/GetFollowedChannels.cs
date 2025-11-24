using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Channels {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-followed-channels">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetFollowedChannels : IChannels {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string followed_at { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.followed_at = body[TwitchWords.FOLLOWED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetFollowedChannels;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetFollowedChannels;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_follows,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}