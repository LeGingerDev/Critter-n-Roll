#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	using ScoredProductions.StreamLinked.API;
	using ScoredProductions.StreamLinked.API.AuthContainers;
	using ScoredProductions.StreamLinked.API.Scopes;
	using ScoredProductions.StreamLinked.API.SharedContainers;
	using ScoredProductions.StreamLinked.LightJson;
	using ScoredProductions.StreamLinked.LightJson.Serialization;
	using ScoredProductions.StreamLinked.Utility;

	using UnityEditor;

	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UIElements;

	[CustomEditor(typeof(TwitchAPIClient))]
	public class TwitchAPIClientEditor : EditorPaged {

		private static GUIStyle lineStyle;
		private static GUILayoutOption[] lineLayoutOptions;

		private TwitchAPIClient source;

		private GUIStyle BoldLabel;

		private FieldInfo[] _twitchAPIClientFI;

		private bool showAuthButtons;

		private bool IDSecretFoldout;

		private bool BodyUpdated;
		private JsonObject ParsedTokens;

		private TokenInstance tokenBody;

		private string TokenSearch;

		private bool TokenCheck;
		private bool CheckedPrefs;
		private string GetUserResult;

		private TwitchAPIClassEnumDropdownProvider _dropdown;
		private TwitchAPIClassEnum requestType = TwitchAPIClassEnum.GetUsers;
		private string[] requestTypeEnumNames;
		private Rect dropdownRect;

		private Type typeBody;
		private List<PropertyInfo> piStaticGets;
		private List<PropertyInfo> piInstanceProperties;
		private List<PropertyInfo> piRequiredProperties;
		private object activatedBody;
		private bool jsonReq;

		private bool scopeFoldout;
		private readonly Dictionary<string, string> requestFields = new Dictionary<string, string>();
		public string jsonToSend;
		public Vector2 scrollResult;
		public bool wordWrap;

		public Type iSharedType;

		private readonly GUILayoutOption BoxHeight = GUILayout.Height(100);

		private readonly Dictionary<int, bool> foldoutIDs = new Dictionary<int, bool>();
		private int foldoutNextID = 0;

		private SerializedProperty LogDebugLevelSP;
		private SerializedProperty PersistBetweenScenesSP;
		private SerializedProperty UsePlayerPrefsFirstSP;
		private SerializedProperty DefaultAPITokenSP;
		private SerializedProperty AuthWebserverActiveTimeSP;

		public override VisualElement CreateInspectorGUI() {
			lineStyle ??= new GUIStyle("ObjectField") { fixedHeight = 1 };
			lineLayoutOptions ??= new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) };

			this.BoldLabel = new GUIStyle("BoldLabel") { alignment = TextAnchor.MiddleCenter };
			this.OnDropdownOptionSelected((int)TwitchAPIClassEnum.GetUsers);
			this._dropdown = new TwitchAPIClassEnumDropdownProvider(this.OnDropdownOptionSelected);
			this._twitchAPIClientFI = typeof(TwitchAPIClient).GetFields();
			this.requestTypeEnumNames = Enum.GetNames(typeof(TwitchAPIClassEnum));
			this.requestTypeEnumNames.MakeReadable();
			this.iSharedType = typeof(IShared);

			this.LogDebugLevelSP = this.serializedObject.FindProperty(nameof(TwitchAPIClient.LogDebugLevel));
			this.PersistBetweenScenesSP = this.serializedObject.FindProperty("persistBetweenScenes");
			this.UsePlayerPrefsFirstSP = this.serializedObject.FindProperty(nameof(TwitchAPIClient.UsePlayerPrefsFirst));
			this.DefaultAPITokenSP = this.serializedObject.FindProperty(nameof(TwitchAPIClient.DefaultAPIToken));
			this.AuthWebserverActiveTimeSP = this.serializedObject.FindProperty(nameof(TwitchAPIClient.AuthWebserverActiveTime));

			this.HideSeperatorLine = false;

			if (this.target is TwitchAPIClient s) {
				this.source = s;
			}

			this.Pages.Clear();
			this.Pages.TryAdd(0, new Page("Settings", () => {
				using (new EditorGUI.DisabledScope(true)) {
					EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(this.source), this.target.GetType(), false);

					EditorGUILayout.ObjectField("Object", this.source.gameObject, typeof(GameObject), true);
				}
				EditorGUILayout.PropertyField(this.LogDebugLevelSP);
				EditorGUILayout.PropertyField(this.PersistBetweenScenesSP);

				this.IDSecretFoldout = EditorGUILayout.Foldout(this.IDSecretFoldout, new GUIContent("Application Credentials", "Twitch Application credentials needed to aquire OAuth tokens. Keep hidden when not editing."));
				if (this.IDSecretFoldout) {
					EditorGUI.indentLevel++;
					string delayedClientID = EditorGUILayout.DelayedTextField("Client ID", this.source.TwitchClientID);
					string delayedSecret = EditorGUILayout.DelayedTextField("Twitch Secret", this.source.TwitchSecret);

					bool storeHasTokens = source.CheckStoreHasTokens();

					if ((this.source.TwitchClientID != delayedClientID || this.source.TwitchSecret != delayedSecret)
						&& (!storeHasTokens || EditorUtility.DisplayDialog("Credentials Changed", "Updating the Twitch credentials invalidates all stored OAuth tokens. Do you want to do this?", "Yes", "No"))) {
						this.source.UpdateClientIDAndSecret(delayedClientID, delayedSecret, storeHasTokens);
						this.BodyUpdated = false;
						EditorUtility.SetDirty(this.source);
					}

					EditorGUILayout.PropertyField(this.UsePlayerPrefsFirstSP);
					EditorGUI.indentLevel--;
				}

				EditorGUILayout.PropertyField(this.DefaultAPITokenSP);
				EditorGUILayout.PropertyField(this.AuthWebserverActiveTimeSP);

				if (this.source.GettingNewToken) {
					EditorGUILayout.LabelField("Client currently retreiving a new Auth Token.");
				}
				else {

					GUILayout.Space(10);

					this.showAuthButtons = EditorGUILayout.Foldout(this.showAuthButtons, "PlayerPrefs shortcuts");
					if (this.showAuthButtons) {
						if (GUILayout.Button("Load Client ID and Secret from PlayerPrefs")) {
							this.source.LoadClientID();
							this.source.LoadClientSecret();
						}

						if (GUILayout.Button("Save Client ID and Secret to PlayerPrefs")) {
							this.source.SaveCurrentToPlayerPrefs();
						}

						if (GUILayout.Button("Clear Client ID and Secret from PlayerPrefs")) {
							this.source.ClearCurrentPlayerPrefs();
						}

						if (GUILayout.Button("Clear Tokens from PlayerPrefs")) {
							this.source.CleanPlayerPrefTokens();
							this.BodyUpdated = false;
						}
					}
				}

				if (this.serializedObject.ApplyModifiedProperties()) {
					EditorUtility.SetDirty(this.source);
					this.serializedObject.UpdateIfRequiredOrScript();
				}
			}));

			this.Pages.TryAdd(1, new Page("Events", () => {
				this.serializedObject.Update();

				foreach (FieldInfo a in this._twitchAPIClientFI) {
					if (a.FieldType == typeof(UnityEvent)
						|| a.FieldType.IsGenericType
							&& a.FieldType.GetGenericTypeDefinition() == typeof(UnityEvent<>)
							&& a.FieldType.GetGenericArguments()[0] == typeof(Exception)) {
						EditorGUILayout.PropertyField(this.serializedObject.FindProperty(a.Name), true);
					}
				}

				this.serializedObject.ApplyModifiedProperties();
			}));

			this.Pages.TryAdd(2, new Page("Read Tokens", () => {
				this.TokenSearch = EditorGUILayout.TextField(this.TokenSearch);

				if (this.ParsedTokens == null || !this.BodyUpdated) {
					if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchAuthenticationTokens, out string tokens)) {
						this.ParsedTokens = JsonReader.Parse(tokens);
						this.BodyUpdated = true;
					}
					else {
						this.ParsedTokens = new JsonObject {
							{ TwitchWords.CLIENT_ID, this.source.TwitchClientID },
							{ TwitchWords.CLIENT_SECRET, this.source.TwitchSecret },
							{TwitchWords.DATA, new JsonArray() }
						};
						InternalSettingsStore.EditSetting(SavedSettings.TwitchAuthenticationTokens, this.ParsedTokens.ToString());
					}
				}

				JsonValue obj = this.ParsedTokens[TwitchWords.DATA];
				JsonArray container;
				if (obj == JsonValue.Null || !obj.IsJsonArray) {
					container = this.ParsedTokens[TwitchWords.DATA] = new JsonArray();
					InternalSettingsStore.EditSetting(SavedSettings.TwitchAuthenticationTokens, this.ParsedTokens.ToString());
				}
				else {
					container = obj;
				}

				foreach (JsonValue x in container) {
					string id = x[TwitchWords.ID].AsString;
					GUIContent deleteContent = new GUIContent("Delete", "Deletes the token displayed above this button.");
					if (string.IsNullOrWhiteSpace(this.TokenSearch) || id.Contains(this.TokenSearch, StringComparison.InvariantCultureIgnoreCase)) {
						EditorGUILayout.HelpBox(x.ToString(true), MessageType.None);
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Find")) {
							bool found = false;
							foreach (string guid in AssetDatabase.FindAssets($"t:{nameof(TokenInstance)}")) {
								string path = AssetDatabase.GUIDToAssetPath(guid);
								if (!string.IsNullOrWhiteSpace(path)) {
									TokenInstance ti = AssetDatabase.LoadAssetAtPath<TokenInstance>(path);
									if (ti.TokenID == id) {
										found = true;
										EditorGUIUtility.PingObject(ti);
										break;
									}
								}
							}
							if (!found) {
								DebugManager.LogMessage($"TokenInstance {{{id}}} not found in project, please delete this token if no instances exist any more.", DebugManager.ErrorLevel.Warning);
							}
						}
						if (GUILayout.Button(deleteContent)) {
							this.source.CleanPlayerPrefTokens(true, id);
							this.BodyUpdated = false;
						}
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Space(5);
					}
				}
			}));

			this.Pages.TryAdd(3, new Page("Endpoint Browser", () => {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("API Endpoint", GUILayout.Width(EditorGUIUtility.labelWidth - 1));
				if (GUILayout.Button(new GUIContent(this.requestTypeEnumNames[(int)this.requestType]), EditorStyles.popup)) {
					this._dropdown.Show(this.dropdownRect);
				}
				if (Event.current.type == EventType.Repaint) {
					this.dropdownRect = GUILayoutUtility.GetLastRect();
				}
				EditorGUILayout.EndHorizontal();

				if (this.piInstanceProperties.Count > 0) {
					GUILayout.Space(10);
					GUILayout.Box(string.Empty, lineStyle, lineLayoutOptions);
					EditorGUILayout.LabelField("Returned Values", this.BoldLabel);
					foreach (PropertyInfo a in this.piInstanceProperties) {
						if (iSharedType.IsAssignableFrom(a.PropertyType)) {
							this.ProduceEditor(a.PropertyType, new GUIContent(a.Name), false);
						}
						else if (a.PropertyType.IsArray && iSharedType.IsAssignableFrom(a.PropertyType.GetElementType())) {
							this.ProduceEditor(a.PropertyType.GetElementType(), new GUIContent(a.Name), true);
						}
						else {
							EditorGUILayout.LabelField(new GUIContent(a.Name), new GUIContent(a.PropertyType.Name));
						}

					}
					foldoutNextID = 0;
				}

				if (this.piRequiredProperties.Count > 0) {
					GUILayout.Space(10);
					GUILayout.Box(string.Empty, lineStyle, lineLayoutOptions);
					EditorGUILayout.LabelField("Request Endpoint Requirements", this.BoldLabel);
					foreach (PropertyInfo a in this.piRequiredProperties) {
						object propertyValue = a.GetValue(this.activatedBody);
						GUIContent nameLabel = new GUIContent(a.Name);
						Type propertyType = propertyValue.GetType();
						if (propertyType.IsArray && propertyType.GetElementType() == typeof(TwitchScopesEnum)) {
							TwitchScopesEnum[] array = (TwitchScopesEnum[])propertyValue;
							int len = array.Length;

							if (len > 0) {
								FlaggedEnumPropertyDrawer.DrawReadonlyView(array, nameLabel, ref scopeFoldout);
							}
							else {
								nameLabel.tooltip = "Be careful, Twitch might add or remove scopes without notice, those changes wouldnt be reflected here until found and updated.";
								EditorGUILayout.LabelField(nameLabel, new GUIContent("None Listed"));
							}
						}
						else {
							EditorGUILayout.LabelField(nameLabel, new GUIContent(propertyValue.ToString()));
						}

					}
				}

				if (this.jsonReq || piStaticGets.Count > 0) {
					GUILayout.Space(10);
					GUILayout.Box(string.Empty, lineStyle, lineLayoutOptions);
					EditorGUILayout.LabelField("Request Parameters", this.BoldLabel);
					foreach (PropertyInfo a in this.piStaticGets) {
						EditorGUILayout.LabelField(a.Name, a.GetValue(null).ToString());
					}
					if (this.jsonReq) {
						EditorGUILayout.LabelField("Json Body Required", "Yes");
					}
				}

			}));

			this.Pages.TryAdd(4, new Page("Quick Requests", () => {
				TokenInstance before = this.tokenBody;
				this.tokenBody = (TokenInstance)EditorGUILayout.ObjectField("Token", this.tokenBody, typeof(TokenInstance), false);
				if (this.tokenBody != before) {
					this.CheckedPrefs = false;
					this.GetUserResult = null;
					this.TokenCheck = false;
				}

				if (this.tokenBody != null) {

					EditorGUILayout.HelpBox(this.tokenBody.RetrieveTokenAsJson().ToString(true), MessageType.None);

					bool callDisallowed = this.tokenBody.RequestScopes.Count == 0 || this.source.CheckTokenIsInQueue(this.tokenBody);
					EditorGUILayout.BeginHorizontal();
					using (new EditorGUI.DisabledScope(callDisallowed)) {
						if (GUILayout.Button(new GUIContent("Aquire New Token", ""), GUILayout.Width(250))) {
							this.source.GetNewAuthToken(this.tokenBody);
						}
					}

					if (this.source.GettingNewToken) {
						GUILayout.FlexibleSpace();

						if (GUILayout.Button(new GUIContent("Cancel", ""), GUILayout.Width(250))) {
							this.source.CancelAPIRequestsAndReset();
						}
					}
					EditorGUILayout.EndHorizontal();

					if (!this.source.GettingNewToken && this.tokenBody.HasToken) {
						GUILayout.Space(10);
						GUILayout.Box(string.Empty, lineStyle, lineLayoutOptions);
						EditorGUILayout.LabelField(new GUIContent("Make Request to Twitch API*", "This is an experimental feature to make Twitch API requests from the Inspector, attempt has been made to be compatible with all request types however this cannot be guarenteed."), this.BoldLabel);

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("API Endpoint", GUILayout.Width(EditorGUIUtility.labelWidth - 1));
						if (GUILayout.Button(new GUIContent(this.requestTypeEnumNames[(int)this.requestType]), EditorStyles.popup)) {
							this._dropdown.Show(this.dropdownRect);
						}
						if (Event.current.type == EventType.Repaint) {
							this.dropdownRect = GUILayoutUtility.GetLastRect();
						}
						EditorGUILayout.EndHorizontal();

						for (int x = 0; x < this.piStaticGets.Count; x++) {
							string presentingName = this.piStaticGets[x].Name;
							if (presentingName != nameof(TwitchWords.CONTENT_TYPE)) {
								string a = (string)this.piStaticGets[x].GetValue(this.activatedBody);
								this.requestFields[a] = EditorGUILayout.TextField(presentingName, this.requestFields[a]);
							}
						}

						if (this.jsonReq) {
							EditorGUILayout.LabelField("JSON to send");
							this.jsonToSend = EditorGUILayout.TextArea(this.jsonToSend, this.BoxHeight);
						}

						if (GUILayout.Button(new GUIContent("Send", ""), GUILayout.Width(250))) {
							string endpoint = (string)this.piRequiredProperties.Find(pi => pi.Name == nameof(ITwitchAPIDataObject.Endpoint)).GetValue(this.activatedBody);
							TwitchAPIRequestMethod method = (TwitchAPIRequestMethod)this.piRequiredProperties.Find(pi => pi.Name == nameof(ITwitchAPIDataObject.HTTPMethod)).GetValue(this.activatedBody);

							List<(string, string)> queryParams = new List<(string, string)>(this.piStaticGets.Count);
							(string, string)[] additionalHeaders = null;

							for (int x = 0; x < this.piStaticGets.Count; x++) {
								string presentingName = this.piStaticGets[x].Name;
								if (presentingName == nameof(TwitchWords.CONTENT_TYPE)) {
									additionalHeaders = new (string, string)[] { ((string, string))this.piStaticGets.Find(pi => pi.Name == nameof(TwitchWords.CONTENT_TYPE)).GetValue(this.activatedBody) };
								}
								else {
									string a = (string)this.piStaticGets[x].GetValue(this.activatedBody);
									if (!string.IsNullOrWhiteSpace(this.requestFields[a])) {
										queryParams.Add((a, this.requestFields[a]));
									}
								}
							}

							JsonValue body = TwitchAPIClient.MakeTwitchAPIRequestJson(endpoint, method, 10000, this.tokenBody, additionalHeaders, queryParams.ToArray(), this.jsonToSend);

							this.GetUserResult = body.ToString(true);
						}
					}
					else {
						EditorGUILayout.BeginHorizontal();
						if (GUILayout.Button(new GUIContent("Check PlayerPrefs", "The OAuth token is not a serialized value, if one exists its stored in PlayerPrefs for persistent storage even in builds."), GUILayout.Width(250))) {
							this.TokenCheck = this.tokenBody.LoadTokenFromSettings(this.source.LogAny);
							this.CheckedPrefs = true;
						}
						if (this.CheckedPrefs && !this.TokenCheck) {
							EditorGUILayout.LabelField("Token not found in PlayerPrefs");
						}
						EditorGUILayout.EndHorizontal();
					}

					if (!string.IsNullOrWhiteSpace(this.GetUserResult)) {
						int newLines = 0;
						for (int x = 0; x < this.GetUserResult.Length; x++) {
							if (this.GetUserResult[x] == '\n') {
								newLines++;
							}
						}
						EditorGUILayout.LabelField(new GUIContent("Request Result:"));
						EditorGUILayout.SelectableLabel(this.GetUserResult, new GUIStyle("CurveEditorBackground") { wordWrap = this.wordWrap }, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * newLines));
						this.wordWrap = EditorGUILayout.ToggleLeft("Word Wrap", this.wordWrap);
					}

				}
			}));

			return base.CreateInspectorGUI();
		}

		private void OnDropdownOptionSelected(int index) {
			this.foldoutNextID = 0;
			this.foldoutIDs.Clear();

			this.jsonToSend = string.Empty;
			this.requestFields.Clear();
			this.requestType = (TwitchAPIClassEnum)index;
			this.typeBody = this.requestType.GetAPIClass();
			this.piStaticGets = new List<PropertyInfo>(this.typeBody.GetProperties(BindingFlags.Static | BindingFlags.Public));
			this.piInstanceProperties = new List<PropertyInfo>(this.typeBody.GetProperties(BindingFlags.Public | BindingFlags.Instance));
			this.piRequiredProperties = this.piInstanceProperties.FindAll(pi => !pi.CanWrite);
			this.piInstanceProperties = this.piInstanceProperties.FindAll(pi => pi.CanWrite); // Replace with writables
			this.activatedBody = Activator.CreateInstance(this.typeBody);
			this.jsonReq = typeof(IJsonRequest).IsAssignableFrom(typeBody);

			for (int x = 0; x < this.piStaticGets.Count; x++) {
				if (this.piStaticGets[x].Name != nameof(TwitchWords.CONTENT_TYPE)) {
					if (this.piStaticGets[x].GetValue(this.activatedBody) is string value) {
						this.requestFields.Add(value, "");
					}
				}
			}
		}

		private void ProduceEditor(Type value, GUIContent nameLabel, bool wasArray) {
			int x = foldoutNextID++;
			foldoutIDs.TryAdd(x, false);
			foldoutIDs[x] = EditorGUILayout.Foldout(foldoutIDs[x], "");
			EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(nameLabel.text), new GUIContent((value.Name + (wasArray ? "[]" : "")).RichTextColour("white")), new GUIStyle("label") { richText = true });

			if (foldoutIDs[x]) {

				EditorGUI.indentLevel++;

				foreach (PropertyInfo pi in value.GetProperties()) {
					GUIContent label = new GUIContent(pi.Name);
					Type declaredType = pi.PropertyType;
					if (iSharedType.IsAssignableFrom(declaredType)) {
						this.ProduceEditor(declaredType, label, false);
					}
					else if (declaredType.IsArray && iSharedType.IsAssignableFrom(declaredType.GetElementType())) {
						this.ProduceEditor(declaredType.GetElementType(), new GUIContent(declaredType.Name), true);
					}
					else {
						EditorGUILayout.LabelField(label, new GUIContent(declaredType.Name));
					}
				}

				EditorGUI.indentLevel--;
			}
		}
	}
}
#endif