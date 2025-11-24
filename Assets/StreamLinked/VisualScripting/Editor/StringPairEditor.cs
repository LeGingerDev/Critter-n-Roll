#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using ScoredProductions.StreamLinked.VisualScripting;

	using Unity.VisualScripting;

	using UnityEditor;

	using UnityEngine;

	[Inspector(typeof(StringPair))]
	public class StringPairInspector : Inspector {

		public Rect LeftRect;
		public Rect RightRect;

		public StringPairInspector(Metadata metadata) : base(metadata) {
		}

		protected override float GetHeight(float width, GUIContent label) => EditorGUIUtility.singleLineHeight;

		protected override void OnGUI(Rect position, GUIContent label) {
			position = Inspector.PrefixLabel(this.metadata, position, label);

			StringPair assignedValue = (StringPair)this.metadata.value;

			if (Event.current.type == EventType.Repaint) {
				this.LeftRect = new Rect(position.x, position.y, position.width / 2, position.height);
				this.RightRect = new Rect(position.x + (position.width / 2) + 1, position.y, (position.width / 2) - 1, position.height);
			}

			assignedValue.Item1 = EditorGUI.TextField(this.LeftRect, assignedValue.Item1);
			assignedValue.Item2 = EditorGUI.TextField(this.RightRect, assignedValue.Item2);

			this.metadata.value = assignedValue;
		}
	}
}
#endif