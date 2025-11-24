using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Streams {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-stream-marker">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateStreamMarker : IStreams {

		[field: SerializeField] public int id { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public int position_seconds { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsInteger;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.position_seconds = body[TwitchWords.POSITION_SECONDS].AsInteger;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.CreateStreamMarker;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateStreamMarker;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_broadcast,
		};

		public static string USER_ID => TwitchWords.USER_ID;
		public static string DESCRIPTION => TwitchWords.DESCRIPTION;
	}
}