/* ​
* Copyright (c) 2023 Go Touch VR SAS. All rights reserved. ​
* ​
*/

using System.Collections.Generic;
using UnityEngine;
using Interhaptics.HapticBodyMapping;
using Interhaptics.Platforms.Mobile;
using System.Collections;

namespace Interhaptics.Core
{
	public static partial class HAR
	{

		#region Constants
		public const double DEFAULT_FREQ_MIN = 65.0;
		public const double DEFAULT_FREQ_MAX = 300.0;
		public const double DEFAULT_INTENSITY = 1.0;
		public const int DEFAULT_LOOPS = 1;
		public const double DELAY_COMPENSATION = 0.02;
		public const LateralFlag DEFAULT_CONTROLLER_SIDE = LateralFlag.Global;
		#endregion

		// Flag to indicate if the haptic effect should be stopped
		public static bool stopHapticEffect = false;

		#region Enums
		public enum HMaterial_VersionStatus
		{
			NoAnHapticsMaterial = 0,
			V3_NeedToBeReworked = 1,
			V4_Current = 2,
			UnknownVersion = 3
		}
		#endregion

		#region Haptic Material/Effect Management
		private static string parseMaterial(UnityEngine.TextAsset _material)
		{
			if (_material == null)
			{
				return "";
			}
			return _material.text;
		}

		private static string parseMaterial(HapticMaterial _material)
		{
			if (_material == null)
			{
				return "";
			}
			return _material.text;
		}

		/// <summary>
		/// Adds a Haptic Material to the system directly from a HAPS compliant JSON string. Useful for loading from a file.from StreamingAssets or Resources.
		/// </summary>
		/// <param name="jsonContent"></param>
		/// <returns></returns>
		public static int AddHMString(string jsonContent) 
		{
			return HAR.AddHM(jsonContent); 
		}

		/// <summary>
		/// Adds a Haptic Material to the system.
		/// </summary>
		/// <param name="_material">Material in Unity TextAsset format.</param>
		/// <returns>ID of the added haptic material.</returns>
		public static int AddHM(UnityEngine.TextAsset _material)
		{
			return AddHM(parseMaterial(_material));
		}

		/// <summary>
		/// Adds a Haptic Material to the system.
		/// </summary>
		/// <param name="_material">Material in HapticMaterial format.</param>
		/// <returns>ID of the added haptic material.</returns>
		public static int AddHM(HapticMaterial _material)
		{
			return AddHM(parseMaterial(_material));
		}

		/// <summary>
		/// Updates a Haptic Material in the system.
		/// </summary>
		/// <param name="_id">ID of the haptic material to update.</param>
		/// <param name="_material">Updated material in Unity TextAsset format.</param>
		/// <returns>True if update is successful.</returns>
		public static bool UpdateHM(int _id, UnityEngine.TextAsset _material)
		{
			return UpdateHM(_id, parseMaterial(_material));
		}

		/// <summary>
		/// Updates a Haptic Material in the system.
		/// </summary>
		/// <param name="_id">ID of the haptic material to update.</param>
		/// <param name="_material">Updated material in HapticMaterial format.</param>
		/// <returns>True if update is successful.</returns>
		public static bool UpdateHM(int _id, HapticMaterial _material)
		{
			return UpdateHM(_id, parseMaterial(_material));
		}
		#endregion

		/// <summary>
		/// Adds a target to a specific haptic event.
		/// </summary>
		/// <param name="_hMaterialId">ID of the haptic material.</param>
		/// <param name="_target">List of CommandData representing the target.</param>
		public static void AddTargetToEvent(int _hMaterialId, List<CommandData> _target)
		{
			AddTargetToEventMarshal(_hMaterialId, _target.ToArray(), _target.Count);
		}

		/// <summary>
		/// Updates event positions for a specific haptic event.
		/// </summary>
		/// <param name="_hMaterialId">ID of the haptic effect.</param>
		/// <param name="_target">List of CommandData representing the target.</param>
		/// <param name="_texturePosition">New texture position.</param>
		/// <param name="_stiffnessPosition">New stiffness position.</param>
		public static void UpdateEventPositions(int _hMaterialId, List<CommandData> _target, double _texturePosition, double _stiffnessPosition)
		{
			UpdateEventPositionsMarshal(_hMaterialId, _target.ToArray(), _target.Count, _texturePosition, _stiffnessPosition);
		}

		/// <summary>
		/// Removes a target from a specific haptic event.
		/// </summary>
		/// <param name="_hMaterialId">ID of the haptic material.</param>
		/// <param name="_target">List of CommandData representing the target.</param>
		public static void RemoveTargetFromEvent(int _hMaterialId, List<CommandData> _target)
		{
			RemoveTargetFromEventMarshal(_hMaterialId, _target.ToArray(), _target.Count);
		}

		/// <summary>
		/// Sets the intensity for a specific target of a haptic event.
		/// </summary>
		/// <param name="_hMaterialId">ID of the haptic material.</param>
		/// <param name="_target">List of CommandData representing the target.</param>
		/// <param name="_intensity">Intensity value.</param>
		public static void SetTargetIntensity(int _hMaterialId, List<CommandData> _target, double _intensity)
		{
			SetTargetIntensityMarshal(_hMaterialId, _target.ToArray(), _target.Count, _intensity);
		}

		/// <summary>
		/// Debug method for printing messages in the console. Debug mode must be enabled in the HapticManager.
		/// </summary>
		/// <param name="message"></param>
		public static void DebugAPIMode(string message)
		{
			if (HapticManager.DebugSwitch)
			{
				Debug.Log(message);
			}
		}

		/// <summary>
		/// Plays a haptic effect using a specified haps file. 
		/// </summary>
		/// <param name="material"></param>
		/// <param name="intensity"></param>
		/// <param name="controllerSide"></param>
		/// <param name="loops"></param>
		public static void PlayHapticEffect(HapticMaterial material, double intensity = DEFAULT_INTENSITY, int loops = DEFAULT_LOOPS, float vibrationOffset = 0f, LateralFlag controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileControl.StopEffects();
			HAR.StopAllEvents();
#endif
			int hMaterialId = AddHM(material);
			if (hMaterialId == -1)
			{
				DebugAPIMode("PlayHapticEffect: Failed to add haptic effect.");
				return;
			}
			List<CommandData> targets = new List<CommandData> { new CommandData(Operator.Plus, GroupID.Palm, controllerSide) };
			AddTargetToEvent(hMaterialId, targets);
			SetEventIntensity(hMaterialId, intensity);
			SetEventLoop(hMaterialId, loops);
			PlayEvent(hMaterialId, (double)-Time.realtimeSinceStartup, 0, 0);
			double duration = HAR.GetVibrationLength(hMaterialId);
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileControl.EnqueueEffect(hMaterialId, duration, loops, intensity, vibrationOffset);
			HAR.PlayEvent(hMaterialId, (double)-Time.realtimeSinceStartup, 0, 0);
#endif
			DebugAPIMode($"PlayHapticEffect: Enqueued haptic effect with Material ID {hMaterialId}, Loops {loops}, Intensity {intensity}, Controller Side {controllerSide}.");
		}

		/// <summary>
		/// Plays a haptic effect using a specified haps file's id.
		/// </summary>
		/// <param name="hMaterialId">The id of the haptic effect </param>
		/// <param name="intensity">The intensity of the effect</param>
		/// <param name="loops">The number of loops</param>
		/// <param name="controllerSide">The side of the controller</param>
		public static void PlayHapticEffectId(int hMaterialId, double intensity = DEFAULT_INTENSITY, int loops = DEFAULT_LOOPS, float vibrationOffset = 0f, LateralFlag controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileControl.StopEffects();
			HAR.StopAllEvents();
#endif
			if (hMaterialId == -1)
			{
				DebugAPIMode("PlayHapticEffectId: Failed to add haptic material.");
				return;
			}
			double duration = HAR.GetVibrationLength(hMaterialId);
			DebugAPIMode("Playing Haptic Effect id" + hMaterialId);
			List<CommandData> targets = new List<CommandData> { new CommandData(Operator.Plus, GroupID.Palm, controllerSide) };
			AddTargetToEvent(hMaterialId, targets);
			SetEventIntensity(hMaterialId, intensity);
			SetEventLoop(hMaterialId, loops);
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileControl.EnqueueEffect(hMaterialId, duration, loops, intensity, vibrationOffset);
#else
			HAR.PlayEvent(hMaterialId, (double)-Time.realtimeSinceStartup - vibrationOffset, 0, 0);
#endif
			DebugAPIMode($"PlayHapticEffectId: Enqueued haptic effect with Material ID {hMaterialId}, Loops {loops}, Intensity {intensity}, Controller Side {controllerSide}.");
		}

		public static void PlayParametricHapticEffect(double[] _amplitude, double[] _pitch, double _freqMin, double _freqMax, double[] _transient, double _intensity, int _loops, LateralFlag _controllerSide)
		{
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			HAR.StopAllEvents();
			MobileControl.StopEffects();
			//only one event allowed for mobile
#endif
			if (_transient != null)
			{
				if (_transient[0] == 0.0)
				{   // If the first transient is at time 0, add a small delay to compensate for the delay in the system
					_transient[0] = DELAY_COMPENSATION;
				}
			}
			//Default values for frequency min and max
			int hMaterialId = AddParametricEffect(
				_amplitude, _amplitude != null ? _amplitude.Length : 0,
				_pitch, _pitch != null ? _pitch.Length : 0,
				_freqMin, _freqMax,
				_transient, _transient != null ? _transient.Length : 0,
				_loops > 0
			);
			if (hMaterialId == -1)
			{
				DebugAPIMode("PlayParametricHapticEffect: Failed to create parametric effect.");
				return;
			}

			List<CommandData> targets = new List<CommandData> { new CommandData(Operator.Plus, GroupID.Palm, _controllerSide) };
			AddTargetToEvent(hMaterialId, targets);
			SetEventIntensity(hMaterialId, _intensity);
			DebugAPIMode("PlayParametricHapticEffect: Event played at " + Time.realtimeSinceStartup);
			double vibrationLength = GetVibrationLength(hMaterialId); // Obtain the duration of the effect
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileControl.EnqueueEffect(hMaterialId, vibrationLength, _loops, _intensity);
#else
			HAR.PlayEvent(hMaterialId, (double)-Time.realtimeSinceStartup, 0, 0);
#endif
		}

		/// <summary>
		/// Plays a parametric haptic effect using specified amplitude, pitch, and transient parameters.
		/// </summary>
		/// <param name="_amplitude">Array of amplitude values formatted as Time - Value pairs, with values between 0 and 1.</param>
		/// <param name="_pitch">Array of pitch values formatted as Time - Value pairs, with values between 0 and 1.</param>
		/// <param name="_transient">Array of transient values formatted as Time - Amplitude - Frequency triples, with values between 0 and 1.</param>
		/// <param name="_freqMin">Minimum frequency of the haptic effect.</param>
		/// <param name="_freqMax">Maximum frequency of the haptic effect.</param>
		/// <param name="_intensity">Intensity of the haptic effect.</param>
		/// <param name="_loops">Number of loops to play.</param>
		/// <param name="_controllerSide">Side of the controller for the haptic effect.</param>
		public static void PlayAdvanced(double[] _amplitude, double[] _pitch, double _freqMin = DEFAULT_FREQ_MIN, double _freqMax = DEFAULT_FREQ_MAX, double[] _transient = null, double _intensity = DEFAULT_INTENSITY, int _loops = DEFAULT_LOOPS, LateralFlag _controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
			PlayParametricHapticEffect(_amplitude, _pitch, _freqMin, _freqMax, _transient, _intensity, _loops, _controllerSide);
		}

		/// <summary>
		/// Plays haptic effects based on provided arrays of amplitudes and transient triplets.
		/// </summary>
		/// <param name="amplitudes">Array of amplitude values, with values between 0 and 1.</param>
		/// <param name="transients">Array of transient triplets formatted as Time - Amplitude - Frequency, with values between 0 and 1.</param>
		/// <param name="_intensity">Intensity of the haptic effect.</param>
		/// <param name="_loops">Number of times to loop the effect.</param>
		/// <param name="_controllerSide">Side of the controller for the haptic effect.</param>
		public static void Play(double[] amplitudes, double[] transients, double _intensity = DEFAULT_INTENSITY, int _loops = DEFAULT_LOOPS, LateralFlag _controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
			DebugAPIMode("PlayAmplitudesTransients: Playing amplitudes and transients at " + Time.realtimeSinceStartup);

			PlayAdvanced(
				amplitudes, // The amplitude array
				null, // No pitch 
				DEFAULT_FREQ_MIN, // Default frequency min
				DEFAULT_FREQ_MAX, // Default frequency max
				transients, // The transient triplets
				_intensity,  // Intensity of the effects
				_loops, // Number of loops
				_controllerSide // Controller side
			);
		}

		/// <summary>
		/// Plays multiple amplitude-based haptic effects based on provided time-amplitudes pairs.
		/// </summary>
		/// <param name="amplitudes">Array of time - amplitude values, with amplitude values between 0 and 1.</param>
		/// <param name="_intensity">Intensity of the haptic effect.</param>
		/// <param name="_loops">Number of times to loop the effect.</param>
		public static void Play(double[] amplitudes, double _intensity = DEFAULT_INTENSITY, int _loops = DEFAULT_LOOPS, LateralFlag _controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
			DebugAPIMode("Play: Playing amplitudes at " + Time.realtimeSinceStartup);

			Play(
				amplitudes, // The amplitude array
				null, // No transients 
				_intensity,  // Intensity of the effects
				_loops, // Number of loops
				_controllerSide // Controller side
			);
		}

		/// <summary>
		/// Plays multiple transient haptic effects based on provided triplets of time, amplitude, and frequency. (Loops number optional)
		/// </summary>
		/// <param name="transients">Array of transient triplets formatted as Time - Amplitude - Frequency, with values between 0 and 1.</param>
		/// <returns>A coroutine that plays the transient haptic effects.</returns>
		/// <example>
		/// <code>
		/// StartCoroutine(HapticEffectPlayer.PlayTransients(new double[] { 0.5, 0.8, 0.6, 1.0, 1.0, 0.5 }));
		/// </code>
		/// This would play two transient effects: the first half a second after the method is called with 0.8 amplitude and 0.6 frequency, and the second one second after the method is called with full amplitude and 0.5 frequency.
		/// </example>
		public static void PlayTransients(double[] transients, double _intensity = DEFAULT_INTENSITY, int _loops = DEFAULT_LOOPS, LateralFlag _controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
			// Debug message with timestamp for when the transients play
			DebugAPIMode("PlayTransients: Playing transients at " + Time.realtimeSinceStartup);
			// This call should include only the transient array, as we're only interested in playing transients.
			PlayAdvanced(
				null, // No amplitude 
				null, // No pitch 
				DEFAULT_FREQ_MIN, // Default frequency min
				DEFAULT_FREQ_MAX, // Default frequency max
				transients, // The transient triplets
				_intensity,  // Intensity of the transient effects
				_loops, // Number of loops
				_controllerSide // Controller side
			);
		}

		/// <summary>
		/// Plays a single transient haptic effect at the specified time, amplitude, and frequency.
		/// </summary>
		/// <param name="time">The time at which the transient should occur, in seconds.</param>
		/// <param name="amplitude">The amplitude of the transient effect, between 0 and 1.</param>
		/// <param name="frequency">The frequency of the transient effect, between 0 and 1.</param>
		/// <returns>A coroutine that plays the transient haptic effect.</returns>
		public static void PlayTransient(double time = DELAY_COMPENSATION, double amplitude = 1.0, double frequency = 1.0, double _intensity = DEFAULT_INTENSITY, int _loops = DEFAULT_LOOPS, LateralFlag _controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
			DebugAPIMode("PlayTransient: Playing transient at " + Time.realtimeSinceStartup);
			double[] transient = { time, amplitude, frequency };
			// Call the PlayParametricHapticEffect coroutine with the transient parameters
			PlayTransients(
				transient, // The transient parameters
				_intensity,
				_loops, // intensity and loops optional
				_controllerSide
			);
		}

		/// <summary>
		/// Plays a single constant haptic effect at the specified amplitude and for a specified time duration.
		/// </summary>
		/// <param name="amplitude"></param>
		/// <param name="time"></param>
		public static void PlayConstant(double amplitude, double time, double _intensity = DEFAULT_INTENSITY, int _loops = DEFAULT_LOOPS, LateralFlag _controllerSide = DEFAULT_CONTROLLER_SIDE)
		{
			double[] amplitudes = { 0.0, amplitude, time, amplitude };
			Play(amplitudes, _intensity, _loops, _controllerSide);
		}

		private static int AndroidVersion
		{
			get
			{
				using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
				{
					return version.GetStatic<int>("SDK_INT");
				}
			}
		}
		public static void MobileCancelHaptics()
		{
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileControl.StopEffects();
#endif
			HAR.StopAllEvents();
			//stopHapticEffect = true; 			// Set the stopHapticEffect flag to true
		}

		/// <summary>
		/// Stops the previous haptic effect.
		/// </summary>
		private static void StopPreviousHapticEffect()
		{
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileCancelHaptics();
			// Set the stopHapticEffect flag to true
			stopHapticEffect = true;
#else
			HAR.StopAllEvents();
#endif
		}

		/// <summary>
		/// Stops the current haptic effect.
		/// </summary>
		public static void StopCurrentHapticEffect(int hapticMaterialId)
		{
#if (UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR) || UNITY_IOS
			MobileCancelHaptics();
#else
			HAR.StopEvent(hapticMaterialId);
#endif
		}
	}
}
