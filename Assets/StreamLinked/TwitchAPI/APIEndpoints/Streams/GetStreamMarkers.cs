using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Streams {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-stream-markers">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetStreamMarkers : IStreams {

		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_name { get; set; }
		[field: SerializeField] public string user_login { get; set; }
		[field: SerializeField] public Video[] videos { get; set; }

		public void Initialise(JsonValue body) {
			this.user_id = body[TwitchWords.USER_ID].AsString;
			this.user_name = body[TwitchWords.USER_NAME].AsString;
			this.user_login = body[TwitchWords.USER_LOGIN].AsString;
			this.videos = body[TwitchWords.VIDEOS].AsJsonArray?.ToModelArray<Video>();
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetStreamMarkers;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetStreamMarkers;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_broadcast,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string VIDEO_ID => TwitchWords.VIDEO_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string BEFORE => TwitchWords.BEFORE;
		public static string AFTER => TwitchWords.AFTER;
	}
}