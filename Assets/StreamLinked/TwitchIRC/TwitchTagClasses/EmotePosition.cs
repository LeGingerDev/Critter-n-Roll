using System;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {

	/// <summary>
	/// Position data for an IRC chat message emote.
	/// </summary>
	[Serializable]
	public struct EmotePosition : IComparable<EmotePosition> {
		[field: SerializeField] public string EmoteId { get; private set; }
		[field: SerializeField] public int Start { get; private set; }
		[field: SerializeField] public int End { get; private set; }
		[field: SerializeField] public string Name { get; private set; }

		public EmotePosition(string emote, int start, int end, string name) {
			this.EmoteId = emote;
			this.Start = start;
			this.End = end;
			this.Name = name;
		}

		public readonly int CompareTo(EmotePosition other) {
			if (this.Start > other.End) {
				return 1;
			}
			if (other.Start > this.End) {
				return -1;
			}

			throw new InvalidOperationException("Sort cant place before or after value");
		}
	}
}