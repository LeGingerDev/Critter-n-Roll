using System.Collections;

using ScoredProductions.StreamLinked.Utility;

namespace ScoredProductions.StreamLinked.UniGif {
	/// <summary>
	/// Extension methods class
	/// </summary>
	public static class UniGifExtension {
        /// <summary>
        /// Convert BitArray to int (Specifies the start index and bit length)
        /// </summary>
        /// <param name="startIndex">Start index</param>
        /// <param name="bitLength">Bit length</param>
        /// <returns>Converted int</returns>
        public static int GetNumeral(this BitArray array, int startIndex, int bitLength) {
			BitArray newArray = new BitArray(bitLength);

            for (int i = 0; i < bitLength; i++) {
                if (array.Length <= startIndex + i) {
                    newArray[i] = false;
                }
                else {
                    bool bit = array.Get(startIndex + i);
                    newArray[i] = bit;
                }
            }

            return newArray.ToNumeral();
        }

        /// <summary>
        /// Convert BitArray to int
        /// </summary>
        /// <returns>Converted int</returns>
        public static int ToNumeral(this BitArray array) {
            if (array == null) {
				DebugManager.LogMessage("array is nothing.", DebugManager.ErrorLevel.Error);
                return 0;
            }
            if (array.Length > 32) {
				DebugManager.LogMessage("must be at most 32 bits long.", DebugManager.ErrorLevel.Error);
                return 0;
            }

			int[] result = new int[1];
            array.CopyTo(result, 0);
            return result[0];
        }
    }
}