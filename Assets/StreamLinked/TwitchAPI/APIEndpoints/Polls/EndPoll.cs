using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Polls {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#end-poll">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct EndPoll : IPolls, IJsonRequest {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public Choice[] choices { get; set; }
		[field: SerializeField] public bool bits_voting_enabled { get; set; }
		[field: SerializeField] public int bits_per_vote { get; set; }
		[field: SerializeField] public bool channel_points_voting_enabled { get; set; }
		[field: SerializeField] public int channel_points_per_vote { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public int duration { get; set; }
		[field: SerializeField] public string started_at { get; set; }
		[field: SerializeField] public string ended_at { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.choices = body[TwitchWords.CHOICES].AsJsonArray?.ToModelArray<Choice>();
			this.bits_voting_enabled = body[TwitchWords.BITS_VOTING_ENABLED].AsBoolean;
			this.bits_per_vote = body[TwitchWords.BITS_PER_VOTE].AsInteger;
			this.channel_points_voting_enabled = body[TwitchWords.CHANNEL_POINTS_VOTING_ENABLED].AsBoolean;
			this.channel_points_per_vote = body[TwitchWords.CHANNEL_POINTS_PER_VOTE].AsInteger;
			this.status = body[TwitchWords.STATUS].AsString;
			this.duration = body[TwitchWords.DURATION].AsInteger;
			this.started_at = body[TwitchWords.STARTED_AT].AsString;
			this.ended_at = body[TwitchWords.ENDED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.Polls;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.EndPoll;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_polls,
		};

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string broadcaster_id,
												string id,
												string status) {
			JsonObject body = new JsonObject() {
					{TwitchWords.BROADCASTER_ID, broadcaster_id},
					{TwitchWords.ID, id},
					{TwitchWords.STATUS, status},
				};
			return JsonWriter.Serialize(body);
		}
	}
}