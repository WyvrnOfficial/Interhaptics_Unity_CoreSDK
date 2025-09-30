/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using Interhaptics.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Interhaptics.Samples
{
	public class EventHapticManagerMobile : MonoBehaviour
	{
		[SerializeField]
		private List<EventHapticSource> eventHapticSources;

		public void StopPlayingEventHapticSources()
		{
			foreach (EventHapticSource eventHapticSource in eventHapticSources)
			{
				if (eventHapticSource.isPlaying)
				{
					Core.HAR.StopEvent(eventHapticSource.HapticMaterialId);
					if (eventHapticSource.playingCoroutine != null)
					{
						eventHapticSource.StopCoroutine(eventHapticSource.playingCoroutine);
					}
					eventHapticSource.playingCoroutine = null;
					eventHapticSource.isPlaying = false;
					break;
				}
			}
		}
	}
}
