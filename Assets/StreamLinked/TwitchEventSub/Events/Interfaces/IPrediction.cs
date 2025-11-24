using ScoredProductions.StreamLinked.EventSub.Events.Objects;

namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IPrediction : IChannel {
		public string id { get; set; }
		public string title { get; set; }
		public Outcomes[] outcomes { get; set; }
		public string started_at { get; set; }
		public string locks_at { get; set; }
	}
}
