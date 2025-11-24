using ScoredProductions.StreamLinked.API.Scopes;

namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface IEvent : IScope { 
		public TwitchEventSubSubscriptionsEnum Enum { get; }
	}
}
