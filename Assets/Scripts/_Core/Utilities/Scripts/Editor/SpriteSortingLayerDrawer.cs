using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utilities.Editor
{
    [CustomPropertyDrawer(typeof(SpriteSortingLayerAttribute))]
    public class SpriteSortingLayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.LabelField(position, label.text, "Use SortingLayer with int.");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            string[] sortingLayerNames = SortingLayer.layers.Select(l => l.name).ToArray();
            int[] sortingLayerIDs = SortingLayer.layers.Select(l => l.id).ToArray();
            int sortingLayerIndex = Array.IndexOf(sortingLayerIDs, property.intValue);

            sortingLayerIndex = EditorGUI.Popup(position, label.text, sortingLayerIndex, sortingLayerNames);

            if (sortingLayerIndex >= 0)
            {
                property.intValue = sortingLayerIDs[sortingLayerIndex];
            }

            EditorGUI.EndProperty();
        }
    }
}