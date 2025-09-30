/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

namespace Interhaptics.Editor
{
	using UnityEditor;
	[CustomEditor(typeof(Interhaptics.Utils.ParametricHapticSource))]
	public class ParametricHapticSourceEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			// This line fetches the current serialized object that this inspector represents.
			SerializedObject so = serializedObject;

			// Start checking for changes in the Inspector
			EditorGUI.BeginChangeCheck();

			// Iterate over all visible properties and draw them, except the hapticMaterial
			so.Update();
			DrawPropertiesExcluding(so, "hapticMaterial");
			so.ApplyModifiedProperties();

			// Apply any changes made in the Inspector
			if (EditorGUI.EndChangeCheck())
			{
				so.ApplyModifiedProperties();
			}
		}
	}
}
