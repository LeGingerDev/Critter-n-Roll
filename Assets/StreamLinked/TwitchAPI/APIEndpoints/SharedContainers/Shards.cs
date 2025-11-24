using System;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	[Serializable]
	public struct Shards : IShared {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public Transport transport { get; set; }

		public Shards(string id, Transport transport) {
			this.id = id;
			this.transport = transport;
		}
	}
}
