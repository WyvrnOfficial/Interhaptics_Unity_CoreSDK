/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using Interhaptics.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Interhaptics.Samples
{
	[RequireComponent(typeof(Slider))]
	public class GlobalIntensitySlider : MonoBehaviour
	{
		public GlobalHapticIntensityController hapticController;
		private Slider slider;

		private void Awake()
		{
			slider = GetComponent<Slider>();
			if (hapticController!=null)
			{
				slider.value = (float)hapticController.globalIntensity;
				slider.onValueChanged.AddListener(HandleSliderValueChanged);
				// Subscribe to the onIntensityChanged event
				hapticController.onIntensityChanged += UpdateSliderPosition;
			}
		}

		private void OnDestroy()
		{
			if (hapticController != null)
			{
				// Unsubscribe to avoid memory leaks
				hapticController.onIntensityChanged -= UpdateSliderPosition;
			}
		}

		public void HandleSliderValueChanged(float value)
		{
			// Update the global intensity when the slider's value changes
			hapticController.OnIntensitySliderChanged(value);
		}
		private void UpdateSliderPosition(double newIntensity)
		{
			slider.value = (float)newIntensity;
		}
	}
}