using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#remove-blocked-term">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct RemoveBlockedTerm : IModeration, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.BlockedTerms;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.RemoveBlockedTerm;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_blocked_terms,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;
		public static string ID => TwitchWords.ID;

		public readonly void Initialise(JsonValue value) { }
	}
}