using ScoredProductions.StreamLinked.EventSub.Events.Objects;

namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IHypeTrain : IChannel {
		public string id { get; set; }
		public int total { get; set; }
		public TopContributions top_contributions { get; set; }
		public int level { get; set; }
		public string started_at { get; set; }
	}
}
