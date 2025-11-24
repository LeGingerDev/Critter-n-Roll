using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-message">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct SendChatMessage : IChat, IJsonRequest {

		[field: SerializeField] public string message_id { get; set; }
		[field: SerializeField] public bool is_sent { get; set; }
		[field: SerializeField] public DropReason drop_reason { get; set; }

		public void Initialise(JsonValue body) {
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
			this.is_sent = body[TwitchWords.IS_SENT].AsBoolean;
			this.drop_reason = new DropReason(body[TwitchWords.DROP_REASON]);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.SendChatMessage;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.SendChatMessage;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_bot,
			TwitchScopesEnum.user_bot,
			TwitchScopesEnum.user_write_chat,
		};

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);

		public static string BuildDataJson(string broadcaster_id,
											string sender_id, 
											string message,
											string reply_parent_message_id = null,
											bool? for_source_only = false) {
			JsonObject body = new JsonObject {
					{ TwitchWords.BROADCASTER_ID, broadcaster_id },
					{ TwitchWords.SENDER_ID, sender_id },
					{ TwitchWords.MESSAGE, message },
				};
			if (!string.IsNullOrEmpty(reply_parent_message_id)) {
				body.Add(TwitchWords.REPLY_PARENT_MESSAGE_ID, reply_parent_message_id);
			}
			if (for_source_only.HasValue) {
				body.Add(TwitchWords.FOR_SOURCE_ONLY, for_source_only.Value);
			}
			return JsonWriter.Serialize(body);
		}

	}
}