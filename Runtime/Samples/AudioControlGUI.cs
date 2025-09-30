/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using Interhaptics.Utils;

namespace Interhaptics.Samples
{
	public class AudioControlGUI : MonoBehaviour
	{
		[SerializeField]
		private AudioHapticSource audioHapticSource;
		[SerializeField]
		AudioManagerMobile audioManagerMobile;
		[SerializeField]
		private GameObject buttonPlay;
		[SerializeField]
		private GameObject buttonStop;

		// Start is called before the first frame update
		private void Start()
		{
			buttonPlay.SetActive(true);
			buttonStop.SetActive(false);
		}

		public void StartHaptics()
		{
			audioManagerMobile.StopPlayingAudioHapticSources();
			Interhaptics.Core.HAR.ClearActiveEvents();
			audioHapticSource.PlayEventVibration();
		}

		public void StopHaptics()
		{
			audioHapticSource.audioSource.Stop();
			Interhaptics.Core.HAR.ClearActiveEvents();
		}

		// Update is called once per frame
		private void Update()
		{
			if (audioHapticSource.audioSource.isPlaying)
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
