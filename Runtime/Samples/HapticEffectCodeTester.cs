using UnityEngine;
using Interhaptics.Core;
using Interhaptics.HapticBodyMapping;
using Interhaptics.Utils;
using Interhaptics.Platforms.Mobile;

namespace Interhaptics.Samples
{
/// <summary>
/// Class to test the different haptic effects in the Interhaptics SDK triggered by key presses on the code side (no Monobehaviour)
/// </summary>
	public class HapticEffectCodeTester : MonoBehaviour
	{
		public HapticMaterial hapticMaterial; // Assign this in the Unity Inspector
		public double intensity = 1.0; // Intensity of the haptic effect
		double[] transientTester = { 0.02, 1.0, 1.0, 0.5, 1.0, 1.0, 1.0, 1.0, 1.0 }; // Transient pattern
																	 // Define your amplitude, pitch, and transient values
		double[] amplitude = { 0.0, 1.0, 2.5, 1.0 }; // Constant amplitude
		double[] pitch = { 0.0, 0.0, 1.5, 1.0, 2.5, 0.0 }; // Pitch pattern
		double[] transient = { 3.0, 1.0, 1.0, 4.0, 1.0, 1.0 , 4.5, 1.0, 1.0}; // Transient pattern

		void Update()
		{
			// Trigger the haptic effect based on a condition, for example, a button press
			if (Input.GetKeyDown(KeyCode.K))
			{
				TestParametricHapticEffect();
			}

			if (Input.GetKeyDown(KeyCode.I))
			{
				TestAmplitude();
			}

			if (Input.GetKeyDown(KeyCode.U))
			{
				TestAmplitudesTransients();
			}

			if (Input.GetKeyDown(KeyCode.J))
			{
				TestTransient();
			}

			if (Input.GetKeyDown(KeyCode.H))
			{
				TestTransients();
			}

			if (Input.GetKeyDown(KeyCode.M))
			{
				TestConstantRight();
			}

			if (Input.GetKeyDown(KeyCode.L))
			{
				// Play the haptic effect
				HAR.PlayHapticEffect(hapticMaterial, 0.5, 2, 0f, LateralFlag.Global);
				//StartCoroutine(HAR.PlayHapticEffectCoroutine(hapticMaterial, 1.0, LateralFlag.Left, true, 3));
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				TestHapticEffect();
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				TestTransients();
			}

			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				StopHapticEffect();
			}
			// Detect key presses for numbers 1 to 9 and play corresponding haptic effect
			for (int i = 1; i <= 9; i++)
			{
				if (Input.GetKeyDown(KeyCode.Alpha0 + i))
				{
					HapticPreset.Play((HapticPreset.PresetType)(i - 1));
				}
			}
		}

		public void CodeTester()
		{ 
			HapticMaterial myHapticMaterial = null;
			if (myHapticMaterial != null)
			{
				HAR.PlayHapticEffect(myHapticMaterial);
				Debug.Log("Haptic effect played successfully!");
			}
			else
			{
				Debug.LogError("Haptic material is not assigned!");
			}
		}
		public void TestConstant()
		{
			HAR.PlayConstant(0.5, 0.5); // Plays a constant haptic effect with 50% amplitude for 0.5s.
		}

		public void TestConstantLoop()
		{
			double[] amplitude = { 0.0, 0.5, 0.5, 0.5 };
			//HAR.PlayParametricHapticEffectWithLoop(amplitude, null, 65, 300, null, 1.0, 2, LateralFlag.Global);
			HAR.PlayConstant(0.5, 0.5, 1, 2, LateralFlag.Global);
		}

		public void TestConstantRight()
		{
			HAR.PlayConstant(1.0, 5.0, _controllerSide: LateralFlag.Right);
		}
		public void TestParametricHapticEffect()
		{
			HAR.PlayAdvanced(amplitude, pitch, HAR.DEFAULT_FREQ_MIN, HAR.DEFAULT_FREQ_MAX, transient, 1.0, 2); // Intensity set to 1.0
		}
		public void TestHapticEffect()
		{
			HAR.PlayHapticEffect(hapticMaterial, 1.0, 1, 0f, LateralFlag.Global);
		}
		public void TestTransient()
		{
			// Set the time, amplitude, and pitch for the transient
			double time = 0.0; // Half a second after being called
			double amplitude = 1.0; // strong transient
			double pitch = 1.0; // High pitch

			// Optional parameters
			double intensity = 1.0; // Full intensity
			int loops = 1; // No looping
			LateralFlag controllerSide = LateralFlag.Global; // Play on both sides

			// Play the transient haptic effect
			HAR.PlayTransient(time, amplitude, pitch, intensity, loops, controllerSide);
			//HAR.PlayTransient(time, amplitude, pitch); same effect with default parameters
			//HAR.PlayTransient(); // Plays a transient immediately with the default values 1.0 and 1.0 for amplitude and pitch
		}
		public void TestTransients()
		{
			// Transients expressed as time amplitude pitch triplets
			double[] transient = {
									0.0, 1.0, 0.5,
									0.25, 0.75, 0.5,
									0.5, 0.5, 0.5,
									0.75, 0.25, 0.5
								 };
			HAR.PlayTransients(transient);
		}

		public void TestTransientsLoop()
		{
			// Transients expressed as time amplitude pitch triplets
			double[] transient = {
				0.0, 1.0, 0.5,
				0.25, 0.75, 0.5,
				0.5, 0.5, 0.5,
				0.75, 0.25, 0.5
			 };
			HAR.PlayTransients(transient, 1.0, 2, LateralFlag.Global);
			//HAR.PlayParametricHapticEffectWithLoop(null, null, 65, 300, transient, 1.0, 2, LateralFlag.Global);
		}
		public void TestAmplitude()
		{
			// Define an array of time-amplitude pairs
			HAR.Play(new double[] { 0, 1, 1, 0.0, 2, 1 }); // plays a vibrations starting at maximum amplitude, decrease at 0.5 at 1 second, and back to 1 at 2 seconds
		}

		public void TestAmplitudesLoop()
		{
			double[] amplitude = { 0, 1, 1, 0.0, 2, 1 };
			//	HAR.PlayParametricHapticEffectWithLoop(amplitude, null, 65, 300, null, 1.0, 2, LateralFlag.Global);
			HAR.Play(amplitude, 1.0, 2, LateralFlag.Global);
		}

		public void TestAmplitudesTransients()
		{
			// Amplitude array as time amplitude pairs
			double[] amplitude = {
									0.0, 0.5,
									2.0, 0.5,
								};
			// Transients expressed as time amplitude pitch triplets
			double[] transient = {
									0.5, 1, 0.5,
									1.5, 0.75, 0.5
								};
			HAR.Play(amplitude, transient, 1.0, 1, LateralFlag.Global); //Plays the complex pattern described by the arrays 2 times at intensity one on the left side. See Intensity Controls
		}

		public void TestAmplitudesTransientsLoop()
		{
			// Amplitude array as time amplitude pairs
			double[] amplitude = {
					0.0, 0.5,
					2.0, 0.5,
				};
			// Transients expressed as time amplitude pitch triplets
			double[] transient = {
					0.5, 1, 0.5,
					1.5, 0.75, 0.5
				};
			HAR.Play(amplitude, transient, 1.0, 2, LateralFlag.Global);
		}

		public void TestFrequency()
		{
			// Amplitude at 0.5 between 0 and 2 seconds
			double[] amplitudes = {
									1.0, 0.5,
									3.0, 0.5
									};
			// pitch between 0 and 1
			double[] pitch = {
								1.0, 0.0,
								3.0, 1.0
								};
			HAR.PlayAdvanced(amplitudes, pitch);
		}

		public void TestFrequencyLoop()
		{
			// Amplitude at 0.5 between 0 and 2 seconds
			double[] amplitudes = {
			1.0, 0.5,
			3.0, 0.5
			};
			// pitch between 0 and 1
			double[] pitch = {
			1.0, 0.0,
			3.0, 1.0
			};
			HAR.PlayAdvanced(amplitudes, pitch, 65, 300, null, 1.0, 2, LateralFlag.Global);
		}

		public void TestFrequencyTransients()
		{
			// Amplitude array
			double[] amplitudes = {
									0.0, 0.0,
									1.0, 1.0,
									2.0, 0.5,
									3.0, 0.25,
									4.0, 0.25
								};
			// pitch array
			double[] pitch = {
								0.0, 1,
								2.0, 0,
								4.0, 1
							};
			// transient array
			double[] transients = {
									0.5, 1, 0.5,
									3.5, 0.5, 0.5
								};
			double _fmin = 65;
			double _fmax = 300;
			HAR.PlayAdvanced(amplitudes, pitch, _fmin, _fmax, transients);
		}

		public void TestFrequencyTransientsLoop()
		{
			// Amplitude array
			double[] amplitudes = {
				0.0, 0.0,
				1.0, 1.0,
				2.0, 0.5,
				3.0, 0.25,
				4.0, 0.25
			};
			// pitch array
			double[] pitch = {
				0.0, 1,
				2.0, 0,
				4.0, 1
			};
			// transient array
			double[] transients = {
				0.5, 1, 0.5,
				3.5, 0.5, 0.5
			};
			double _fmin = 65;
			double _fmax = 300;
			HAR.PlayAdvanced(amplitudes, pitch, _fmin, _fmax, transients, 1.0, 2, LateralFlag.Global);
			//HAR.PlayParametricHapticEffectWithLoop(amplitudes, pitch, _fmin, _fmax, transients, 1.0, 2, LateralFlag.Global);
		}

		public void TestLongHaptics()
		{
			// Amplitude array
			double[] amplitudes = {
				0.0, 0.0,
				4.0, 1.0
			};
			// pitch array
			double[] pitch = {
				0.0, 0,
				4.0, 1
			};
			double _fmin = 65;
			double _fmax = 300;
			HAR.PlayAdvanced(amplitudes, pitch, _fmin, _fmax, null, 1.0, 1, LateralFlag.Global);
		}

		public void PlayStockHaptics(int i)
		{
			HapticPreset.Play((HapticPreset.PresetType)(i));
		}
		public void StopMobileHaptics()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			GenericAndroidHapticAbstraction.Cancel();
#endif
			MobileControl.StopEffects();
			HAR.MobileCancelHaptics();
		}
		public void StopHapticEffect()
		{
			HAR.StopAllEvents();
		}
	}

}
