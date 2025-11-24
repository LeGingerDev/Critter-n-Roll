
using System;

using UnityEngine;

namespace ScoredProductions.StreamLinked.Utility {

	public static class Extensions {
		/// <summary>
		/// <b>value</b>
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string RichTextBold(this string str) => "<b>" + str + "</b>";
		/// <summary>
		/// #{0:X2}{1:X2}{2:X2}
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToHex(this Color clr) => string.Format("#{0:X2}{1:X2}{2:X2}", clr.r.ToByte(), clr.g.ToByte(), clr.b.ToByte());
		/// <summary>
		/// #{0:X2}{1:X2}{2:X2}
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static byte ToByte(this float f) => (byte)(Mathf.Clamp01(f) * 255);
		/// <summary>
		/// <color={0}>value</color>
		/// </summary>
		/// <param name="str"></param>
		/// <param name="clr">{0}</param>
		/// <returns></returns>
		public static string RichTextColour(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
		/// <summary>
		/// <color={0}>value</color>
		/// </summary>
		/// <param name="str"></param>
		/// <param name="clr">{0}</param>
		/// <returns></returns>
		public static string RichTextColour(this string str, Color clr) => string.Format("<color={0}>{1}</color>", clr.ToHex(), str);
		/// <summary>
		/// <i>value</i>
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string RichTextItalic(this string str) => "<i>" + str + "</i>";
		/// <summary>
		/// <size={0}>value</size>
		/// </summary>
		/// <param name="str"></param>
		/// <param name="size">{0}</param>
		/// <returns></returns>
		public static string RichTextSize(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);
		/// <summary>
		/// Handy for strings, why not arrays
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this Array array) => array == null || array.Length == 0;
		/// <summary>
		/// Process a string to be more human readable
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToReadable(this string value) {
			int len = value.Length;
			Span<char> chars = stackalloc char[len];
			int x = 0;
			int y = 0;

			for (; x < len; x++) {
				chars[x] = value[x];
			}

			x = 0;
			while (x < len) {
				char c = chars[x];
				if (x == 0) { // Proon empty starting chars
					if (char.IsWhiteSpace(c) || c == '_') {
						chars = chars.Slice(1, len--);						
					}
					else {
						chars[x++] = char.ToUpper(c);
					}
				}
				else {
					char p = chars[x - 1]; // previous
					if (c == '_') {
						chars[x] = ' ';
					}
					else if ((char.IsUpper(c) && char.IsLower(p))
						|| (x < len - 2 && char.IsUpper(c) && char.IsUpper(p) && char.IsLower(chars[x + 1]) && char.IsLower(chars[x + 2]))) { // Look 2 ahead, e.g VIPs

						Span<char> temp = stackalloc char[++len];
						for (y = 0; y < len; y++) {
							if (y == x) {
								temp[y] = ' ';
							} else {
								temp[y] = chars[y < x ? y : y - 1];
							}
						}
						chars = temp;
					}
					else if (char.IsLower(c) && char.IsWhiteSpace(p)) {
						chars[x] = char.ToUpper(c);
					}
					x++;
				}
			}

			return new string(chars);
		}
		/// <summary>
		/// Applies string.ToReadable to each value in array
		/// </summary>
		/// <param name="value"></param>
		public static void MakeReadable(this string[] value) {
			for (int x = 0; x < value.Length; x++) {
				value[x] = value[x].ToReadable();
			}
		}
	}
}