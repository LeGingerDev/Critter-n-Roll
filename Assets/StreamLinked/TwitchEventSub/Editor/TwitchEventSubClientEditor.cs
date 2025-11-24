#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	using ScoredProductions.StreamLinked.EventSub;
	using ScoredProductions.StreamLinked.EventSub.Interfaces;
	using ScoredProductions.StreamLinked.EventSub.WebSocketMessages;
	using ScoredProductions.StreamLinked.LightJson.Serialization;

	using UnityEditor;

	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UIElements;

	[CustomEditor(typeof(TwitchEventSubClient))]
	public class TwitchEventSubClientEditor : EditorPaged {

		private readonly List<FieldInfo> _twitchEventSubClientFIEventSubEvents = new List<FieldInfo>();
		private readonly List<FieldInfo> _twitchEventSubClientFISystemEvents = new List<FieldInfo>();

		private TwitchEventSubClient source;

		private string eventSearch = string.Empty;
		private Rect searchRect;

		private SerializedProperty LogDebugLevelSP;
		private SerializedProperty PersistBetweenScenesSP;
		private SerializedProperty EventSubTokenSP;
		private SerializedProperty KeepaliveTimeoutSecondsSP;

		public override VisualElement CreateInspectorGUI() {
			this.LogDebugLevelSP = this.serializedObject.FindProperty(nameof(TwitchEventSubClient.LogDebugLevel));
			this.PersistBetweenScenesSP = this.serializedObject.FindProperty("persistBetweenScenes");
			this.EventSubTokenSP = this.serializedObject.FindProperty("eventSubToken");
			this.KeepaliveTimeoutSecondsSP = this.serializedObject.FindProperty(nameof(TwitchEventSubClient.KeepaliveTimeoutSeconds));

			this._twitchEventSubClientFIEventSubEvents.Clear();
			this._twitchEventSubClientFISystemEvents.Clear();
			foreach (FieldInfo fi in typeof(TwitchEventSubClient).GetFields()) {
				Type fieldType = fi.FieldType;
				if (fieldType.IsGenericType
					&& fieldType.GetGenericTypeDefinition() == typeof(UnityEvent<>)) {
					if (typeof(IEvent).IsAssignableFrom(fieldType.GetGenericArguments()[0])) {
						this._twitchEventSubClientFIEventSubEvents.Add(fi);
					} else if (fieldType.GetGenericArguments()[0] == typeof(Subscription)) {
						this._twitchEventSubClientFISystemEvents.Add(fi);
					}
				}
			}

			this.HideSeperatorLine = false;

			if (this.target is TwitchEventSubClient s) {
				this.source = s;
			}

			this.Pages.Clear();
			this.Pages.TryAdd(0, new Page("Main Page", () => {
				using (new EditorGUI.DisabledScope(true)) {
					EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(this.source), this.GetType(), false);

					EditorGUILayout.ObjectField("Object", this.source.gameObject, typeof(GameObject), true);
				}

				EditorGUILayout.PropertyField(this.LogDebugLevelSP);
				EditorGUILayout.PropertyField(this.PersistBetweenScenesSP);
				EditorGUILayout.PropertyField(this.EventSubTokenSP);
				EditorGUILayout.PropertyField(this.KeepaliveTimeoutSecondsSP);

				GUILayout.Space(5);
				EditorGUILayout.LabelField(new GUIContent("Socket State"), new GUIContent(source.SocketState.ToString()));
				EditorGUILayout.LabelField(new GUIContent("Known Max Total Cost"), new GUIContent(source.KnownMaxTotalCost.ToString()));
				EditorGUILayout.LabelField(new GUIContent("Used Cost"), new GUIContent(source.UsedCost.ToString()));
				EditorGUILayout.HelpBox(source.CurrentSessionState.HasValue ? JsonWriter.Serialize(source.CurrentSessionState.Value, true) : "", MessageType.None);
				GUILayout.Space(5);

				if (this.serializedObject.ApplyModifiedProperties()) {
					EditorUtility.SetDirty(this.source);
					this.serializedObject.UpdateIfRequiredOrScript();
				}
			}));

			this.Pages.TryAdd(1, new Page("EventSub Events", () => {
				this.serializedObject.Update();

				this.eventSearch = EditorGUILayout.TextField(this.eventSearch);
				if (Event.current.type == EventType.Repaint) {
					this.searchRect = GUILayoutUtility.GetLastRect();
				}
				if (string.IsNullOrWhiteSpace(this.eventSearch)) {
					EditorGUI.LabelField(this.searchRect, "Search", EditorStyles.centeredGreyMiniLabel);
				}

				string bufferedValue = this.eventSearch.Replace(" ", "");

				foreach (FieldInfo a in this._twitchEventSubClientFIEventSubEvents) {
					if (a.Name.Contains(bufferedValue, StringComparison.CurrentCultureIgnoreCase)) {
						EditorGUILayout.PropertyField(this.serializedObject.FindProperty(a.Name), true);
					}
				}

				this.serializedObject.ApplyModifiedProperties();
			}));

			this.Pages.TryAdd(2, new Page("System Events", () => {
				this.serializedObject.Update();

				foreach (FieldInfo a in this._twitchEventSubClientFISystemEvents) {
					EditorGUILayout.PropertyField(this.serializedObject.FindProperty(a.Name), true);
				}

				this.serializedObject.ApplyModifiedProperties();
			}));

			this.Pages.TryAdd(3, new Page("Subscription Info", () => {
				GUILayout.Space(10);
				List<Subscription> subs = source.GetSubscriptions;
				EditorGUILayout.LabelField("Registered Subscriptions:");
				if (subs.Count > 0) {
					foreach (Subscription sub in subs) {
						EditorGUILayout.HelpBox(JsonWriter.Serialize(sub, true), MessageType.None);
					}
				}
				else {
					EditorGUILayout.HelpBox("No Subscriptions found.", MessageType.None);
				}
			}));
			return base.CreateInspectorGUI();
		}
	}
}
#endif