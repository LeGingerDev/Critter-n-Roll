using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.LightJson;

namespace ScoredProductions.StreamLinked.API {
	public interface ITwitchAPIDataObject : IScope {
		public TwitchAPIRequestMethod HTTPMethod { get; }
		public string Endpoint { get; }
		public TwitchAPIClassEnum TypeEnum { get;}

		public void Initialise(JsonValue value);

		public static bool HasResponse<T>() where T : ITwitchAPIDataObject, new() {
			return !typeof(INoResponse).IsAssignableFrom(typeof(T));
		}
	}

	public static class ITwitchAPIDataObjectExtensions {
		public static bool HasResponse(this ITwitchAPIDataObject value) {
			return !typeof(INoResponse).IsAssignableFrom(value.GetType());
		}
	}
}
