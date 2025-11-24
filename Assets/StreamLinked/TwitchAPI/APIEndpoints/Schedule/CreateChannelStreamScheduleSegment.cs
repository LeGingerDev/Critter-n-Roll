using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Schedule {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-channel-stream-schedule-segment">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateChannelStreamScheduleSegment : ISchedule, IJsonRequest {

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

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.ChannelStreamScheduleSegment;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateChannelStreamScheduleSegment;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_schedule,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string start_time,
												string timezone,
												string duration,
												bool? is_recurring = null,
												string category_id = null,
												string title = null) {
			JsonObject body = new JsonObject() {
					{TwitchWords.START_TIME, start_time},
					{TwitchWords.TIMEZONE, timezone},
					{TwitchWords.DURATION, duration }
				};
			if (is_recurring.HasValue) {
				body.Add(TwitchWords.IS_RECURRING, is_recurring);
			}
			if (!string.IsNullOrEmpty(category_id)) {
				body.Add(TwitchWords.CATEGORY_ID, category_id);
			}
			if (!string.IsNullOrEmpty(title)) {
				body.Add(TwitchWords.TITLE, title);
			}
			return JsonWriter.Serialize(body);
		}
	}
}