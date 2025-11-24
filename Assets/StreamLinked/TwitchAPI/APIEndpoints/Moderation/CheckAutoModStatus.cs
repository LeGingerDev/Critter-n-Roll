using System;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.LightJson;
using UnityEngine;
using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.API.Moderation {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#check-automod-status">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct CheckAutoModStatus : IModeration, IJsonRequest {

		[field: SerializeField] public string msg_id { get; set; }
		[field: SerializeField] public bool is_permitted { get; set; }

		public void Initialise(JsonValue body) {
			this.msg_id = body[TwitchWords.MSG_ID].AsString;
			this.is_permitted = body[TwitchWords.IS_PERMITTED].AsBoolean;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.POST;
		public readonly string Endpoint => TwitchAPILinks.CheckAutoModStatus;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.CheckAutoModStatus;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderation_read,
		};

		public static string BROADCASTER_ID => TwitchWords.BROADCASTER_ID;
		public static (string, string) CONTENT_TYPE => (TwitchWords.CONTENT_TYPE, TwitchWords.APPLICATION_JSON);
		public static string BuildDataJson((string, string)[] data) {
			JsonObject body = new JsonObject();
			JsonArray dataArray = new JsonArray();
			foreach ((string msg_id, string msg_text) in data) {
				JsonObject dataBody = new JsonObject() {
						{TwitchWords.MSG_ID, msg_id },
						{TwitchWords.MSG_TEXT, msg_text }
					};
				dataArray.Add(dataBody);
			};
			body.Add(TwitchWords.DATA, dataArray);
			return JsonWriter.Serialize(body);
		}
	}
}