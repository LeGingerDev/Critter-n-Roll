namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IShield : IChannel {
		public string moderator_user_id { get; set; }
		public string moderator_user_login { get; set; }
		public string moderator_user_name { get; set; }
	}
}
