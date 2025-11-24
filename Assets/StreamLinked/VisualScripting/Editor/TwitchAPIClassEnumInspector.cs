#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System;

	using ScoredProductions.StreamLinked.API;
	using ScoredProductions.StreamLinked.Utility;

	using Unity.VisualScripting;

	using UnityEditor;

	using UnityEngine;

	[Inspector(typeof(TwitchAPIClassEnum))]
	public class TwitchAPIClassEnumInspector : Inspector {
		private readonly TwitchAPIClassEnumDropdownProvider _dropdown;
		private readonly string[] _enumNames;
		private readonly int _width = 1;

		private Rect _buttonRect;
		
		public override float GetAdaptiveWidth() => this._width * 10; // Seems like the best value? idk

		public TwitchAPIClassEnumInspector(Metadata metadata) : base(metadata) {
			this._enumNames = Enum.GetNames(typeof(TwitchAPIClassEnum));
			for (int x = 0; x < this._enumNames.Length; x++) {
				this._enumNames[x] = this._enumNames[x].ToReadable();
				if (this._enumNames[x].Length > this._width) {
					this._width = this._enumNames[x].Length;
				}
			}
			this._dropdown = new TwitchAPIClassEnumDropdownProvider(this.OnDropdownOptionSelected);
		}

		protected override float GetHeight(float width, GUIContent label) => EditorGUIUtility.singleLineHeight;

		protected override void OnGUI(Rect position, GUIContent label) {
			position = Inspector.PrefixLabel(this.metadata, position, label); // Use this one to get the correct Inspector shape

			if (Event.current.type == EventType.Repaint) {
				this._buttonRect = position;
			}

			if (GUI.Button(this._buttonRect, new GUIContent(this._enumNames[(int)this.metadata.value]), EditorStyles.popup)) {
				this._dropdown.Show(this._buttonRect);
			}
		}

		private void OnDropdownOptionSelected(int index) {
			this.metadata.value = (TwitchAPIClassEnum)index;
			this.metadata.Editor().DrawLayout();
		}
	}
}
#endif