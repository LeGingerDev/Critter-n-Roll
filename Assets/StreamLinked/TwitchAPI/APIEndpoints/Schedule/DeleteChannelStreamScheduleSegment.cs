using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Schedule {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-channel-stream-schedule-segment">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct DeleteChannelStreamScheduleSegment : ISchedule, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.ChannelStreamScheduleSegment;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.DeleteChannelStreamScheduleSegment;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_schedule,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string ID => TwitchWords.ID;

		public readonly void Initialise(JsonValue value) { }
	}
}