using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-moderated-channels">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetModeratedChannels : IModeration {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetModeratedChannels;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetModeratedChannels;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_moderated_channels,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string AFTER => TwitchWords.AFTER;
		public static string FIRST => TwitchWords.FIRST;
	}
}