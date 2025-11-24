using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-global-emotes">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetGlobalEmotes : IChat {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public Images images { get; set; }
		[field: SerializeField] public string[] format { get; set; }
		[field: SerializeField] public string[] scale { get; set; }
		[field: SerializeField] public string[] theme_mode { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.name = body[TwitchWords.NAME].AsString;
			this.images = new Images(body[TwitchWords.IMAGES]);
			this.format = body[TwitchWords.FORMAT].AsJsonArray?.CastToStringArray;
			this.scale = body[TwitchWords.SCALE].AsJsonArray?.CastToStringArray;
			this.theme_mode = body[TwitchWords.THEME_MODE].AsJsonArray?.CastToStringArray;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetGlobalEmotes;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetGlobalEmotes;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();
	}
}