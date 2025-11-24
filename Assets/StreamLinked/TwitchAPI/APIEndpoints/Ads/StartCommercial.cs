using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Ads {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#start-commercial">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct StartCommercial : IAds, IJsonRequest {

		[field: SerializeField] public int length { get; set; }
		[field: SerializeField] public string message { get; set; }
		[field: SerializeField] public int retry_after { get; set; }

		public void Initialise(JsonValue body) {
			this.length = body[TwitchWords.LENGTH].AsInteger;
			this.message = body[TwitchWords.MESSAGE].AsString;
			this.retry_after = body[TwitchWords.RETRY_AFTER].AsInteger;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.StartCommercial;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.StartCommercial;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_edit_commercial,
		};

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(string broadcaster_id = null,
												int? length = null) {
			JsonObject body = new JsonObject();
			if (!string.IsNullOrEmpty(broadcaster_id)) {
				body.Add(TwitchWords.BROADCASTER_NAME, broadcaster_id);
			}
			if (length.HasValue) {
				body.Add(TwitchWords.LENGTH, length);
			}
			return body.Count > 0 ? JsonWriter.Serialize(body) : string.Empty;
		}
	}
}
