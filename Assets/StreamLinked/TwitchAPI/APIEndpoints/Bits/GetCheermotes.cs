using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Bits {

	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-cheermotes">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetCheermotes : IBits {

		[field: SerializeField] public string prefix { get; set; }
		[field: SerializeField] public Tiers tiers { get; set; }
		[field: SerializeField] public string type { get; set; }
		[field: SerializeField] public int order { get; set; }
		[field: SerializeField] public string last_updated { get; set; }
		[field: SerializeField] public bool is_charitable { get; set; }

		public void Initialise(JsonValue body) {
			this.prefix = body[TwitchWords.PREFIX_EXCLAMATION].AsString;
			this.tiers = new Tiers(body[TwitchWords.TIERS]);
			this.type = body[TwitchWords.TYPE].AsString;
			this.order = body[TwitchWords.ORDER].AsInteger;
			this.last_updated = body[TwitchWords.LAST_UPDATED].AsString;
			this.is_charitable = body[TwitchWords.IS_CHARITABLE].AsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetCheermotes;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetCheermotes;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
	}
}