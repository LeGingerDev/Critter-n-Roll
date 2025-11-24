using System;
using System.Globalization;

using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

using UnityEngine;

namespace ScoredProductions.StreamLinked {

	/// <summary>
	/// Additional functions, methods and values.
	/// </summary>
	public static class TwitchStatic {

		public const int SSL_IRC_PORT = 443;
		public const int NON_SSL_IRC_PORT = 80;

		public static readonly DateTime TwitchUTCStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public readonly static WaitForEndOfFrame EndOfFrameWait = new WaitForEndOfFrame();
		public readonly static WaitForSeconds OneSecondWait = new WaitForSeconds(1);
		public readonly static WaitForSecondsRealtime OneSecondWaitRealtime = new WaitForSecondsRealtime(1);

		public static DateTime GetExpiryDate(long seconds) {
			TimeSpan timeToAdd = TimeSpan.FromSeconds(seconds);
			DateTime expiretime = DateTime.UtcNow + timeToAdd;
			return expiretime;
		}

		public static Color ConvertHexToColor(string hexString) {
			if (string.IsNullOrEmpty(hexString)) {
				return Color.white;
			}

			if (hexString.Contains('#')) {
				hexString = hexString.Replace("#", "");
			}

			return new Color(int.Parse(hexString[0..1], NumberStyles.AllowHexSpecifier) / 255.0f,
				int.Parse(hexString[2..3], NumberStyles.AllowHexSpecifier) / 255.0f,
				int.Parse(hexString[4..5], NumberStyles.AllowHexSpecifier) / 255.0f);
		}

		public static string AppendOAuthToBearer(string oauth) => $"{TwitchWords.BEARER} {oauth}";
		public static string AppendOAuthToAuth(string oauth) => $"{TwitchWords.OAUTH}:{oauth}";

		public static string[] ByteSizeUnits = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };

		public static string FormatBytes(double bytes) {
			double kilo = 1024.0;

			int i = 0;

			while (bytes >= kilo) {
				bytes /= kilo;
				i++;
			}
			return Math.Round(bytes, 2) + ByteSizeUnits[i];
		}

		public const string NonceCharacters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public static string GenerateNonce(int length = 8) {
			System.Random rnd = new System.Random(DateTime.Now.Millisecond);
			int nonceLength = NonceCharacters.Length - 1;
			char[] chars = new char[length];
			for (int x = 0; x < length; x++) {
				chars[x] = NonceCharacters[rnd.Next(nonceLength)];
			}
			return new string(chars);
		}

		public static string ToJSONString(this (string, string)[] value) {
			if (value == null) {
				return "";
			}
			JsonObject json = new JsonObject();
			foreach ((string, string) item in value) {
				json.Add(item.Item1, item.Item2);
			}
			return JsonWriter.Serialize(json);
		}
	}
}
