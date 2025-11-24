using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-extension-configuration-segment">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetExtensionConfigurationSegment : IExtensions {

		[field: SerializeField] public string segment { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string content { get; set; }
		[field: SerializeField] public string version { get; set; }

		public void Initialise(JsonValue body) {
			this.segment = body[TwitchWords.SEGMENT].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.content = body[TwitchWords.CONTENT].AsString;
			this.version = body[TwitchWords.VERSION].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.ExtensionConfigurationSegment;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetExtensionConfigurationSegment;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string EXTENSION_ID => TwitchWords.EXTENSION_ID;
		public static string SEGMENT => TwitchWords.SEGMENT;
	}
}