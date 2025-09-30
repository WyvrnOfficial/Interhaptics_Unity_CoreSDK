/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEditor;
using UnityEngine;
using Interhaptics;
using Interhaptics.Internal;

namespace Interhaptics.Editor
{

[CustomEditor(typeof(HapticSource))]
public class HapticSourceEditor : UnityEditor.Editor
{
    private SerializedProperty hapticMaterial;
    private SerializedProperty vibrationOffset;
    private SerializedProperty textureOffset;
    private SerializedProperty stiffnessOffset;
    private SerializedProperty debugMode;

    private void OnEnable()
    {
        hapticMaterial = serializedObject.FindProperty("hapticMaterial");
        vibrationOffset = serializedObject.FindProperty("vibrationOffset");
        textureOffset = serializedObject.FindProperty("textureOffset");
        stiffnessOffset = serializedObject.FindProperty("stiffnessOffset");
        debugMode = serializedObject.FindProperty("debugMode");
    }

    public override void OnInspectorGUI()
    {
        HapticSource script = (HapticSource)target;

        GUIContent hapticMaterialLabel = new GUIContent("Haptic Material", "The haptic material used by this source.");
        EditorGUILayout.PropertyField(hapticMaterial, hapticMaterialLabel, true);

        GUIContent vibrationOffsetLabel = new GUIContent("Vibration offset", "The offset applied to the vibration of the haptic effect.");
        EditorGUILayout.PropertyField(vibrationOffset, vibrationOffsetLabel);

        GUIContent textureOffsetLabel = new GUIContent("Texture offset", "The offset applied to the texture of the haptic effect.");
        EditorGUILayout.PropertyField(textureOffset, textureOffsetLabel);

        GUIContent stiffnessOffsetLabel = new GUIContent("Stiffness offset", "The offset applied to the stiffness of the haptic effect.");
        EditorGUILayout.PropertyField(stiffnessOffset, stiffnessOffsetLabel);

        GUIContent debugModeLabel = new GUIContent("Debug mode", "Shows debug messages in Editor if checked.");
        EditorGUILayout.PropertyField(debugMode, debugModeLabel);

        serializedObject.ApplyModifiedProperties();
    }
}

}
