#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using ScoredProductions.StreamLinked.EventSub;
	using ScoredProductions.StreamLinked.TwitchSceneFiles;

	using UnityEditor;

	using UnityEngine;

	[CustomEditor(typeof(TwitchEventSubStartupExample))]
	public class TwitchEventSubStartupExampleEditor : Editor {

		public override void OnInspectorGUI() {
			this.DrawDefaultInspector();

			if (this.target is TwitchEventSubStartupExample source) {

				GUI.enabled = EditorApplication.isPlayingOrWillChangePlaymode && !TwitchEventSubClient.EventSubStartingUp && !TwitchEventSubClient.EventSubConnectionActive;

				if (GUILayout.Button("Subscribe to Channel Polling events")) {
					source.BuildUserSubscriptionsForUser().ConfigureAwait(false);
				}
			}
		}
	}
}
#endif