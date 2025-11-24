using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Chat {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-shared-chat-session">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetSharedChatSession : IChat {

		[field: SerializeField] public string session_id { get; set; }
		[field: SerializeField] public string host_broadcaster_id { get; set; }
		[field: SerializeField] public Participants[] participants { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string updated_at { get; set; }

		public void Initialise(JsonValue body) {
			this.session_id = body[TwitchWords.SESSION_ID].AsString;
			this.host_broadcaster_id = body[TwitchWords.HOST_BROADCASTER_ID].AsString;
			this.participants = body[TwitchWords.PARTICIPANTS].AsJsonArray.ToModelArray<Participants>();
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.updated_at = body[TwitchWords.UPDATED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.Session;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetSharedChatSession;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}