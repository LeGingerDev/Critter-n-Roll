using System;
using System.Collections.Generic;
using System.Reflection;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace ScoredProductions.StreamLinked.Editor {

    public static class CheckEnum
    {
#if UNITY_EDITOR
        [MenuItem("Tools/StreamLinked/Enums/Check for Duplicates")]
        public static void CheckEnums() {
			DebugManager.LogMessage("Performing Enum check for duplicate values");
			Type[] a = Assembly.GetAssembly(typeof(TwitchAPIClient)).GetTypes();
            Dictionary<long, List<string>> values = new Dictionary<long, List<string>>();
            foreach (Type t in a) {
                values.Clear();
                if (t.IsEnum) {
					string[] container = Enum.GetNames(t);
                    for (int x = 0; x < container.Length; x++) {
                        string name = container[x];
                        if (Enum.TryParse(t, name, out object result) && !values.TryAdd(Convert.ToInt64(result), new List<string>() { name })) {
                            List<string> existingValues = values[Convert.ToInt64(result)];
                            existingValues.Add(name);
                        }
                    }
                    if (values.Count > 0) {
                        foreach (KeyValuePair<long, List<string>> dic in values) {
                            if (dic.Value.Count > 1) {
							    DebugManager.LogMessage($"Enum '{t.Name}', Duplicate value assigned on value: '{dic.Key}', values with the same assigned value are: '{string.Join(", ", dic.Value)}'", DebugManager.ErrorLevel.Error);
                            }
						}
                    }
				}
			}
			DebugManager.LogMessage("Enum check Complete");
		}
#endif
    }
}


/*
var NextValue = Enum.GetValues(typeof(ScoredProductions.StreamLinked.TwitchScopesEnum)).Cast<ScoredProductions.StreamLinked.EventSub.TwitchScopesEnum>().Max();
		Console.Write(((int)NextValue) + 1);
*/