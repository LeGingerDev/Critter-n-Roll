using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userwhispermessage">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct WhisperReceived : IEvent {

		[field: SerializeField] public string from_user_id { get; set; }
		[field: SerializeField] public string from_user_name { get; set; }
		[field: SerializeField] public string from_user_login { get; set; }
		[field: SerializeField] public string to_user_id { get; set; }
		[field: SerializeField] public string to_user_name { get; set; }
		[field: SerializeField] public string to_user_login { get; set; }
		[field: SerializeField] public string whisper_id { get; set; }
		[field: SerializeField] public Whisper whisper { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Whisper_Received;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_read_whispers,
			TwitchScopesEnum.user_manage_whispers,
		};

		public WhisperReceived(JsonValue body) {
			this.from_user_id = body[TwitchWords.FROM_USER_ID].AsString;
			this.from_user_name = body[TwitchWords.FROM_USER_NAME].AsString;
			this.from_user_login = body[TwitchWords.FROM_USER_LOGIN].AsString;
			this.to_user_id = body[TwitchWords.TO_USER_ID].AsString;
			this.to_user_name = body[TwitchWords.TO_USER_NAME].AsString;
			this.to_user_login = body[TwitchWords.TO_USER_LOGIN].AsString;
			this.whisper_id = body[TwitchWords.WHISPER_ID].AsString;
			this.whisper = new Whisper(body[TwitchWords.WHISPER]);
		}
	}
}