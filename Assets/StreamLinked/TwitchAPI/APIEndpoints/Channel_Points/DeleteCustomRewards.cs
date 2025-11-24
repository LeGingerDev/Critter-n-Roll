using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Channel_Points {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-custom-reward">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct DeleteCustomReward : IChannel_Points, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.CustomRewards;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.DeleteCustomReward;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_redemptions,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string ID => TwitchWords.ID;

		public readonly void Initialise(JsonValue value) { }
	}
}