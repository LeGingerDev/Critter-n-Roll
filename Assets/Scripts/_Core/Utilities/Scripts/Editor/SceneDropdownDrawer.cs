using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilities.PropertyAttributes;

namespace Utilities.Editor
{
    [CustomPropertyDrawer(typeof(SceneDropdownAttribute))]
    public class SceneDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                // Get all scene paths in the project
                string[] scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
                string[] sceneNames = scenePaths.Select(PathToSceneName).ToArray();

                string sceneName = property.stringValue;
                int currentIndex = Array.IndexOf(sceneNames, sceneName);

                // Draw the label
                position.width /= 2;
                EditorGUI.LabelField(position, label);

                // Draw the popup
                position.x += position.width;
                EditorGUI.BeginChangeCheck();
                currentIndex = EditorGUI.Popup(position, currentIndex, sceneNames);

                if (EditorGUI.EndChangeCheck())
                {
                    // Update the selected scene if it changed
                    property.stringValue = sceneNames[currentIndex];
                }
            }
            else
            {
                EditorGUI.LabelField(position, label, "Property type is not a string");
            }
        }

        private string PathToSceneName(string path)
        {
            // Extract the scene name from the full path
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}