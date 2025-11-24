namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface ISubscription : IChannel, IUser {
		public string tier { get; set; }
	}
}
