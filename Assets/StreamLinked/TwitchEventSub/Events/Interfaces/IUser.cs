namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IUser : IEvent {
		public string user_id { get; set; }
		public string user_login { get; set; }
		public string user_name { get; set; }
	}
}
