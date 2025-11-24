#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System.Reflection;

	using ScoredProductions.StreamLinked.IRC;
	using ScoredProductions.StreamLinked.LightJson.Serialization;

	using UnityEditor;

	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UIElements;

	[CustomEditor(typeof(TwitchIRCClient))]
	public class TwitchIRCClientEditor : EditorPaged {

		private string targetString;

		private FieldInfo[] _twitchIRCClientFI;

		private SerializedProperty LogDebugLevelSP;
		private SerializedProperty PersistBetweenScenesSP;
		private SerializedProperty IRCTokenSP;
		private SerializedProperty SaveTargetToPlayerPrefsSP;
		private SerializedProperty OverwriteFromInternalSettingsSP;
		private SerializedProperty CommandsEnabledSP;
		private SerializedProperty MembershipEnabledSP;
		private SerializedProperty TagsEnabledSP;
		private SerializedProperty SSLConnectionSP;
		private SerializedProperty UseAsyncToReadSP;

		private TwitchIRCClient source;

		public override VisualElement CreateInspectorGUI() {
			this._twitchIRCClientFI = typeof(TwitchIRCClient).GetFields();

			this.LogDebugLevelSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.LogDebugLevel));
			this.PersistBetweenScenesSP = this.serializedObject.FindProperty("persistBetweenScenes");
			this.IRCTokenSP = this.serializedObject.FindProperty("ircToken");
			this.SaveTargetToPlayerPrefsSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.SaveTargetToPlayerPrefs));
			this.OverwriteFromInternalSettingsSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.OverwriteFromInternalSettings));
			this.CommandsEnabledSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.CommandsEnabled));
			this.MembershipEnabledSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.MembershipEnabled));
			this.TagsEnabledSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.TagsEnabled));
			this.SSLConnectionSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.SSLConnection));
			this.UseAsyncToReadSP = this.serializedObject.FindProperty(nameof(TwitchIRCClient.UseAsyncToRead));

			this.HideSeperatorLine = false;

			if (this.target is TwitchIRCClient s) {
				this.source = s;
			}

			this.targetString = this.source.TwitchTarget ?? string.Empty;

			this.Pages.Clear();
			this.Pages.TryAdd(0, new Page("Settings", () => {
				using (new EditorGUI.DisabledScope(true)) {
					EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(this.source), this.target.GetType(), false);

					EditorGUILayout.ObjectField("Object", this.source.gameObject, typeof(GameObject), true);
				}

				if (EditorApplication.isPlayingOrWillChangePlaymode) {
					if (GUILayout.Button(this.source.IRCEnabled ? "Disconnect" : "Connect", GUILayout.Width(250))) {
						this.source.IRCEnabled = !this.source.IRCEnabled;
						EditorUtility.SetDirty(this.source);
					}
				} else {
					EditorGUILayout.PropertyField(this.serializedObject.FindProperty("ircEnabled"), new GUIContent("Connect IRC on Start", "Toggle if functionality should start immediatly for this object."));
				}

				EditorGUILayout.PropertyField(this.LogDebugLevelSP);
				EditorGUILayout.PropertyField(this.PersistBetweenScenesSP);

				EditorGUILayout.Space(5);

				EditorGUILayout.LabelField(new GUIContent("Current channel target:"), new GUIContent(this.source.TwitchTarget));

				EditorGUILayout.BeginHorizontal();
				using (new EditorGUI.DisabledScope((string.IsNullOrWhiteSpace(this.targetString) && string.IsNullOrWhiteSpace(this.source.TwitchTarget)) || this.targetString == this.source.TwitchTarget)) {
					if (GUILayout.Button(string.IsNullOrWhiteSpace(this.targetString) ? "Clear Target" : "Switch Target", GUILayout.Width(EditorGUIUtility.labelWidth - 1))) {
						this.source.TwitchTarget = this.targetString;
						this.targetString = null;
						EditorUtility.SetDirty(this.source);
					}
				}
				this.targetString = GUILayout.TextField(this.targetString)?.Trim() ?? string.Empty;
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space(5);

				using (new EditorGUI.DisabledScope(this.source.IsConnectingOrConnected)) {
					EditorGUILayout.PropertyField(this.IRCTokenSP, new GUIContent("Authentication Token", "OAuth token credentials to log in with."));
					EditorGUILayout.PropertyField(this.SaveTargetToPlayerPrefsSP);
					EditorGUILayout.PropertyField(this.OverwriteFromInternalSettingsSP);
					EditorGUILayout.PropertyField(this.CommandsEnabledSP);
					EditorGUILayout.PropertyField(this.MembershipEnabledSP);
					EditorGUILayout.PropertyField(this.TagsEnabledSP);
					EditorGUILayout.PropertyField(this.SSLConnectionSP);
					EditorGUILayout.PropertyField(this.UseAsyncToReadSP);
				}

				if (this.serializedObject.ApplyModifiedProperties()) {
					EditorUtility.SetDirty(this.source);
					this.serializedObject.UpdateIfRequiredOrScript();
				}
			}));

			this.Pages.TryAdd(1, new Page("Events", () => {
				this.serializedObject.Update();

				foreach (FieldInfo a in this._twitchIRCClientFI) {
					if (a.FieldType.IsGenericType
						&& a.FieldType.GetGenericTypeDefinition() == typeof(UnityEvent<>)
						&& a.FieldType.GetGenericArguments()[0] == typeof(TwitchMessage)) {
						EditorGUILayout.PropertyField(this.serializedObject.FindProperty(a.Name), true);
					}
				}
				this.serializedObject.ApplyModifiedProperties();
			}));


			this.Pages.TryAdd(2, new Page("All Messages", () => {
				foreach (TwitchMessage msg in this.source.AllMessages) {
					EditorGUILayout.HelpBox(JsonWriter.Serialize(msg, true), MessageType.None);
				}
			}));


			return base.CreateInspectorGUI();
		}
	}
}
#endif