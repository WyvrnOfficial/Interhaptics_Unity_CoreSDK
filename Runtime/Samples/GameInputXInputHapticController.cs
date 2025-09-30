/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using System;
using UnityEngine;
using Interhaptics.Samples;
using Interhaptics.Utils;

namespace Interhaptics.Platforms.GameInput.Samples
{
    public class GameInputXInputHapticController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Add EventHapticSources for vibrations on button press or event calls")]
        private EventHapticSource[] eventHapticSources;
        [SerializeField]
        [Tooltip("Add AudioHapticSources for vibrations on button press or event calls")]
        private AudioHapticSource[] audioHapticSources;
        [SerializeField]
        [Tooltip("Add SpatialHapticSources for vibrations on button press or collision/trigger calls")]
        private SpatialHapticSource[] spatialHapticSources;
        [SerializeField]
        [Tooltip("Add EventHapticSources for vibrations on index (GameInput exclusive feature)")]
        private EventHapticSource[] triggerEventHapticSources;
        [SerializeField]
        private TextMesh buttonPressed;
        [SerializeField]
        private TextMesh vibrationMaterialName;
        [SerializeField]
        private TextMesh vibrationType;
        [SerializeField]
        private int m_StickId = 1;
        [SerializeField]
        [Range(0, 6)]
        private int indexStiffness;
        [SerializeField]
        [Range(0, 2)]
        private int indexVibration;
		[SerializeField]
		public bool debugMode;
		private bool canModifyVibrationType = true;
		private AudioSource[] allAudioSources;
		private SpatialHapticSource[] allSpatialHapticSources;

		private void Start()
        {
            VibrationHapticSourceGUI();
        }

		/// <summary>
		/// Debug method to print messages in the console only when debugMode is enabled
		/// </summary>
		/// <param name="debugMessage"></param>
		public void DebugMode(string debugMessage)
		{
			if (debugMode)
			{
				Debug.Log(debugMessage);
			}
		}

		private void ResetHapticSources()
		{
			Core.HAR.StopAllEvents();
			foreach (EventHapticSource hapticSource in eventHapticSources)
			{
				hapticSource.Stop(); // Ensure the haptic effect is stopped.
				if (hapticSource.playingCoroutine != null)
				{
					StopCoroutine(hapticSource.playingCoroutine); // Stop the coroutine if it's running.
					hapticSource.playingCoroutine = null; // Clear the reference.
				}
				hapticSource.isPlaying = false; // Reset the isPlaying state.
			}
#if UNITY_6_0_OR_NEWER
			allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
			allSpatialHapticSources = FindObjectsByType<SpatialHapticSource>(FindObjectsSortMode.None); 
#else
			allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
			allSpatialHapticSources = FindObjectsOfType(typeof(SpatialHapticSource)) as SpatialHapticSource[];
#endif
			foreach (AudioSource audioSource in allAudioSources)
			{
				if (audioSource.isPlaying)
				{
					audioSource.Stop();
					Debug.Log("Audio Source stopped: " + audioSource);
					break;
				}
			}
			foreach (SpatialHapticSource spatialHapticSource in allSpatialHapticSources)
			{
				if (spatialHapticSource.GetComponent<ObjectTransform>().buttonPressed)
				{
					spatialHapticSource.GetComponent<ObjectTransform>().buttonPressed = false;
					spatialHapticSource.GetComponent<ObjectTransform>().ResetPosition();
					break;
				}
			}
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if ((!hasFocus) && (HapticManager.StopHapticsOnFocusLoss))
			{
				ResetHapticSources();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
            if (pauseStatus && HapticManager.StopHapticsOnFocusLoss)
            { 				
                ResetHapticSources();
            }

		}
		private void Update()
        {
            VibrationButtons();
			GameInputTriggers();
			ButtonsUI();
        }

		private void GameInputTriggers()
		{
			KeyCode[] joystickButtonCodes = {
		(KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + m_StickId + "Button4", true),
		(KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + m_StickId + "Button5", true)
			};
			for (int i = 0; i < joystickButtonCodes.Length; i++)
			{
				if (Input.GetKeyDown(joystickButtonCodes[i]))
				{
                    Debug.Log("Trigger " + i + " pressed");
                    triggerEventHapticSources[i].PlayEventVibration();
					vibrationMaterialName.text = triggerEventHapticSources[i].name;
				}
			}
		}

		private void VibrationButtons()
        {
            float dpadVertical = Input.GetAxis("dpad" + m_StickId + "_vertical");
            if (dpadVertical == 0)
            {
                canModifyVibrationType = true;
            }
            else
            {
                IncreaseIndexVibrations(dpadVertical > 0);
            }
            KeyCode[] joystickButtonCodes = {
        (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + m_StickId + "Button0", true),
        (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + m_StickId + "Button1", true),
        (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + m_StickId + "Button2", true),
        (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + m_StickId + "Button3", true)
            };
            for (int i = 0; i < joystickButtonCodes.Length; i++)
            {
                if (Input.GetKeyDown(joystickButtonCodes[i]))
                {
                    HapticVibrationController(indexVibration, i);
                }
            }
        }

        private void HapticVibrationController(int indexVibration, int indexButton)
        {
            switch (indexVibration)
            {
                case 0:
                    PlayAudioVibrationController(indexButton);
                    break;
                case 1:
                    PlayEventVibrationController(indexButton);
                    break;
                case 2:
                    PlaySpatialVibrationController(indexButton);
                    break;
                default:
                    Debug.LogWarning("Invalid input for indexVibration.");
                    break;
            }
        }

        public void PlayAudioVibrationController(int indexButton)
        {
            ResetHapticSources();
			audioHapticSources[indexButton].PlayEventVibration();
            vibrationMaterialName.text = audioHapticSources[indexButton].name;
        }

        public void PlayEventVibrationController(int indexButton)
        {
			ResetHapticSources();
			eventHapticSources[indexButton].PlayEventVibration();
            vibrationMaterialName.text = eventHapticSources[indexButton].name;
        }

        public void PlaySpatialVibrationController(int indexButton)
        {
			ResetHapticSources();
			spatialHapticSources[indexButton].GetComponent<ObjectTransform>().enabled = true;
            spatialHapticSources[indexButton].GetComponent<ObjectTransform>().buttonPressed = true;
            vibrationMaterialName.text = spatialHapticSources[indexButton].name;
        }

        public void IncreaseIndexVibrations(bool isIncrementing)
        {
            if (canModifyVibrationType)
            {
                indexVibration = isIncrementing ? (indexVibration + 1) % 3 : (indexVibration + 2) % 3;
                canModifyVibrationType = false;
            }
            VibrationHapticSourceGUI();
        }

        private void VibrationHapticSourceGUI()
        {
            switch (indexVibration)
            {
                case 0:
                    vibrationType.text = "Audio Haptic Source";
                    break;
                case 1:
                    vibrationType.text = "Event Haptic Source";
                    break;
                case 2:
                    vibrationType.text = "Spatial Haptic Source";
                    break;
                default:
                    Debug.LogWarning("Invalid input for Haptic Source.");
                    break;
            }
        }

        private void ButtonsUI()
        {
            string[] buttonNames = { "A | Cross", "B | Circle", "X | Square", "Y | Triangle", "Shoulder Left", "Shoulder Right" };
            string[] dpadNames = { "DPad left", "DPad right", "DPad down", "DPad up" };
            for (int i = 0; i < buttonNames.Length; i++)
            {
                if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + m_StickId + "Button" + i, true)))
                {
                    buttonPressed.text = buttonNames[i];
                    return;
                }
            }
            for (int i = 0; i < dpadNames.Length; i++)
            {
                if (i < 2 && Input.GetAxis("dpad" + m_StickId + "_horizontal") == (i == 0 ? -1 : 1))
                {
                    buttonPressed.text = dpadNames[i] + " " + Input.GetAxis("dpad" + m_StickId + "_horizontal");
                    return;
                }
                else if (i >= 2 && Input.GetAxis("dpad" + m_StickId + "_vertical") == (i == 2 ? -1 : 1))
                {
                    buttonPressed.text = dpadNames[i] + " " + Input.GetAxis("dpad" + m_StickId + "_vertical");
                    return;
                }
            }
        }
    }
}

