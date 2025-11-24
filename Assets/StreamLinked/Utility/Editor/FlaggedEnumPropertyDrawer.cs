#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System;
	using System.Collections.Generic;

	using ScoredProductions.StreamLinked.Utility;

	using UnityEditor;

	using UnityEngine;

	[CustomPropertyDrawer(typeof(FlaggedEnum<>), true)]
	public class FlaggedEnumPropertyDrawer : PropertyDrawer {

		private Vector2 scrollPos;

		private Type GenericEnumType;

		private int NumEnums;

		private string[] EnumNames;

		private float DropdownSize = 0;

		private string SearchField = "";

		private int SearchFound = 0;

		private readonly List<int> FoundValues = new List<int>();
		private readonly List<string> FoundNames = new List<string>();

		private void SetUpStoredDetails() {
			// Initialise reflection values
			if (this.GenericEnumType == null) {
				this.GenericEnumType = this.fieldInfo.FieldType.GetGenericArguments()[0]; // Can only be 1
			}
			if (this.NumEnums <= 0) {
				this.NumEnums = (int)this.fieldInfo.FieldType.GetProperty("EnumLength").GetValue(null);
			}
			if (this.EnumNames == null) {
				this.EnumNames = (string[])this.fieldInfo.FieldType.GetProperty("EnumNames").GetValue(null);
				this.EnumNames.MakeReadable();
			}
		}

		// Space taken up by the property in the inspector
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
			=> property.isExpanded ? Mathf.Min(200f, this.DropdownSize + EditorGUIUtility.singleLineHeight) : EditorGUIUtility.singleLineHeight;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {			
			this.FoundValues.Clear();
			this.FoundNames.Clear();

			this.SetUpStoredDetails();

			EditorGUI.BeginProperty(position, label, property);

			// Aquire SerializeField used to store values
			SerializedProperty answers = property.FindPropertyRelative("_serializedAnswers");
			if (answers != null) {
				for (int x = 0; x < answers.arraySize; x++) {
					SerializedProperty value = answers.GetArrayElementAtIndex(x);

					if (value != null) {
						int v = value.intValue;
						this.FoundValues.Add(v);
						this.FoundNames.Add(this.EnumNames[v]);
					}
				}
			}
			
			Rect rectFoldout = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			property.isExpanded = EditorGUI.Foldout(rectFoldout, property.isExpanded, label);

			Rect rectValuesSelected = new Rect(rectFoldout.x + EditorGUIUtility.labelWidth + 1, rectFoldout.y, rectFoldout.width - EditorGUIUtility.labelWidth - 1, EditorGUIUtility.singleLineHeight);
			EditorGUI.LabelField(rectValuesSelected, new GUIContent($"{answers.arraySize} Value{(answers.arraySize == 1 ? "" : "s")} Selected", this.FoundNames.Count > 0 ? string.Join('\n', this.FoundNames) : ""));
			
			if (property.isExpanded) {
				EditorGUI.indentLevel = 1;
				int nextLine = 0;

				// Rects for the scroll view and inside the scroll view
				Rect posRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight);
				Rect innerRect = new Rect(posRect.x, posRect.y, posRect.width - EditorGUIUtility.singleLineHeight, this.DropdownSize);

				this.scrollPos = GUI.BeginScrollView(posRect, this.scrollPos, innerRect);

				Rect searchFieldRect = this.IncrementLine(ref nextLine, innerRect);
				this.SearchField = EditorGUI.TextField(searchFieldRect, this.SearchField);
				EditorGUI.LabelField(searchFieldRect, new GUIContent(string.IsNullOrWhiteSpace(this.SearchField) ? "Search Field" : "", "Use this field to search for the enums you wish to select."), EditorStyles.centeredGreyMiniLabel);

				// Data writing
				answers.arraySize = 0;
				this.SearchFound = 1;
				List<int> selectedEnums = new List<int>(this.NumEnums); // Flagged enum only stores the enums that were flagged, not the whole list with on off
				for (int x = 0; x < this.NumEnums; x++) {
					string enumName = this.EnumNames[x];
					bool searchContains = string.IsNullOrWhiteSpace(this.SearchField) || enumName.Contains(this.SearchField, StringComparison.CurrentCultureIgnoreCase);
					bool preSelected = this.FoundValues.Contains(x);

					if (searchContains) {
						this.SearchFound++;
					}

					if (property.editable) {
						if ((!searchContains && preSelected)
						|| (searchContains && EditorGUI.ToggleLeft(this.IncrementLine(ref nextLine, innerRect), enumName, preSelected))) {
							selectedEnums.Add(x);
						}
					} else {
						if (searchContains) {
							EditorGUI.LabelField(this.IncrementLine(ref nextLine, innerRect), enumName);
						}
					}
				}

				this.DropdownSize = this.DropdownHeight(this.SearchFound); // Enums + Search field

				if (property.editable) {
					answers.arraySize = selectedEnums.Count; // Set the collection size in one go
					for (int x = 0; x < selectedEnums.Count; x++) { // Loop again
						answers.GetArrayElementAtIndex(x).enumValueIndex = selectedEnums[x]; // Set each value
					}
				}

				GUI.EndScrollView();
			}

			EditorGUI.indentLevel = 0;
			EditorGUI.EndProperty();
		}

		private Rect IncrementLine(ref int indentValue, Rect originRect) {
			return new Rect(originRect.x, originRect.y + (indentValue++ * EditorGUIUtility.singleLineHeight), originRect.width, EditorGUIUtility.singleLineHeight);
		}

		private float DropdownHeight(int numberOfRows) {
			return this.DropdownSize = EditorGUIUtility.singleLineHeight * numberOfRows;
		}

		public static void DrawReadonlyView<T>(T[] values, GUIContent label, ref bool fold) where T : Enum, new() {
			T[] flags = values;
			int flagsLen = flags.Length;
			string[] names = new string[flagsLen];
			for (int x = 0; x < flagsLen; x++) {
				names[x] = flags[x].ToString().ToReadable();
			}

			fold = EditorGUILayout.BeginFoldoutHeaderGroup(fold, label);
			Rect foldoutPosition = GUILayoutUtility.GetLastRect();
			foldoutPosition.x += EditorGUIUtility.labelWidth + 1;
			EditorGUI.LabelField(foldoutPosition, new GUIContent($"{flagsLen} Value{(flagsLen == 1 ? "" : "s")}", flagsLen > 0 ? string.Join('\n', names) : ""));

			if (fold) {
				int beforeIndent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = beforeIndent + 2;
				for (int x = 0; x < flagsLen; x++) {
					EditorGUILayout.LabelField(names[x]);
				}
				EditorGUI.indentLevel = beforeIndent;
			}

			EditorGUILayout.EndFoldoutHeaderGroup();
		}

	}

}
#endif