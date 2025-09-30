/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using Interhaptics.Internal;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

namespace Interhaptics.Samples
{
	[RequireComponent(typeof(Slider))]
	public class SourceIntensitySlider : MonoBehaviour
	{
		public HapticSource[] hapticSources;
		private Slider slider;

		private void Awake()
		{
			slider = GetComponent<Slider>();
			if (hapticSources[0] != null)
			{
				slider.value = hapticSources[0].SourceIntensity;
				slider.onValueChanged.AddListener(HandleSliderValueChanged);
			}
			else
			{
				Debug.Log("No HapticSource component found in the HapticSources array.");
			}
		}

		public void HandleSliderValueChanged(float value)
		{
			// Update the source intensity for all Haptic Sources when the slider's value changes
			foreach (HapticSource hapticSource in hapticSources)
			{
				if (hapticSource != null)
				{
					hapticSource.SourceIntensity = value;
					hapticSource.ApplySourceIntensity();
				}
			}
		}
	}
}
