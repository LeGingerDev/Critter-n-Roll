using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Streams {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-stream-key">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetStreamKey : IStreams {

		[field: SerializeField] public string stream_key { get; set; }

		public void Initialise(JsonValue body) {
			this.stream_key = body[TwitchWords.STREAM_KEY].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetStreamKey;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetStreamKey;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_stream_key,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}