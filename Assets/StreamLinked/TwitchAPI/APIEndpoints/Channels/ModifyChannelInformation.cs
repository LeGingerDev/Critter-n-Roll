using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

namespace ScoredProductions.StreamLinked.API.Channels {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#modify-channel-information">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct ModifyChannelInformation : IChannels, INoResponse, IJsonRequest {

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.ChannelInformation;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.ModifyChannelInformation;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_broadcast,
		};

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;

		public static string BuildDataJson(string game_id = null,
												string broadcaster_language = null,
												string title = null,
												int? delay = null,
												string[] tags = null,
												(string ID, bool IS_ENABLED)[] content_classification_labels = null,
												bool? is_branded_content = null) {
			JsonObject body = new JsonObject();
			if (!string.IsNullOrWhiteSpace(game_id)) {
				body.Add(TwitchWords.GAME_ID, game_id);
			}
			if (!string.IsNullOrWhiteSpace(broadcaster_language)) {
				body.Add(TwitchWords.BROADCASTER_LANGUAGE, broadcaster_language);
			}
			if (!string.IsNullOrWhiteSpace(title)) {
				body.Add(TwitchWords.TITLE, title);
			}
			if (delay.HasValue) {
				body.Add(TwitchWords.DELAY, delay.Value);
			}
			if (tags?.Length > 0) {
				body.Add(TwitchWords.TAGS, new JsonArray(tags));
			}

			if (content_classification_labels?.Length > 0) {
				JsonArray outer = new JsonArray();
				foreach ((string ID, bool IS_ENABLED) label in content_classification_labels) {
					JsonObject inner = new JsonObject() {
							{ TwitchWords.ID, label.ID },
							{ TwitchWords.IS_ENABLED, label.IS_ENABLED }
						};

					outer.Add(inner);
				}

				body.Add(TwitchWords.CONTENT_CLASSIFICATION_LABELS, outer);
			}

			if (is_branded_content == true) {
				body.Add(TwitchWords.IS_BRANDED_CONTENT, is_branded_content);
			}
			return body.Count > 0 ? JsonWriter.Serialize(body) : string.Empty;
		}

		public readonly void Initialise(JsonValue value) { }
	}
}