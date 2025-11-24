using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-chat-settings">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetChatSettings : IChat {

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

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.ChatSettings;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetChatSettings;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_chat_settings,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string MODERATOR_ID => TwitchWords.MODERATOR_ID;

	}
}