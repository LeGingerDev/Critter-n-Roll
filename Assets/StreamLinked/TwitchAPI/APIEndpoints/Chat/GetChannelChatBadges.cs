using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-chat-badges">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChannelChatBadges : IChat {

		[field: SerializeField] public string set_id { get; set; }
		[field: SerializeField] public BadgeVersion[] versions { get; set; }

		public void Initialise(JsonValue body) {
			this.set_id = body[TwitchWords.SET_ID].AsString;
			this.versions = body[TwitchWords.VERSIONS].AsJsonArray?.ToModelArray<BadgeVersion>();
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetChannelChatBadges;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChannelChatBadges;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}