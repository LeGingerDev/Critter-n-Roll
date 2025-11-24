using System;

using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

namespace ScoredProductions.StreamLinked.Utility {
	// https://docs.unity3d.com/ScriptReference/PlayerPrefs.html

	[Serializable]
	public enum SavedSettings : byte {
		/// <summary>
		/// <term>Type</term> <c>string</c>
		/// </summary>
		TwitchClientSecret,
		/// <summary>
		/// <term>Type</term> <c>string</c>
		/// </summary>
		TwitchClientID,
		/// <summary>
		/// <term>Type</term> <c>string</c>
		/// </summary>
		TwitchTarget,

		/// <summary>
		/// <term>Type</term> <c> JsonValue { String (Used ClientID), String (Used Secret), List[IAuthFlow](JSON Array) }</c>
		/// </summary>
		TwitchAuthenticationTokens,
	}

	/// <summary>
	/// Simple method of credential store.
	/// PlayerPref will need populating on Install of App.
	/// PlayerPrefs are not saved/included in build.
	/// </summary>
	public static class InternalSettingsStore {

		public const string SettingsWord = "Settings";

		/// <summary>
		/// Requires main thread
		/// </summary>
		/// <param name="editSetting"></param>
		public static void EditSetting(SavedSettings setting, string value, bool logMessage = false) {
			string keyName = Enum.GetName(typeof(SavedSettings), setting);
			if (string.IsNullOrWhiteSpace(value)) {
				UnityEngine.PlayerPrefs.DeleteKey(keyName);
			}
			else {
				UnityEngine.PlayerPrefs.SetString(keyName, value);
			}
			if (logMessage) {
				DebugManager.LogMessage($"PlayerPrefs Write: {setting}".RichTextBold());
			}
		}

		/// <summary>
		/// Edit a group of settings values, Requires main thread
		/// </summary>
		/// <param name="setting"></param>
		/// <param name="value"></param>
		public static void GroupEditSettings((SavedSettings, string)[] values) {
			foreach ((SavedSettings setting, string value) in values) {
				EditSetting(setting, value);
			}
		}

		/// <summary>
		/// This will delete ALL SavedSettings, use with caution, Requires main thread
		/// </summary>
		public static void ClearPlayerPrefs() {
			foreach (SavedSettings setting in Enum.GetValues(typeof(SavedSettings))) {
				EditSetting(setting, "");
			}
		}

		/// <summary>
		/// Attempts to get value from PlayerPrefs, all values are stored as string, this will try to convert it back
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="setting"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException"></exception>
		public static bool TryGetSetting<T>(SavedSettings setting, out T value, bool logMessage = false) where T : IComparable, IConvertible, IComparable<T>, IEquatable<T> {
			value = default;

			string storedValue = UnityEngine.PlayerPrefs.GetString(Enum.GetName(typeof(SavedSettings), setting));
			if (logMessage) {
				DebugManager.LogMessage($"PlayerPrefs Read: {setting}".RichTextBold());
			}

			Type type = typeof(T);

			switch (type.Name) {
				case "String":
					if (!string.IsNullOrWhiteSpace(storedValue)) {
						value = (T)(object)storedValue;
						return true;
					}
					goto default;
				case "Char":
					if (char.TryParse(storedValue, out char c)) {
						value = (T)(object)c;
						return true;
					}
					goto default;
				case "Boolean":
					if (bool.TryParse(storedValue, out bool bl)) {
						value = (T)(object)bl;
						return true;
					}
					goto default;
				case "SByte":
					if (sbyte.TryParse(storedValue, out sbyte sb)) {
						value = (T)(object)sb;
						return true;
					}
					goto default;
				case "Byte":
					if (byte.TryParse(storedValue, out byte b)) {
						value = (T)(object)b;
						return true;
					}
					goto default;
				case "Int16":
					if (short.TryParse(storedValue, out short s)) {
						value = (T)(object)s;
						return true;
					}
					goto default;
				case "UInt16":
					if (ushort.TryParse(storedValue, out ushort us)) {
						value = (T)(object)us;
						return true;
					}
					goto default;
				case "Int32":
					if (int.TryParse(storedValue, out int i)) {
						value = (T)(object)i;
						return true;
					}
					goto default;
				case "UInt32":
					if (uint.TryParse(storedValue, out uint ui)) {
						value = (T)(object)ui;
						return true;
					}
					goto default;
				case "Int64":
					if (long.TryParse(storedValue, out long l)) {
						value = (T)(object)l;
						return true;
					}
					goto default;
				case "UInt64":
					if (ulong.TryParse(storedValue, out ulong ul)) {
						value = (T)(object)ul;
						return true;
					}
					goto default;
				case "Single":
					if (float.TryParse(storedValue, out float f)) {
						value = (T)(object)f;
						return true;
					}
					goto default;
				case "Double":
					if (double.TryParse(storedValue, out double dou)) {
						value = (T)(object)dou;
						return true;
					}
					goto default;
				case "Decimal":
					if (decimal.TryParse(storedValue, out decimal de)) {
						value = (T)(object)de;
						return true;
					}
					goto default;
				default:
					return false;
					// Left over from storing settings in files, PlayerPref values are ""
					//string message = $"TryGetSetting<{setting}> : <{typeof(T).Name}> parse missmatch";
					//DebugManager.LogMessage(message, DebugManager.ErrorLevel.Exception);
					//throw new NotSupportedException(message);
			}
		}

		/// <summary>
		/// Get all stored settings in PlayerPrefs under the settings enum <c>SavedSettings</c>
		/// </summary>
		/// <returns></returns>
		public static string GetAllSettingsAsJson() {
			JsonObject innerValue = new JsonObject();

			foreach (SavedSettings setting in Enum.GetValues(typeof(SavedSettings))) {
				string settingName = Enum.GetName(typeof(SavedSettings), setting);
				string innerSetting = UnityEngine.PlayerPrefs.GetString(settingName);
				if (!string.IsNullOrWhiteSpace(innerSetting)) {
					innerValue.Add(settingName, new JsonValue(innerSetting));
				}
			}

			JsonObject outValue = new JsonObject {
				{ SettingsWord, innerValue }
			};
			return JsonWriter.Serialize(outValue, true);
		}

		/// <summary>
		/// Parses a saved instance of the settings into the store, overwrites existing values
		/// </summary>
		/// <param name="JSON"></param>
		public static void InputDataJSON(string JSON) {
			JsonObject jsonParsed = JsonReader.Parse(JSON)[SettingsWord].AsJsonObject;
			if (jsonParsed == null) {
				return;
			}
			SavedSettings[] settings = (SavedSettings[])Enum.GetValues(typeof(SavedSettings));
			for (int i = 0; i < settings.Length; i++) {
				SavedSettings setting = settings[i];
				if (jsonParsed.TryGetKey(Enum.GetName(typeof(SavedSettings), setting), out JsonValue jvalue)) {
					EditSetting(setting, jvalue.AsString);
				}
			}
		}
	}
}