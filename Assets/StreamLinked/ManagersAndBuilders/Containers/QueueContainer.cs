using System;

namespace ScoredProductions.StreamLinked.ManagersAndBuilders.Containers {
	/// <summary>
	/// Data container pertaining to a twitch emote
	/// </summary>
	public struct QueueContainer {
		public Guid ID { get; set; }
		public string[] Emotes { get; set; }
		public string[] Badges { get; set; }
		public long RoomId { get; set; }
	}
		
}