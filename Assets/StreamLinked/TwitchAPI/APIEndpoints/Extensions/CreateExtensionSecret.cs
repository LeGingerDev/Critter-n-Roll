using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-extension-secret">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateExtensionSecret : IExtensions {

		[field: SerializeField] public int format_version { get; set; }
		[field: SerializeField] public Secrets[] secrets { get; set; }

		public void Initialise(JsonValue body) {
			this.format_version = body[TwitchWords.FORMAT_VERSION].AsInteger;
			this.secrets = body[TwitchWords.SECRETS].AsJsonArray?.ToModelArray<Secrets>();
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.ExtensionSecrets;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateExtensionSecret;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string EXTENSION_ID => TwitchWords.EXTENSION_ID;
		public static string DELAY => TwitchWords.DELAY;
	}
}