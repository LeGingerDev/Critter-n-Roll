using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneManagement.Editor
{
    [InitializeOnLoad]
    public static class OTBGStartSceneHelper
    {
        /// <summary>
        /// Set this to the scene name you'd like to always start at.
        /// Leaving it empty will ensure the current scene starts on play.
        /// </summary>
        private const string START_SCENE = ""; // Set this to null or keep it empty to use the current scene

        static OTBGStartSceneHelper()
        {
            Refresh();
        }

        private static void Refresh()
        {
            // If START_SCENE is empty, set playModeStartScene to null to use the current scene
            if (string.IsNullOrEmpty(START_SCENE))
            {
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            var startScenePath = Path.Combine("Assets/", "Scenes/", START_SCENE);
            var preloadScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(startScenePath);

            if (preloadScene == null)
            {
                Debug.LogError($"{nameof(OTBGStartSceneHelper)} could not find start scene {START_SCENE}");
                // If the specified start scene is not found, revert to using the current scene
                EditorSceneManager.playModeStartScene = null;
            }
            else
            {
                // If a valid start scene is specified and found, use it as the start scene
                EditorSceneManager.playModeStartScene = preloadScene;
            }
        }
    }
}