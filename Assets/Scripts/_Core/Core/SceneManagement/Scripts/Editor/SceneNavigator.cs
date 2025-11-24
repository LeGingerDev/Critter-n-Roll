using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneManagement.Editor
{
    public class SceneNavigator : EditorWindow
    {
        private string[] _mainScenePaths;
        private string[] _mainSceneNames;

        private string[] _sandboxScenePaths;
        private string[] _sandboxSceneNames;

        private string _lastScenePath;

        [MenuItem("BagOfDucks/Scene Navigator")]
        public static void ShowWindow()
        {
            GetWindow<SceneNavigator>("Scene Navigator");
        }

        private void OnEnable()
        {
            RefreshSceneList();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh Scene List", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
            {
                RefreshSceneList();
            }

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Simulate From First Scene", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
            {
                SimulateFromFirstScene();
            }

            GUI.backgroundColor = Color.white;

            GUILayout.Label("Main Scenes", EditorStyles.boldLabel);
            DrawSceneButtons(_mainSceneNames, _mainScenePaths);

            GUILayout.Space(20); // Separator

            if (_sandboxSceneNames != null && _sandboxSceneNames.Length > 0)
            {
                GUILayout.Label("Sandbox Scenes", EditorStyles.boldLabel);
                DrawSceneButtons(_sandboxSceneNames, _sandboxScenePaths);
            }

            if (!Directory.Exists("Assets/Scenes/MainScenes"))
            {
                if (GUILayout.Button("Create MainScenes Folder", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
                {
                    Directory.CreateDirectory("Assets/Scenes/MainScenes");
                    AssetDatabase.Refresh();
                }
            }
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && !string.IsNullOrEmpty(_lastScenePath))
            {
                EditorSceneManager.OpenScene(_lastScenePath);
                _lastScenePath = null; // Reset the stored path
            }
        }

        private void DrawSceneButtons(string[] sceneNames, string[] scenePaths)
        {
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (GUILayout.Button(sceneNames[i], GUILayout.ExpandWidth(true), GUILayout.Height(30)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePaths[i]);
                    }
                }
            }
        }

        private void RefreshSceneList()
        {
            if (Directory.Exists("Assets/Scenes/MainScenes"))
            {
                _mainScenePaths =
                    Directory.GetFiles("Assets/Scenes/MainScenes", "*.unity", SearchOption.AllDirectories);
                _mainSceneNames = _mainScenePaths.Select(Path.GetFileNameWithoutExtension).ToArray();
            }

            if (Directory.Exists("Assets/Scenes/Sandboxes"))
            {
                _sandboxScenePaths =
                    Directory.GetFiles("Assets/Scenes/Sandboxes", "*.unity", SearchOption.AllDirectories);
                _sandboxSceneNames = _sandboxScenePaths.Select(Path.GetFileNameWithoutExtension).ToArray();
            }
        }

        private void SimulateFromFirstScene()
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.LogWarning("No scenes in Build Settings. Cannot simulate.");
                return;
            }

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // Store the current scene path
                _lastScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;

                // Get the path of the first scene in the Build Settings
                string firstScenePath = EditorBuildSettings.scenes[0].path;

                // Open the first scene
                EditorSceneManager.OpenScene(firstScenePath);

                // Start game simulation
                EditorApplication.isPlaying = true;
            }
        }
    }
}