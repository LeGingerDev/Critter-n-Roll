using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using UnityEngine;
using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;

namespace ScoredProductions.StreamLinked.API.Predictions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#end-prediction">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct EndPrediction : IPredictions, IJsonRequest {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string broadcaster_id { get; set; }
		[field: SerializeField] public string broadcaster_name { get; set; }
		[field: SerializeField] public string broadcaster_login { get; set; }
		[field: SerializeField] public string title { get; set; }
		[field: SerializeField] public string winning_outcome_id { get; set; }
		[field: SerializeField] public Outcome[] outcomes { get; set; }
		[field: SerializeField] public int prediction_window { get; set; }
		[field: SerializeField] public string status { get; set; }
		[field: SerializeField] public string created_at { get; set; }
		[field: SerializeField] public string ended_at { get; set; }
		[field: SerializeField] public string locked_at { get; set; }

		public void Initialise(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.broadcaster_id = body[TwitchWords.BROADCASTER_ID].AsString;
			this.broadcaster_name = body[TwitchWords.BROADCASTER_NAME].AsString;
			this.broadcaster_login = body[TwitchWords.BROADCASTER_LOGIN].AsString;
			this.title = body[TwitchWords.TITLE].AsString;
			this.winning_outcome_id = body[TwitchWords.WINNING_OUTCOME_ID].AsString;
			this.outcomes = body[TwitchWords.OUTCOMES].AsJsonArray?.ToModelArray<Outcome>();
			this.prediction_window = body[TwitchWords.PREDICTION_WINDOW].AsInteger;
			this.status = body[TwitchWords.STATUS].AsString;
			this.created_at = body[TwitchWords.CREATED_AT].AsString;
			this.ended_at = body[TwitchWords.ENDED_AT].AsString;
			this.locked_at = body[TwitchWords.LOCKED_AT].AsString;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.PATCH;
		public readonly string Endpoint => TwitchAPILinks.Predictions;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.EndPrediction;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_predictions,
		};

		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson(string broadcaster_id,
												string id,
												string status,
												string winning_outcome_id = null) {
			JsonObject body = new JsonObject() {
					{TwitchWords.BROADCASTER_ID, broadcaster_id},
					{TwitchWords.ID, id},
					{TwitchWords.STATUS, status }
				};
			if (!string.IsNullOrEmpty(winning_outcome_id)) {
				body.Add(TwitchWords.WINNING_OUTCOME_ID, winning_outcome_id);
			}
			return JsonWriter.Serialize(body);
		}
	}
}