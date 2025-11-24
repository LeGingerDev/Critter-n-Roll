namespace ScoredProductions.StreamLinked.API.Scopes {
    public interface IScope
    {
		/// <summary>
		/// Optional and Required scopes needed by Twitch, found at <see href="https://dev.twitch.tv/docs/authentication/scopes/"> Twitch Scopes</see>
		/// </summary>
		public TwitchScopesEnum[] Scopes { get; }
    }
}
