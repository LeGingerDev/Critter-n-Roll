using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-emotes">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChannelEmotes : IChat {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public Images images { get; set; }
		[field: SerializeField] public string tier { get; set; }
		[field: SerializeField] public string emote_type { get; set; }
		[field: SerializeField] public string emote_set_id { get; set; }
		[field: SerializeField] public string[] format { get; set; }
		[field: SerializeField] public string[] scale { get; set; }
		[field: SerializeField] public string[] theme_mode { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.name = body[TwitchWords.NAME].AsString;
			this.images = new Images(body[TwitchWords.IMAGES]);
			this.tier = body[TwitchWords.TIER].AsString;
			this.emote_type = body[TwitchWords.EMOTE_TYPE].AsString;
			this.emote_set_id = body[TwitchWords.EMOTE_SET_ID].AsString;
			this.format = body[TwitchWords.FORMAT].AsJsonArray?.CastToStringArray;
			this.scale = body[TwitchWords.SCALE].AsJsonArray?.CastToStringArray;
			this.theme_mode = body[TwitchWords.THEME_MODE].AsJsonArray?.CastToStringArray;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetChannelEmotes;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChannelEmotes;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		
	}
}