using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using UnityEngine;
using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;

namespace ScoredProductions.StreamLinked.API.Schedule {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-channel-stream-schedule-segment">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateChannelStreamScheduleSegment : ISchedule, IJsonRequest {

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

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.ChannelStreamScheduleSegment;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateChannelStreamScheduleSegment;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_schedule,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string ID => TwitchWords.ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string start_time = null,
												string duration = null,
												string category_id = null,
												string title = null,
												bool? is_canceled = null,
												string timezone = null) {
			JsonObject body = new JsonObject();
			if (!string.IsNullOrEmpty(start_time)) {
				body.Add(TwitchWords.START_TIME, start_time);
			}
			if (!string.IsNullOrEmpty(duration)) {
				body.Add(TwitchWords.DURATION, duration);
			}
			if (!string.IsNullOrEmpty(category_id)) {
				body.Add(TwitchWords.CATEGORY_ID, category_id);
			}
			if (!string.IsNullOrEmpty(title)) {
				body.Add(TwitchWords.TITLE, title);
			}
			if (is_canceled.HasValue) {
				body.Add(TwitchWords.IS_CANCELED, is_canceled);
			}
			if (!string.IsNullOrEmpty(timezone)) {
				body.Add(TwitchWords.TIMEZONE, timezone);
			}
			return body.Count > 0 ? JsonWriter.Serialize(body) : string.Empty;
		}
	}
}