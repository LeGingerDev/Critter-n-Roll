using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct DropReason : IShared {

		[field: SerializeField] public string code { get; set; }
		[field: SerializeField] public string message { get; set; }

		public DropReason(JsonValue body) {
			this.code = body[TwitchWords.CODE].AsString;
			this.message = body[TwitchWords.MESSAGE].AsString;
		}
	}
}
