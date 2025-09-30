/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interhaptics.HapticBodyMapping;
using Interhaptics.Core;

namespace Interhaptics.Utils
{
    [AddComponentMenu("Interhaptics/AudioHapticSource")]
    [RequireComponent(typeof(AudioSource))]
    public class AudioHapticSource : Internal.HapticSource
    {
        // AudioSource variables and properties
        [SerializeField]
        public AudioSource audioSource;
        [SerializeField]
        private HapticBodyPart[] hapticBodyParts;
		private bool switchedPlayAtStart = false;

		public bool PlayOnAwake { get { return playAtStart; } set { playAtStart = value; } }

		protected override void Awake()
		{
			if (audioSource == null && !TryGetComponent<AudioSource>(out audioSource))
			{
				Debug.LogError("AudioSource is not assigned, please assign one");
				return;
			}
			base.Awake();
			if (audioSource.playOnAwake)
			{
				audioSource.playOnAwake = false; // Prevent AudioSource from auto-playing
				playAtStart = true; // Enable haptic effect to play on start and sync with audio
			}
		}

		protected override void Start()
        {
            if (playAtStart)
            {
                playAtStart = false; //so it doesn't play only vibration on start
            }
			AddTarget(hapticBodyParts.Select(hapticBodyPart => new CommandData(Operator.Plus, hapticBodyPart.BodyPart, hapticBodyPart.Side)).ToList());
			base.Start();
			if (switchedPlayAtStart)
			{
				playAtStart = true;
				playingCoroutine = StartCoroutine(ControlVibration());
			}
		}   

        public override void Play()
        {
			AddTarget(hapticBodyParts.Select(hapticBodyPart => new CommandData(Operator.Plus, hapticBodyPart.BodyPart, hapticBodyPart.Side)).ToList());
            base.Play();
            audioSource.Play();
        }

        public override void Stop()
        {
			base.Stop();
            audioSource.Stop();
            RemoveTarget(hapticBodyParts.Select(hapticBodyPart => new CommandData(Operator.Plus, hapticBodyPart.BodyPart, hapticBodyPart.Side)).ToList());
        }

		// Use the base class's coroutine for looping
		public override void PlayEventVibration()
		{
			if (playingCoroutine != null)
			{
				StopCoroutine(playingCoroutine);
			}
			playingCoroutine = StartCoroutine(ControlVibration());
		}

		public override IEnumerator ControlVibration()
		{
#if (!ENABLE_METAQUEST && !ENABLE_OPENXR && UNITY_ANDROID && !UNITY_EDITOR) || UNITY_IOS
			yield return new WaitForSeconds(vibrationOffset);
#endif
			DebugMode(string.Format("Started playing haptics! + {0}", Time.time));
			int loopsPlayed = 0;
			float loopStartTime = Time.time;
			float totalTimePlayed = 0f;
			int maxComputedLoops = maxLoops > 0 ? maxLoops : int.MaxValue;

			while (loopsPlayed < maxComputedLoops)
			{
				Play(); // Play audio and haptic effect
				loopsPlayed++;
				DebugMode($"Loop {loopsPlayed} start at {Time.time}");

				// Wait for the haptic effect duration to finish before restarting
				yield return new WaitForSeconds((float)hapticEffectDuration);
				totalTimePlayed = Time.time - loopStartTime;

				// Check if the maxLoops condition has been met
				if (loopsPlayed >= maxComputedLoops)
				{
					DebugMode($"Max loops reached: {loopsPlayed} loops at {Time.time}");
					break;
				}
			}
			Stop(); // Stop audio and haptic effect
			DebugMode($"Finished playing haptics at {Time.time} after {totalTimePlayed} seconds");
			playingCoroutine = null;
		}
	}
}