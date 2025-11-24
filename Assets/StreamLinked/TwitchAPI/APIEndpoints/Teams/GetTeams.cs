using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Teams {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-teams">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetTeams : ITeams {

		[field: SerializeField] public User[] users { get; set; }
		[field: SerializeField] public string background_image_url { get; set; }
		[field: SerializeField] public string banner { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string updated_at { get; set; }
		[field: SerializeField] public string info { get; set; }
		[field: SerializeField] public string thumbnail_url { get; set; }
		[field: SerializeField] public string team_name { get; set; }
		[field: SerializeField] public string team_display_name { get; set; }
		[field: SerializeField] public string id { get; set; }

		public void Initialise(JsonValue body) {
			this.users = body[TwitchWords.USERS].AsJsonArray?.ToModelArray<User>();
			this.background_image_url = body[TwitchWords.BACKGROUND_IMAGE_URL].AsString;
			this.banner = body[TwitchWords.BANNER].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.updated_at = body[TwitchWords.UPDATED_AT].AsString;
			this.info = body[TwitchWords.INFO].AsString;
			this.thumbnail_url = body[TwitchWords.THUMBNAIL_URL].AsString;
			this.team_name = body[TwitchWords.TEAM_NAME].AsString;
			this.team_display_name = body[TwitchWords.TEAM_DISPLAY_NAME].AsString;
			this.id = body[TwitchWords.ID].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetTeams;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetTeams;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string NAME => TwitchWords.NAME;
		public static string ID => TwitchWords.ID;
	}
}