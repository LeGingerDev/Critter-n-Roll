using System;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	public struct OTHER : ITagContainer {

		[field: SerializeField] public string TagData { get; set; }

		public OTHER(string tags) {
			this.TagData = tags;
		}
	}
}