using System;

namespace ScoredProductions.StreamLinked.API {

	[Serializable]
	public enum TwitchAPIRequestMethod {
		POST,
		GET,
		PUT,
		DELETE,
		PATCH
	}
}