#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using ScoredProductions.StreamLinked.TwitchSceneFiles;

	using UnityEditor;

	[CustomEditor(typeof(TwitchMessageSingleDisplayer))]
	public class TwitchMessageSingleDisplayerEditor : Editor {

		public override void OnInspectorGUI() {
			this.DrawDefaultInspector();

			if (this.target is TwitchMessageSingleDisplayer s) {
				s.FilterByUser = EditorGUILayout.TextField("Filter By User", s.FilterByUser);
				if (!string.IsNullOrEmpty(s.FilterByUser)) {
					s.FilterByUser = s.FilterByUser.Trim();
				}
				this.serializedObject.FindProperty("_filterByUser").stringValue = s.FilterByUser;

				s.MessageDelay = EditorGUILayout.IntField("Message Delay", s.MessageDelay);
				if (s.MessageDelay < 0) {
					s.MessageDelay = 0;
				} else if (s.MessageDelay > 10) {
					s.MessageDelay = 10;
				}
				this.serializedObject.FindProperty("_messageDelay").intValue = s.MessageDelay;

				this.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
#endif