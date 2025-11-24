using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Hype_Train {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-hype-train-events">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetHypeTrainEvents : IHype_Train {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string event_type { get; set; }
		[field: SerializeField] public string event_timestamp { get; set; }
		[field: SerializeField] public string version { get; set; }
		[field: SerializeField] public EventData event_data { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.event_type = body[TwitchWords.EVENT_TYPE].AsString;
			this.event_timestamp = body[TwitchWords.EVENT_TIMESTAMP].AsString;
			this.version = body[TwitchWords.VERSION].AsString;
			this.event_data = new EventData(body[TwitchWords.EVENT_DATA]);
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetHypeTrainEvents;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetHypeTrainEvents;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_read_hype_train,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static string FIRST => TwitchWords.FIRST;
		public static string AFTER => TwitchWords.AFTER;
	}
}