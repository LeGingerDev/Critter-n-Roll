using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Channels {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-editors">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChannelEditors : IChannels {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string created_at { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetChannelEditors;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChannelEditors;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_editors,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}