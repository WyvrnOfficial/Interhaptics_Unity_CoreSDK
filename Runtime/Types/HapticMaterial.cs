/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

namespace Interhaptics
{
	/// <summary>
	/// Represents a haptic material, which is a ScriptableObject in Unity.
	/// This class is used to handle haptic effects data.
	/// </summary>
	public class HapticMaterial : UnityEngine.ScriptableObject
    {
		/// <summary>
		/// Serialized and hidden in Inspector, this field holds the haptic data as a string.
		/// </summary>
		[UnityEngine.SerializeField, UnityEngine.HideInInspector]
        private string m_text;
		/// <summary>
		/// Publicly accessible property to get the haptic data text.
		/// </summary>
		public string text => m_text;

		/// <summary>
		/// Creates an instance of HapticMaterial from a string.
		/// </summary>
		/// <param name="text">String containing haptic data.</param>
		/// <returns>A new instance of HapticMaterial with the specified text.</returns>
		public static HapticMaterial CreateInstanceFromString(string text)
        {
            HapticMaterial hapticMaterial = CreateInstance<HapticMaterial>();
            hapticMaterial.m_text = text;
            return hapticMaterial;
        }

		/// <summary>
		/// Creates an instance of HapticMaterial from a Unity TextAsset.
		/// </summary>
		/// <param name="asset">TextAsset containing haptic data.</param>
		/// <returns>A new instance of HapticMaterial created from the TextAsset.</returns>
		public static HapticMaterial CreateInstance(UnityEngine.TextAsset asset)
        {
            return CreateInstanceFromString(asset.text);
        }

		/// <summary>
		/// Overrides the default ToString method to return the haptic data text.
		/// </summary>
		/// <returns>The haptic data text.</returns>
		public override string ToString()
        {
            return text;
        }

    }

}