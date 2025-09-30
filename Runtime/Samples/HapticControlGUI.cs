/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using Interhaptics.Utils;

namespace Interhaptics.Samples
{
	public class HapticControlGUI : MonoBehaviour
	{
		[SerializeField]
		private EventHapticManagerMobile eventHapticManagerMobile;
		[SerializeField]
		private EventHapticSource eventHapticSource;
		[SerializeField]
		private GameObject buttonPlay;
		[SerializeField]
		private GameObject buttonStop;

		void Start()
		{
			buttonPlay.SetActive(true);
			buttonStop.SetActive(false);
		}

		public void StartHaptics()
		{
			eventHapticManagerMobile.StopPlayingEventHapticSources();
			eventHapticSource.PlayEventVibration();
		}
		public void StopHaptics()
		{
			eventHapticSource.Stop();
			Interhaptics.Core.HAR.ClearActiveEvents();
		}

		void Update()
		{
			if (eventHapticSource == null) return;
			if (eventHapticSource.isPlaying)
			{
				buttonPlay.SetActive(false);
				buttonStop.SetActive(true);
			}
			else
			{
				buttonPlay.SetActive(true);
				buttonStop.SetActive(false);
			}
		}
	}

}
