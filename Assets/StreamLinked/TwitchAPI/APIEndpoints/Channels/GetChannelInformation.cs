using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Channels {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-information">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChannelInformation : IChannels {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_language { get; set; }
		[field: SerializeField] public string game_id { get; set; }
		[field: SerializeField] public string game_name { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public int delay { get; set; }
		[field: SerializeField] public string[] tags { get; set; }
		[field: SerializeField] public string[] content_classification_labels { get; set; }
		[field: SerializeField] public bool is_branded_content { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_language = body[TwitchWords.BROADCASTER_LANGUAGE].AsString;
			this.game_id = body[TwitchWords.GAME_ID].AsString;
			this.game_name = body[TwitchWords.GAME_NAME].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.delay = body[TwitchWords.DELAY].AsInteger;
			this.tags = body[TwitchWords.TAGS].AsJsonArray?.CastToStringArray;
			this.content_classification_labels = body[TwitchWords.CONTENT_CLASSIFICATION_LABELS].AsJsonArray?.CastToStringArray;
			this.is_branded_content = body[TwitchWords.IS_BRANDED_CONTENT].AsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.ChannelInformation;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChannelInformation;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}