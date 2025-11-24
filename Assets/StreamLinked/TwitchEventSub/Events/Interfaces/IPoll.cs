using ScoredProductions.StreamLinked.EventSub.Events.Objects;

namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IPoll : IChannel {
		public string id { get; set; }
		public string title { get; set; }
		public Choices[] choices { get; set; }
		public BitsVoting bits_voting { get; set; }
		public ChannelPointsVoting channel_points_voting { get; set; }
		public string started_at { get; set; }
		public string ends_at { get; set; }
	}
}
