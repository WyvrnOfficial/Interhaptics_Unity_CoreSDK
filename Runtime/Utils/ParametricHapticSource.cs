/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using System.Linq;
using Interhaptics.HapticBodyMapping;
using Interhaptics.Core;
using System;

namespace Interhaptics.Utils
{

	[Serializable]
	public class TimeValuePair
	{
		public double time;
		[Range(0f, 1f)]
		public double value; // For Amplitude or Pitch
	}

	[Serializable]
	public class TimeAmplitudeFrequencyTriplet
	{
		public double time;
		[Range(0f, 1f)]
		public double amplitude;
		[Range(0f, 1f)]
		public double frequency;
	}

	[AddComponentMenu("Interhaptics/ParametricHapticSource")]
	public class ParametricHapticSource : Internal.HapticSource
	{
#region Public Fields
		[Header("Amplitude Configuration")]
		[Tooltip("Enable to add time-amplitude composition.")]
		public bool useAmplitude;

		[ConditionalHide("useAmplitude", true)]
		[Tooltip("Pairs of time and amplitude values.")]
		public TimeValuePair[] timeAmplitudePairs;

		[Header("Pitch Configuration")]
		[Tooltip("Enable to add time-pitch composition.")]
		public bool usePitch;

		[ConditionalHide("usePitch", true)]
		[Tooltip("Pairs of time and pitch values.")]
		public TimeValuePair[] timePitchPairs;

		[Header("Transient Configuration")]
		[Tooltip("Enable to add transient composition.")]
		public bool useTransients;

		[ConditionalHide("useTransients", true)]
		[Tooltip("Triplets of time, amplitude, and frequency values for transients.")]
		public TimeAmplitudeFrequencyTriplet[] timeAmplitudeFrequencyTriplets;
		//Resulting Amplitude Array
		[HideInInspector]
		private double[] amplitude;
//Resulting Pitch Array
		[HideInInspector]
		private double[] pitch;
		[Tooltip("Minimum value for the frequency range. Default 65")]
		[SerializeField]
		private double freqMin = 65.0;
		[Tooltip("Maximum value for the frequency range. Default 300")]
		[SerializeField]
		private double freqMax = 300.0;
//Resulting Transient Array
		[HideInInspector]
		[SerializeField]
		private double[] transients;
		public HapticBodyPart[] hapticBodyParts;
#endregion

#region Lifecycle
		protected override void Awake()
		{
			// Custom initialization for ParametricHapticSource. Bypasses adding a haptic effect
#if UNITY_ANDROID
			if (HapticManager.MonoScriptingBackend)
			{
				DebugMode(ERROR_MESSAGE_MONO + "Haptic Source");
				return;
			}
			else
			{
				DebugMode("IL2CPP Haptic Source");
			}
#endif
		}
		protected override void Start()
		{
			// Initialize the parametric haptic effect with the vector data
			InitializeParametricHapticSource();
			base.Start();
		}

		private void OnDestroy()
		{
			// Optional: Cleanup the haptic effect
			//HAR.ClearEvent(HapticMaterialId);
		}
		#endregion

		public bool InitializeParametricHapticSource()
		{
			bool initializationSuccess = true;

			// Convert the custom class data to arrays suitable for AddParametricEffect
			amplitude = useAmplitude ? timeAmplitudePairs.SelectMany(pair => new double[] { pair.time, pair.value }).ToArray() : null;
			pitch = usePitch ? timePitchPairs.SelectMany(pair => new double[] { pair.time, pair.value }).ToArray() : null;
			transients = useTransients ? timeAmplitudeFrequencyTriplets.SelectMany(triplet => new double[] { triplet.time, triplet.amplitude, triplet.frequency }).ToArray() : null;

			// Now you can call AddParametricEffect with the converted arrays
			HapticMaterialId = HAR.AddParametricEffect(
				amplitude, amplitude?.Length ?? 0,
				pitch, pitch?.Length ?? 0,
				freqMin, freqMax,
				transients, transients?.Length ?? 0,
				isLooping
			);

			// Check if the ID is valid
			if (HapticMaterialId == -1)
			{
				DebugMode("Failed to create parametric haptic effect.");
				initializationSuccess = false;
			}
			else
			{
				DebugMode("Parametric haptic effect created with ID: " + HapticMaterialId);
			}
			return initializationSuccess;
		}

		// Overrides the base class Play method to use the haptic effect created on the fly
		public override void Play()
		{
			AddTarget(hapticBodyParts.Select(hapticBodyPart => new CommandData(Operator.Plus, hapticBodyPart.BodyPart, hapticBodyPart.Side)).ToList());
			base.Play();
		}
		public override void Stop()
		{
			base.Stop();
		}

		public override void PlayEventVibration()
		{
			AddTarget(hapticBodyParts.Select(hapticBodyPart => new CommandData(Operator.Plus, hapticBodyPart.BodyPart, hapticBodyPart.Side)).ToList());
			base.PlayEventVibration();
		}

	}
}