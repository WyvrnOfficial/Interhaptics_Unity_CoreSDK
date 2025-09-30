/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using UnityEngine.XR;

namespace Interhaptics.Platforms
{
    public class HapticCapabilityCheck : MonoBehaviour
    {
        [SerializeField]
        private bool debugMode = false;

        void Start()
        {
            // Check for iOS
#if UNITY_IOS
        if (UnityEngine.iOS.Device.generation > UnityEngine.iOS.DeviceGeneration.iPhone8 &&
            UnityEngine.iOS.Device.systemVersion.CompareTo("13") > 0)
        {
            DebugMode("Haptic capabilities supported on iOS.");
        }
        else
        {
            DebugMode("Haptic capabilities not supported on this iOS device.");
        }
#endif

            // Check for Android
#if !ENABLE_METAQUEST && !ENABLE_OPENXR && UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        bool hasVibrator = vibrator.Call<bool>("hasVibrator");

        if (hasVibrator)
        {
            DebugMode("Haptic capabilities supported on Android.");
        }
        else
        {
            DebugMode("Haptic capabilities not supported on this Android device.");
        }
#endif

// Check for Windows
#if UNITY_STANDALONE_WIN
        if (Input.GetJoystickNames().Length > 0)
        {
            DebugMode("GameInput/XInput controller connected on Windows. Haptic feedback enabled.");
        }
        else
        {
            DebugMode("No GameInput/XInput controller connected on Windows. No haptic feedback.");
        }
#endif

// Check for Meta Quest/Open XR
#if ENABLE_METAQUEST || ENABLE_OPENXR
        if (XRSettings.enabled)
        {
            DebugMode("Meta Quest/Open XR enabled.");
        }
        else
        {
            DebugMode("Meta Quest/Open XR not enabled.");
        }
#endif

// Check for PS5
#if UNITY_PS5
        DebugMode("Platform is PS5. Haptic feedback enabled.");
#endif

// Check for Nintendo Switch
#if UNITY_SWITCH
		DebugMode("Platform is Nintendo Switch. Haptic feedback enabled.");
#endif
		}
		public void DebugMode(string message)
        {
            if (debugMode)
            {
                Debug.Log(message);
            }
        }
	}
}
