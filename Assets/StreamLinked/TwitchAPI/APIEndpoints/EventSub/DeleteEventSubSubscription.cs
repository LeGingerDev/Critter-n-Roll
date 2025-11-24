using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.EventSub {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-eventsub-subscription">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct DeleteEventSubSubscription : IEventSub, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.EventSubSubscription;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.DeleteEventSubSubscription;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string ID => TwitchWords.ID;

		public readonly void Initialise(JsonValue value) { }
	}
}