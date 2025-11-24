#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using ScoredProductions.StreamLinked.ManagersAndBuilders;

	using UnityEditor;

	using UnityEngine;

	[CustomEditor(typeof(TwitchMessageAtlasProducer))]
	public class TwitchMessageAtlasProducerEditor : Editor {
		public override void OnInspectorGUI() {
			this.DrawDefaultInspector();

			if (this.target is TwitchMessageAtlasProducer source) {
				GUILayout.Space(10);

				GUILayout.Label("Items current awaiting processing:");
				GUILayout.Label(source.NumberOfItemsWaiting.ToString());
				GUILayout.Label("Items completed and awaiting retrieval:");
				GUILayout.Label(source.ItemsAwaitingRetrieval.ToString());
			}
		}
	}
}
#endif