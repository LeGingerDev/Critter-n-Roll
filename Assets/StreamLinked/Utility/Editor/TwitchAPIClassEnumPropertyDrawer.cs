#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using ScoredProductions.StreamLinked.API;

	using UnityEditor;

	using UnityEngine;

	[CustomPropertyDrawer(typeof(TwitchAPIClassEnum))]
	public class TwitchAPIClassEnumPropertyDrawer : PropertyDrawer {
		private TwitchAPIClassEnumDropdownProvider _dropdown;
		private SerializedProperty _property;
		private Rect _buttonRect;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			this._dropdown ??= new TwitchAPIClassEnumDropdownProvider(this.OnDropdownOptionSelected);
			this._property = property;

			position = EditorGUI.PrefixLabel(position, label);

			if (Event.current.type == EventType.Repaint) {
				this._buttonRect = position;
			}

			if (GUI.Button(this._buttonRect, new GUIContent(property.enumDisplayNames[property.enumValueIndex]), EditorStyles.popup)) {
				this._dropdown.Show(this._buttonRect);
			}
		}

		private void OnDropdownOptionSelected(int index) {
			this._property.enumValueIndex = index;
			this._property.serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif