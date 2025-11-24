using ScoredProductions.StreamLinked.EventSub.Events.Objects;

namespace ScoredProductions.StreamLinked.EventSub.Interfaces {
	public interface ICharityCampaign : IChannel {

		public string id { get; set; }
		public string charity_name { get; set; }
		public string charity_description { get; set; }
		public string charity_logo { get; set; }
		public string charity_website { get; set; }
		public Amount current_amount { get; set; }
		public Amount target_amount { get; set; }
	}
}
