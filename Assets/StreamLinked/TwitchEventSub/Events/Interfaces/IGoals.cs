namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IGoals : IChannel {
		public string id { get; set; }
		public string type { get; set; }
		public string description { get; set; }
		public bool is_achieved { get; set; }
		public int current_amount { get; set; }
		public int target_amount { get; set; }
		public int started_at { get; set; }
		public int ended_at { get; set; }
	}
}