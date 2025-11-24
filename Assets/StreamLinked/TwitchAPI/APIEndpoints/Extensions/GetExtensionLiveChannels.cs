using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-extension-live-channels">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetExtensionLiveChannels : IExtensions {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string game_name { get; set; }
		[field: SerializeField] public string game_id { get; set; }
		[field: SerializeField] public string title { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.game_name = body[TwitchWords.GAME_NAME].AsString;
			this.game_id = body[TwitchWords.GAME_ID].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetExtensionLiveChannels;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetExtensionLiveChannels;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string EXTENSION_ID => TwitchWords.EXTENSION_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}