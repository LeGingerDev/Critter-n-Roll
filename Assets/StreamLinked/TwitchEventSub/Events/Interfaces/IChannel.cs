namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IChannel : IEvent {
		public string broadcaster_user_id { get; set; }
		public string broadcaster_user_name { get; set; }
		public string broadcaster_user_login { get; set; }
	}
}
