using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Users {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-user">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct UpdateUser : IUsers {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string login { get; set; }
		[field: SerializeField] public string display_name { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public string broadcaster_type { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public string profile_image_url { get; set; }
		[field: SerializeField] public string offline_image_url { get; set; }
		[field: SerializeField] public int view_count { get; set; }
		[field: SerializeField] public string email { get; set; }
		[field: SerializeField] public string created_at { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.login = body[TwitchWords.LOGIN].AsString;
			this.display_name = body[TwitchWords.DISPLAY_NAME].AsString;
			this.type = body[TwitchWords.TYPE].AsString;
			this.broadcaster_type = body[TwitchWords.BROADCASTER_TYPE].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.profile_image_url = body[TwitchWords.PROFILE_IMAGE_URL].AsString;
			this.offline_image_url = body[TwitchWords.OFFLINE_IMAGE_URL].AsString;
			this.view_count = body[TwitchWords.VIEW_COUNT].AsInteger;
			this.email = body[TwitchWords.EMAIL].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PUT;
		public readonly string Endpoint => TwitchAPILinks.Users;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.UpdateUser;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.user_edit,
			TwitchScopesEnum.user_read_email,
		};

		public static string DESCRIPTION => TwitchWords.DESCRIPTION;
	}
}