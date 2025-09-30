/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using UnityEngine.UI;
using Interhaptics.HapticBodyMapping;

namespace Interhaptics.Samples
{
	[RequireComponent(typeof(Slider))]
	public class TargetBodyPartIntensitySlider : MonoBehaviour
	{
		public HapticBodyPart[] hapticBodyParts; // Reference to the array of HapticBodyPart components
		private Slider slider;

		private void Awake()
		{
			slider = GetComponent<Slider>();
			// Initialize the slider value to the first haptic body part's intensity and subscribe to the onValueChanged event
			if (hapticBodyParts.Length > 0 && hapticBodyParts[0] != null)
			{
				slider.value = (float)hapticBodyParts[0].TargetIntensity; // Assuming all haptic body parts have the same initial intensity
				slider.onValueChanged.AddListener(HandleIntensityChange);
			}
		}

		public void HandleIntensityChange(float value)
		{
			// Update the TargetIntensity of each HapticBodyPart
			foreach (HapticBodyPart hapticBodyPart in hapticBodyParts)
			{
				if (hapticBodyPart != null)
				{
					hapticBodyPart.TargetIntensity = value; // Updating only the value and not calling UpdateTargetIntensity() to avoid crash calls to the native plugin - no HapticMaterialId is set yet
				}
			}
		}
	}
}
