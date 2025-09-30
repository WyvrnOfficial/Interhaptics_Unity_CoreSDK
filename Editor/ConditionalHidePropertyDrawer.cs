/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEditor;
using UnityEngine;
using Interhaptics.Utils;

namespace Interhaptics.Editor
{
	[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
	public class ConditionalHidePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ConditionalHideAttribute conditionalHideAttribute = (ConditionalHideAttribute)attribute;
			bool enabled = ShouldShowProperty(conditionalHideAttribute, property);

			bool wasEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (enabled)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}

			GUI.enabled = wasEnabled;
		}

		private bool ShouldShowProperty(ConditionalHideAttribute conditionalHideAttribute, SerializedProperty property)
		{
			SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionalHideAttribute.ConditionalSourceField);
			return sourcePropertyValue != null && sourcePropertyValue.boolValue;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			ConditionalHideAttribute conditionalHideAttribute = (ConditionalHideAttribute)attribute;
			if (!ShouldShowProperty(conditionalHideAttribute, property))
				return -EditorGUIUtility.standardVerticalSpacing;

			return EditorGUI.GetPropertyHeight(property, label);
		}
	}
}
