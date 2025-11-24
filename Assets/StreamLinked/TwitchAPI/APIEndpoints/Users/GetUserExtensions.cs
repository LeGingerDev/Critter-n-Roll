using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Users {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-extensions">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetUserExtensions : IUsers {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string version { get; set; }
		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public bool can_activate { get; set; }
		[field: SerializeField] public string[] type { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.version = body[TwitchWords.VERSION].AsString;
			this.name = body[TwitchWords.NAME].AsString;
			this.can_activate = body[TwitchWords.CAN_ACTIVATE].AsBoolean;
			this.type = body[TwitchWords.TYPE].AsJsonArray?.CastToStringArray;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetUserExtensions;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetUserExtensions;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_edit_broadcast,
			TwitchScopesEnum.user_read_broadcast,
		};

	}
}