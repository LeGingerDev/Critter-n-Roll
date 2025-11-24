using System.Collections.Generic;

namespace ScoredProductions.StreamLinked.VisualScripting {

	public static class VisualScriptingExtensions {
		/// <summary>
		/// Used in Visual Scripting as a StringPair is used and needs converting to a tuple
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static (string, string)[] ToTuple(this List<StringPair> value) {
			(string, string)[] newArray = new (string, string)[value.Count];

			for (int x = 0; x < value.Count; x++) {
				newArray[x] = value[x].ToTuple();
			}

			return newArray;
		}
	}

}