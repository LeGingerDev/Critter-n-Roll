using System;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {

	[Serializable]
	public struct BadgeData {
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public int Value { get; private set; }

		public BadgeData(string name, int value) {
			this.Name = name;
			this.Value = value;
		}
	}
}