/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Interhaptics.HapticBodyMapping;

namespace Interhaptics.Core
{
	public class MobileControl : MonoBehaviour
	{
		private static MobileControl currentInstance;

		private struct HapticEffect
		{
			public int hMaterialId;
			public double duration;
			public int loops;
			public double intensity;
			public float vibrationOffset;

			public HapticEffect(int hMaterialId, double duration, int loops, double intensity, float vibrationOffset)
			{
				this.hMaterialId = hMaterialId;
				this.duration = duration;
				this.loops = loops;
				this.intensity = intensity;
				this.vibrationOffset = vibrationOffset;
			}
		}

		private Queue<HapticEffect> hapticEffectsQueue = new Queue<HapticEffect>();
		private bool isPlaying = false;
		private Coroutine playingCoroutine = null; // Reference to the current playing coroutine

		void Awake()
		{
			if (currentInstance != null && currentInstance != this)
			{
				Destroy(gameObject);
			}
			else
			{
				currentInstance = this;
				DontDestroyOnLoad(gameObject);
			}
		}

		public static void EnqueueEffect(int hMaterialId, double duration, int loops, double intensity, float vibrationOffset = 0f)
		{
			if (currentInstance == null)
			{
				GameObject mobileControllerObject = new GameObject("MobileController");
				currentInstance = mobileControllerObject.AddComponent<MobileControl>();
				DontDestroyOnLoad(mobileControllerObject);
			}

			currentInstance.EnqueueEffectInternal(new HapticEffect(hMaterialId, duration, loops, intensity, vibrationOffset));
		}

		private void EnqueueEffectInternal(HapticEffect effect)
		{
			if (isPlaying)
			{
				// Stop the currently playing effect immediately
				StopCurrentEffect();
			}

			// Clear the queue and add the new effect
			hapticEffectsQueue.Clear();
			hapticEffectsQueue.Enqueue(effect);
			PlayNextEffect();
		}

		private void PlayNextEffect()
		{
			if (hapticEffectsQueue.Count > 0 && !isPlaying)
			{
				var effect = hapticEffectsQueue.Dequeue();
				playingCoroutine = StartCoroutine(PlayHapticEffect(effect));
			}
		}

		private IEnumerator PlayHapticEffect(HapticEffect effect)
		{
			if (effect.vibrationOffset > 0)
			{
				yield return new WaitForSeconds(effect.vibrationOffset);
			}
			isPlaying = true;
			HAR.StopAllEvents(); // Stop any currently playing haptic events

			HAR.SetEventIntensity(effect.hMaterialId, effect.intensity);

			for (int i = 1; i <= effect.loops; i++)
			{
				HAR.PlayEvent(effect.hMaterialId, (double)-Time.realtimeSinceStartup, 0, 0);
				Debug.Log($"Playing haptic effect: Material ID {effect.hMaterialId}, Loop {i}/{effect.loops}");
				yield return new WaitForSeconds((float)effect.duration);
			}

			isPlaying = false;
			PlayNextEffect(); // Check if there is another effect queued
		}

		private void StopCurrentEffect()
		{
			if (playingCoroutine != null)
			{
				StopCoroutine(playingCoroutine);
				playingCoroutine = null;
			}
			HAR.StopAllEvents(); // Ensure to stop the current haptic event
			isPlaying = false;
		}

		public static void StopEffects()
		{
			if (currentInstance != null)
			{
				currentInstance.StopCurrentEffect();
				currentInstance.hapticEffectsQueue.Clear();
				Debug.Log("All haptic effects stopped.");
			}
		}
	}
}
