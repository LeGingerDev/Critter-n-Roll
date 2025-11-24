#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using ScoredProductions.StreamLinked.API.AuthContainers;
	using ScoredProductions.StreamLinked.Utility;

	using UnityEditor;

	using UnityEngine;

	public static class TokenInstanceStatic {

		[MenuItem("Tools/StreamLinked/Create OAuth Token")]
		private static void CreateMenuInstance() {
			TokenInstance t = ScriptableObject.CreateInstance<TokenInstance>();
			t.hideFlags = HideFlags.None;
			string filePath = AssetDatabase.GenerateUniqueAssetPath("Assets/StreamLinked/New Token Instance.asset");
			AssetDatabase.CreateAsset(t, filePath);
			Selection.activeObject = t;
			EditorGUIUtility.PingObject(t);
			DebugManager.LogMessage($"New token created at; '{filePath}'");
		}
	}
}
#endif