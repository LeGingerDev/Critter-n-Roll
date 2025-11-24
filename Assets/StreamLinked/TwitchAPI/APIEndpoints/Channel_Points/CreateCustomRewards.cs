using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Channel_Points {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-custom-rewards">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateCustomRewards : IChannel_Points, IJsonRequest {

		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string prompt { get; set; }
		[field: SerializeField] public int cost { get; set; }
		[field: SerializeField] public Images image { get; set; }
		[field: SerializeField] public Images default_image { get; set; }
		[field: SerializeField] public string background_color { get; set; }
		[field: SerializeField] public bool is_enabled { get; set; }
		[field: SerializeField] public bool is_user_input_required { get; set; }
		[field: SerializeField] public Global_Cooldown_Setting max_per_stream_setting { get; set; }
		[field: SerializeField] public Max_Per_Stream_Setting max_per_user_per_stream_setting { get; set; }
		[field: SerializeField] public Max_Per_User_Per_Stream_Setting global_cooldown_setting { get; set; }
		[field: SerializeField] public bool is_paused { get; set; }
		[field: SerializeField] public bool is_in_stock { get; set; }
		[field: SerializeField] public bool should_redemptions_skip_request_queue { get; set; }
		[field: SerializeField] public int redemptions_redeemed_current_stream { get; set; }
		[field: SerializeField] public string cooldown_expires_at { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.id = body[TwitchWords.ID].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.prompt = body[TwitchWords.PROMPT].AsString;
			this.cost = body[TwitchWords.COST].AsInteger;
			this.image = new Images(body[TwitchWords.IMAGE]);
			this.default_image = new Images(body[TwitchWords.DEFAULT_IMAGE]);
			this.background_color = body[TwitchWords.BACKGROUND_COLOR].AsString;
			this.is_enabled = body[TwitchWords.IS_ENABLED].AsBoolean;
			this.is_user_input_required = body[TwitchWords.IS_USER_INPUT_REQUIRED].AsBoolean;
			this.max_per_stream_setting = new Global_Cooldown_Setting(body[TwitchWords.MAX_PER_STREAM_SETTING]);
			this.max_per_user_per_stream_setting = new Max_Per_Stream_Setting(body[TwitchWords.MAX_PER_USER_PER_STREAM_SETTING]);
			this.global_cooldown_setting = new Max_Per_User_Per_Stream_Setting(body[TwitchWords.GLOBAL_COOLDOWN_SETTING]);
			this.is_paused = body[TwitchWords.IS_PAUSED].AsBoolean;
			this.is_in_stock = body[TwitchWords.IS_IN_STOCK].AsBoolean;
			this.should_redemptions_skip_request_queue = body[TwitchWords.SHOULD_REDEMPTIONS_SKIP_REQUEST_QUEUE].AsBoolean;
			this.redemptions_redeemed_current_stream = body[TwitchWords.REDEMPTIONS_REDEEMED_CURRENT_STREAM].AsInteger;
			this.cooldown_expires_at = body[TwitchWords.COOLDOWN_EXPIRES_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.CustomRewards;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateCustomRewards;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_redemptions,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(string title,
												long cost,
												string prompt = null,
												bool? is_enabled = null,
												string background_color = null,
												bool? is_user_input_required = null,
												bool? is_max_per_stream_enabled = null,
												int? max_per_stream = null,
												bool? is_max_per_user_per_stream_enabled = null,
												int? max_per_user_per_stream = null,
												bool? is_global_cooldown_enabled = null,
												int? global_cooldown_seconds = null,
												bool? should_redemptions_skip_request_queue = null) {
			JsonObject body = new JsonObject {
					{ TwitchWords.TITLE, title },
					{ TwitchWords.COST, cost }
				};
			if (!string.IsNullOrEmpty(prompt)) {
				body.Add(TwitchWords.PROMPT, prompt);
			}
			if (is_enabled.HasValue) {
				body.Add(TwitchWords.IS_ENABLED, is_enabled);
			}
			if (!string.IsNullOrEmpty(background_color)) {
				body.Add(TwitchWords.BACKGROUND_COLOR, background_color);
			}
			if (is_user_input_required.HasValue) {
				body.Add(TwitchWords.IS_USER_INPUT_REQUIRED, is_user_input_required);
			}
			if (is_max_per_stream_enabled.HasValue) {
				body.Add(TwitchWords.IS_MAX_PER_STREAM_ENABLED, is_max_per_stream_enabled);
			}
			if (max_per_stream.HasValue) {
				body.Add(TwitchWords.MAX_PER_STREAM, max_per_stream);
			}
			if (is_max_per_user_per_stream_enabled.HasValue) {
				body.Add(TwitchWords.IS_MAX_PER_USER_PER_STREAM_ENABLED, is_max_per_user_per_stream_enabled);
			}
			if (max_per_user_per_stream.HasValue) {
				body.Add(TwitchWords.MAX_PER_USER_PER_STREAM, max_per_user_per_stream);
			}
			if (is_global_cooldown_enabled.HasValue) {
				body.Add(TwitchWords.IS_GLOBAL_COOLDOWN_ENABLED, is_global_cooldown_enabled);
			}
			if (global_cooldown_seconds.HasValue) {
				body.Add(TwitchWords.GLOBAL_COOLDOWN_SECONDS, global_cooldown_seconds);
			}
			if (should_redemptions_skip_request_queue.HasValue) {
				body.Add(TwitchWords.SHOULD_REDEMPTIONS_SKIP_REQUEST_QUEUE, should_redemptions_skip_request_queue);
			}
			return JsonWriter.Serialize(body);
		}
	}
}