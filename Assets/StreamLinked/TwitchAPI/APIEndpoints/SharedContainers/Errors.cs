using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Errors : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string message { get; set; }
		[field: SerializeField] public string code { get; set; }

		public Errors(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.message = body[TwitchWords.MESSAGE].AsString;
			this.code = body[TwitchWords.CODE].AsString;
		}
	}
}
