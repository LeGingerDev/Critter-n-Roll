#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System;
	using System.Collections.Generic;

	using UnityEditor;
	using UnityEngine;

	public abstract class EditorPaged : Editor {

		protected readonly Dictionary<int, Page> Pages = new Dictionary<int, Page>();

		protected int SelectedKey;

		protected string PopupName = "Page";

		protected string[] drowdownNames = new string[0];
		protected int[] dropdownValues = new int[0];

		protected Action LoadBefore;
		protected Action LoadAfter;

		protected int SpaceAfterSelector = 0;

		protected bool HideSeperatorLine = true;

		private static GUIStyle lineStyle;
		private static GUILayoutOption[] lineLayoutOptions;

		public sealed override void OnInspectorGUI() {
			this.LoadBefore?.Invoke();

			if (this.Pages.Count > 0) {
				Array.Resize(ref this.drowdownNames, this.Pages.Count);
				Array.Resize(ref this.dropdownValues, this.Pages.Count);

				if (this.SelectedKey >= this.dropdownValues.Length) {
					this.SelectedKey = 0;
				}

				int count = 0;
				foreach (int key in this.Pages.Keys) {
					this.dropdownValues[count++] = key;
				}
				Array.Sort(this.dropdownValues);
				count = 0;
				for (; count < this.Pages.Count; count++) {
					this.drowdownNames[count] = this.Pages[this.dropdownValues[count]].Name;
				}

				// Popup drop down returns an int
				this.SelectedKey = EditorGUILayout.Popup(this.PopupName, this.SelectedKey, this.drowdownNames);

				if (!this.HideSeperatorLine) {
					EditorGUILayout.Space(1);
					GUILayout.Box(string.Empty, lineStyle ??= new GUIStyle("minibutton") { fixedHeight = 1}, lineLayoutOptions ??= new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
					EditorGUILayout.Space(1);
				}

				if (this.SpaceAfterSelector > 0) {
					EditorGUILayout.Space(this.SpaceAfterSelector);
				}

				//Selected int gets the dictionary value
				if (this.Pages.TryGetValue(this.dropdownValues[this.SelectedKey], out Page page)) {
					//Execute code that displays the page
					page.Content?.Invoke();
				} else {
					this.SelectedKey = 0;
				}
			} else {
				this.DrawDefaultInspector();
			}

			this.LoadAfter?.Invoke();
		}

		protected class Page {
			public Action Content;
			public string Name;

			public Page(string Name, Action Content) {
				this.Name = Name;
				this.Content = Content;
			}
		}

	}
}
#endif