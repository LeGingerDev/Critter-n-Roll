using System;

using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct IconUrls : IShared {

		[field: SerializeField] public string _100x100 { get; set; }
		[field: SerializeField] public string _24x24 { get; set; }
		[field: SerializeField] public string _300x200 { get; set; }

		public IconUrls(JsonValue body) {
			this._100x100 = body[TwitchWords._100X100].AsString;
			this._24x24 = body[TwitchWords._24X24].AsString;
			this._300x200 = body[TwitchWords._300X200].AsString;
		}
	}
}
