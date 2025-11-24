using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Schedule {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-stream-schedule">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChannelStreamSchedule : ISchedule {

		[field: SerializeField] public Segment[] segments { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string vacation { get; set; }

		public void Initialise(JsonValue body) {
			this.segments = body[TwitchWords.SEGMENTS].AsJsonArray?.ToModelArray<Segment>();
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.vacation = body[TwitchWords.VACATION].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetChannelStreamSchedule;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChannelStreamSchedule;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string ID => TwitchWords.ID;
		public static string START_TIME => TwitchWords.START_TIME;
		/// <summary>
		/// Not Supported...
		/// </summary>
		public static string UTC_OFFSET => TwitchWords.UTC_OFFSET; // If its not supported why is it in?
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}