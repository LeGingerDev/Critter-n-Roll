using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(ConstDropdownAttribute))]
    public class ConstDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConstDropdownAttribute dropdownAttribute = (ConstDropdownAttribute)attribute;

            // Get all public constant fields from the static class
            List<string> options = GetConstFields(dropdownAttribute.TargetType);

            if (options == null || options.Count == 0)
            {
                EditorGUI.LabelField(position, label.text, "No constants found.");
                return;
            }

            // Get current value and index
            string currentValue = property.stringValue;
            int selectedIndex = options.IndexOf(currentValue);
            if (selectedIndex == -1) selectedIndex = 0;

            // Display popup
            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options.ToArray());

            // Update the property value
            if (selectedIndex >= 0 && selectedIndex < options.Count)
            {
                property.stringValue = options[selectedIndex];
            }
        }

        private List<string> GetConstFields(Type targetType)
        {
            return targetType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly &&
                            f.FieldType == typeof(string)) // Ensure it's a constant string field
                .Select(f => (string)f.GetRawConstantValue())
                .ToList();
        }
    }
}