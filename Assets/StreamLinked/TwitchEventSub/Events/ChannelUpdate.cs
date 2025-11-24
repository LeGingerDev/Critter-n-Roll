using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	[Serializable]
	public struct ChannelUpdate : IEvent {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string language { get; set; }
		[field: SerializeField] public string category_id { get; set; }
		[field: SerializeField] public string category_name { get; set; }
		[field: SerializeField] public string[] content_classification_labels { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Update;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public ChannelUpdate(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.language = body[TwitchWords.LANGUAGE].AsString;
			this.category_id = body[TwitchWords.CATEGORY_ID].AsString;
			this.category_name = body[TwitchWords.CATEGORY_NAME].AsString;
			this.content_classification_labels = body[TwitchWords.CONTENT_CLASSIFICATION_LABELS].AsJsonArray?.CastToStringArray;
		}
	}
}