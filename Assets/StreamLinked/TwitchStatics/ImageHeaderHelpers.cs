using System.Collections.Generic;

using Unity.Collections;

namespace ScoredProductions.StreamLinked {
	public static class ImageHeaderHelpers {

		public static readonly List<byte> jpg = new List<byte> { 0xFF, 0xD8 };
		public static readonly List<byte> png = new List<byte> { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
		public static readonly List<byte> gif = new List<byte> { 0x47, 0x49, 0x46 };

		public static bool IsJPG(this byte[] array, int offset = 0) {
			for (int x = 0, arrayIndex = offset; x < jpg.Count; x++, arrayIndex++) {
				if (arrayIndex > array.Length - 1 || array[arrayIndex] != jpg[x]) {
					return false;
				}
			}
			return true;
		}

		public static bool IsPNG(this byte[] array, int offset = 0) {
			for (int x = 0, arrayIndex = offset; x < png.Count; x++, arrayIndex++) {
				if (arrayIndex > array.Length - 1 || array[arrayIndex] != png[x]) {
					return false;
				}
			}
			return true;
		}

		public static bool IsGIF(this byte[] array, int offset = 0) {
			for (int x = 0, arrayIndex = offset; x < gif.Count; x++, arrayIndex++) {
				if (arrayIndex > array.Length - 1 || array[arrayIndex] != gif[x]) {
					return false;
				}
			}
			return true;
		}

		public static bool IsJPG(this NativeArray<byte> array, int offset = 0) {
			for (int x = 0, arrayIndex = offset; x < jpg.Count; x++, arrayIndex++) {
				if (arrayIndex > array.Length - 1 || array[arrayIndex] != jpg[x]) {
					return false;
				}
			}
			return true;
		}

		public static bool IsPNG(this NativeArray<byte> array, int offset = 0) {
			for (int x = 0, arrayIndex = offset; x < png.Count; x++, arrayIndex++) {
				if (arrayIndex > array.Length - 1 || array[arrayIndex] != png[x]) {
					return false;
				}
			}
			return true;
		}

		public static bool IsGIF(this NativeArray<byte> array, int offset = 0) {
			for (int x = 0, arrayIndex = offset; x < gif.Count; x++, arrayIndex++) {
				if (arrayIndex > array.Length - 1 || array[arrayIndex] != gif[x]) {
					return false;
				}
			}
			return true;
		}
	}
}
