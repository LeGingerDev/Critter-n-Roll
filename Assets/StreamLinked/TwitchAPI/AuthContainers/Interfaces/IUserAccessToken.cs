namespace ScoredProductions.StreamLinked.API.AuthContainers {
	public interface IUserAccessToken : IAuthFlow {
		public string[] Scope { get; set; }
	}
}