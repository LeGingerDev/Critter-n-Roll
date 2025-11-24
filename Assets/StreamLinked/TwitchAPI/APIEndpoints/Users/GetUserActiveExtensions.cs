using System;
using System.Collections.Generic;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Users {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-active-extensions">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetUserActiveExtensions : IUsers, ISerializationCallbackReceiver {

		public Dictionary<int, Map> panel { get; set; }
		public Dictionary<int, Map> overlay { get; set; }
		public Dictionary<int, Map> component { get; set; }

		[HideInInspector, SerializeField]
		private int[] panel_keys; // ISerializationCallbackReceiver
		[HideInInspector, SerializeField]
		private Map[] panel_values; // ISerializationCallbackReceiver

		[HideInInspector, SerializeField]
		private int[] overlay_keys; // ISerializationCallbackReceiver
		[HideInInspector, SerializeField]
		private Map[] overlay_values; // ISerializationCallbackReceiver

		[HideInInspector, SerializeField]
		private int[] component_keys; // ISerializationCallbackReceiver
		[HideInInspector, SerializeField]
		private Map[] component_values; // ISerializationCallbackReceiver

		public void Initialise(JsonValue body) {
			JsonValue panel = body[TwitchWords.PANEL];
			if (!panel.IsNull) {
				this.panel = new Dictionary<int, Map>();
				JsonObject panelObject = panel.AsJsonObject;
				int x = 1;
				while (panelObject.TryGetKey(x.ToString(), out JsonValue panelValue)) {
					this.panel[x++] = new Map(panelValue);
					x++;
				}
			}
			JsonValue overlay = body[TwitchWords.OVERLAY];
			if (!overlay.IsNull) {
				this.overlay = new Dictionary<int, Map>();
				JsonObject overlayObject = overlay.AsJsonObject;
				int x = 1;
				while (overlayObject.TryGetKey(x.ToString(), out JsonValue overlayValue)) {
					this.overlay[x++] = new Map(overlayValue);
					x++;
				}
			}
			JsonValue component = body[TwitchWords.COMPONENT];
			if (!component.IsNull) {
				this.component = new Dictionary<int, Map>();
				JsonObject componentObject = component.AsJsonObject;
				int x = 1;
				while (componentObject.TryGetKey(x.ToString(), out JsonValue componentValue)) {
					this.component[x++] = new Map(componentValue);
					x++;
				}
			}
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetExtensions;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetUserActiveExtensions;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_manage_extensions,
			TwitchScopesEnum.user_edit_broadcast,
			TwitchScopesEnum.user_read_broadcast,
		};

		public static string USER_ID => TwitchWords.USER_ID;

		public void OnBeforeSerialize() { // ISerializationCallbackReceiver
			this.panel_keys = new int[this.panel.Count];
			this.panel_values = new Map[this.panel.Count];
			this.overlay_keys = new int[this.overlay.Count];
			this.overlay_values = new Map[this.overlay.Count];
			this.component_keys = new int[this.component.Count];
			this.component_values = new Map[this.component.Count];

			int counter = 0;
			foreach (KeyValuePair<int, Map> pair in this.panel) {
				this.panel_keys[counter] = pair.Key;
				this.panel_values[counter++] = pair.Value;
			}
			counter = 0;
			foreach (KeyValuePair<int, Map> pair in this.overlay) {
				this.overlay_keys[counter] = pair.Key;
				this.overlay_values[counter++] = pair.Value;
			}
			counter = 0;
			foreach (KeyValuePair<int, Map> pair in this.component) {
				this.component_keys[counter] = pair.Key;
				this.component_values[counter++] = pair.Value;
			}
		}

		public void OnAfterDeserialize() { // ISerializationCallbackReceiver
			int panelLen = this.panel_keys?.Length ?? 0;
			int overlayLen = this.overlay_keys?.Length ?? 0;
			int componentLen = this.component_keys?.Length ?? 0;

			this.panel ??= new Dictionary<int, Map>(panelLen);
			this.panel.Clear();
			this.overlay ??= new Dictionary<int, Map>(overlayLen);
			this.overlay.Clear();
			this.component ??= new Dictionary<int, Map>(componentLen);
			this.component.Clear();

			if (this.panel_keys != null && this.panel_values != null) {
				for (int p = 0; p < panelLen; p++) {
					this.panel.Add(this.panel_keys[p], this.panel_values[p]);
				}
			}			
			if (this.overlay_keys != null && this.overlay_values != null) {
				for (int p = 0; p < overlayLen; p++) {
					this.panel.Add(this.overlay_keys[p], this.overlay_values[p]);
				}
			}			
			if (this.component_keys != null && this.component_values != null) {
				for (int p = 0; p < componentLen; p++) {
					this.panel.Add(this.component_keys[p], this.component_values[p]);
				}
			}
		}
	}
}