using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-chat-settings">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateChatSettings : IChat, IJsonRequest {

		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public bool emote_mode { get; set; }
		[field: SerializeField] public bool follower_mode { get; set; }
		[field: SerializeField] public int follower_mode_duration { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public bool non_moderator_chat_delay { get; set; }
		[field: SerializeField] public int non_moderator_chat_delay_duration { get; set; }
		[field: SerializeField] public bool slow_mode { get; set; }
		[field: SerializeField] public int slow_mode_wait_time { get; set; }
		[field: SerializeField] public bool subscriber_mode { get; set; }
		[field: SerializeField] public bool unique_chat_mode { get; set; }

		public void Initialise(JsonValue body) {
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.emote_mode = body[TwitchWords.EMOTE_MODE].AsBoolean;
			this.follower_mode = body[TwitchWords.FOLLOWER_MODE].AsBoolean;
			this.follower_mode_duration = body[TwitchWords.FOLLOWER_MODE_DURATION].AsInteger;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.non_moderator_chat_delay = body[TwitchWords.NON_MODERATOR_CHAT_DELAY].AsBoolean;
			this.non_moderator_chat_delay_duration = body[TwitchWords.NON_MODERATOR_CHAT_DELAY_DURATION].AsInteger;
			this.slow_mode = body[TwitchWords.SLOW_MODE].AsBoolean;
			this.slow_mode_wait_time = body[TwitchWords.SLOW_MODE_WAIT_TIME].AsInteger;
			this.subscriber_mode = body[TwitchWords.SUBSCRIBER_MODE].AsBoolean;
			this.unique_chat_mode = body[TwitchWords.UNIQUE_CHAT_MODE].AsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.ChatSettings;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateChatSettings;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_manage_chat_settings,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(bool? emote_mode = null,
												bool? follower_mode = null,
												int? follower_mode_duration = null,
												bool? non_moderator_chat_delay = null,
												int? non_moderator_chat_delay_duration = null,
												bool? slow_mode = null,
												int? slow_mode_wait_time = null,
												bool? subscriber_mode = null,
												bool? unique_chat_mode = null) {
			JsonObject body = new JsonObject();
			if (emote_mode.HasValue) {
				body.Add(TwitchWords.EMOTE_MODE, emote_mode);
			}
			if (follower_mode.HasValue) {
				body.Add(TwitchWords.FOLLOWER_MODE, follower_mode);
			}
			if (follower_mode_duration.HasValue) {
				body.Add(TwitchWords.FOLLOWER_MODE_DURATION, follower_mode_duration);
			}
			if (non_moderator_chat_delay.HasValue) {
				body.Add(TwitchWords.NON_MODERATOR_CHAT_DELAY, non_moderator_chat_delay);
			}
			if (non_moderator_chat_delay_duration.HasValue) {
				body.Add(TwitchWords.NON_MODERATOR_CHAT_DELAY_DURATION, non_moderator_chat_delay_duration);
			}
			if (slow_mode.HasValue) {
				body.Add(TwitchWords.SLOW_MODE, slow_mode);
			}
			if (slow_mode_wait_time.HasValue) {
				body.Add(TwitchWords.SLOW_MODE_WAIT_TIME, slow_mode_wait_time);
			}
			if (subscriber_mode.HasValue) {
				body.Add(TwitchWords.SUBSCRIBER_MODE, subscriber_mode);
			}
			if (unique_chat_mode.HasValue) {
				body.Add(TwitchWords.UNIQUE_CHAT_MODE, unique_chat_mode);
			}
			return body.Count > 0 ? JsonWriter.Serialize(body) : string.Empty;
		}
	}
}