using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Schedule {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-icalendar">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChanneliCalendar : ISchedule {

		/// <summary>
		/// <see href="https://datatracker.ietf.org/doc/html/rfc5545">iCalendar data</see>
		/// </summary>
		[field: SerializeField] public string Response { get; set; }

		public void Initialise(JsonValue body) {
			this.Response = body[TwitchWords.RESPONSE];
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetChanneliCalendar;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChanneliCalendar;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.TEXT_CALENDAR);
	}
}