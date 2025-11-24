using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API.Schedule {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-channel-stream-schedule">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateChannelStreamSchedule : ISchedule, INoResponse {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.UpdateChannelStreamSchedule;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateChannelStreamSchedule;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_schedule,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string IS_VACATION_ENABLED => TwitchWords.IS_VACATION_ENABLED;
		public static string VACATION_START_TIME => TwitchWords.VACATION_START_TIME;
		public static string VACATION_END_TIME => TwitchWords.VACATION_END_TIME;
		public static string TIMEZONE => TwitchWords.TIMEZONE;

		public readonly void Initialise(JsonValue value) { }
	}
}