using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Videos {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-videos">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct DeleteVideos : IVideos {

		/// <summary>
		/// data returned is just a literal string[]
		/// <code>e.g { "data": [0,1,2] }</code>
		/// </summary>
		[field: SerializeField] public string[] _data;

		public void Initialise(JsonValue body) {
			this._data = body.AsJsonArray?.CastToStringArray;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.DELETE;
		public readonly string Endpoint => TwitchAPILinks.Videos;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.DeleteVideos;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_videos,
		};

		public static string ID => TwitchWords.ID;
	}
}