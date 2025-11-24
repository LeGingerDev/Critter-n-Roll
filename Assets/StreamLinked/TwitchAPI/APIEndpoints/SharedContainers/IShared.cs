using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

namespace ScoredProductions.StreamLinked.API.SharedContainers {
	public interface IShared { }

	public static class ISharedExtensions {
		public static JsonValue ToJsonValue(this IShared value) {
			return (JsonValue)typeof(JsonWriter).GetMethod(nameof(JsonWriter.StructToJsonValue)).MakeGenericMethod(value.GetType()).Invoke(null, new object[] { value });
		}
	}
}
