using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Clips {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-clip">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CreateClip : IClips {

		[field: SerializeField] public string edit_url { get; set; }
		[field: SerializeField] public string id { get; set; }

		public void Initialise(JsonValue body) {
			this.edit_url = body[TwitchWords.EDIT_URL].AsString;
			this.id = body[TwitchWords.ID].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.Clips;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CreateClip;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.clips_edit,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string HAS_DELAY => TwitchWords.HAS_DELAY;

	}
}