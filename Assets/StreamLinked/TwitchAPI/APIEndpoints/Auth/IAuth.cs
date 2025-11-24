namespace ScoredProductions.StreamLinked.API.Auth {
	public interface IAuth : ITwitchAPIDataObject { 
		public static bool EndpointIsAuthRequest(string endpoint) {
			return endpoint == TwitchAPILinks.GetAuthToken || endpoint == TwitchAPILinks.GetDeviceToken || endpoint == TwitchAPILinks.GetTokenValidation;
		}
	}
}
