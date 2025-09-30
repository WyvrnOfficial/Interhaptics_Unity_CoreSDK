/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

#if !ENABLE_METAQUEST && !ENABLE_OPENXR && UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using Interhaptics.HapticBodyMapping;
using System.Collections;
using System.Collections.Generic;

namespace Interhaptics.Platforms.Android
{
    public sealed class AndroidProvider : IHapticProvider
    {
#region HAPTIC CHARACTERISTICS FIELDS
        private const string DISPLAY_NAME = "Android";
        private const string DESCRIPTION = "Android device";
        private const string MANUFACTURER = "Google";
        private const string VERSION = "1.0";
        private const int SAMPLERATE = 100;
        private const int AMPLITUDE_DISTANCE = 5; //previously 0
        private ulong lastBufferStartingTime = 0;
        private bool hapticPlaying= false;
        private float? expectedEndTimestamp = null;
        private BodyPartID Hand= BodyPartID.Bp_Left_palm;
        private const EProtocol PROTOCOL = EProtocol.Clips;
        private int myAvatarID = -1;
#endregion

#region HAPTIC CHARACTERISTICS GETTERS
        [UnityEngine.Scripting.Preserve]
        public string DisplayName()
        {
            return DISPLAY_NAME;
        }

        [UnityEngine.Scripting.Preserve]
        public string Description()
        {
            return DESCRIPTION;
        }

        [UnityEngine.Scripting.Preserve]
        public string Manufacturer()
        {
            return MANUFACTURER;
        }

        [UnityEngine.Scripting.Preserve]
        public string Version()
        {
            return VERSION;
        }
#endregion

#region PROVIDER LOOP

        [UnityEngine.Scripting.Preserve]
        public bool Init()
        {
#if !ENABLE_IL2CPP && UNITY_ANDROID && !UNITY_EDITOR
        UnityEngine.Debug.LogError("Interhaptics requires IL2CPP scripting backend for Android. Please change it in Player Settings. Haptics will not play on the Mono scripting backend on the Android platform." + "Source: Android Provider");
        HapticManager.MonoScriptingBackend = true;
        return false;
#endif

#if !UNITY_EDITOR && UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR
//Added a way to disable the provider if the device does not have haptic capabilities - TODO: Update when Android haptic controllers are available (DirectInput)
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        bool hasVibrator = vibrator.Call<bool>("hasVibrator");
        if (hasVibrator)
        {
            Debug.Log("Haptic capabilities supported on Android.");
        }
        else
        {
            Debug.Log("Haptic capabilities not supported on this Android device.");
        }
		if (!SystemInfo.supportsVibration)
		{
			UnityEngine.Debug.LogError("The device does not have haptic capabilities. Haptics will not play on this device. ");
        // Attempt to log Android version, API, and device model - corrected access to Android properties
        // Note: Correct usage involves using AndroidJavaObject and AndroidJavaClass for accessing Android-specific information
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            string androidVersion = version.GetStatic<string>("RELEASE");
            int apiLevel = version.GetStatic<int>("SDK_INT");
            using (var build = new AndroidJavaClass("android.os.Build"))
            {
                string deviceModel = build.GetStatic<string>("MODEL");
                Debug.LogError($"Android version: {androidVersion} API: {apiLevel} Device model: {deviceModel}");
            }
        }
			return false;
		}
#endif

            Interhaptics.Platforms.Mobile.GenericAndroidHapticAbstraction.Initialize();
            myAvatarID = Core.HAR.CreateBodyPart(Perception.Vibration, Hand, 1, 1, 1, SAMPLERATE, false, false, false, PROTOCOL);
            if (HapticManager.DebugSwitch)
			{
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                string androidVersion = version.GetStatic<string>("RELEASE");
                int apiLevel = version.GetStatic<int>("SDK_INT");
                using (var build = new AndroidJavaClass("android.os.Build"))
                    {
                        string deviceModel = build.GetStatic<string>("MODEL");
                        Debug.Log($"Android haptic provider started. Android version: {androidVersion} API: {apiLevel} Device model: {deviceModel}");
                    }
                }
            }
            return true;
        }

        [UnityEngine.Scripting.Preserve]
        public bool IsPresent()
        {
            return Interhaptics.Platforms.Mobile.GenericAndroidHapticAbstraction.HasVibrator();
        }

        [UnityEngine.Scripting.Preserve]
        public bool Clean()
        {
            return true;
        }

        private bool IsEqual(int _amplitudeA, int _amplitudeB, int _distance = 0)
        {
            return Mathf.Abs(_amplitudeA - _amplitudeB) <= _distance;
        }

        [UnityEngine.Scripting.Preserve]
        public void RenderHaptics()
        {
            ulong startingTime = Core.HAR.GetVectorStartingTime(myAvatarID, Perception.Vibration, Hand, 0, 0, 0);
            //if something to update
            if (startingTime!= lastBufferStartingTime)
            {
                lastBufferStartingTime = startingTime;
                int size = Core.HAR.GetOutputBufferSize(myAvatarID, Perception.Vibration, Hand, 0, 0, 0, BufferDataType.Amplitude);
                if (size > 0)
                {  
                    double[] outputBuffer = new double[size];
                    //getting haptic amplitude buffer to play
                    Core.HAR.GetOutputBuffer(outputBuffer, size, myAvatarID, Perception.Vibration, Hand, 0, 0, 0, BufferDataType.Amplitude);
                    List<int> AndroidOutputBuffer = new List<int>();
                    List<long> timeBuffer = new List<long>();
                    long step = Mathf.RoundToInt(1000.0f / SAMPLERATE);
                    int lastAmplitude = -1;
                    int currentAmplitude = -1;
                    long currentTiming = 0;
                    //creating pattern array and casting haptic data into the correct range (0 <-> 255)
                    for (int i = 0; i < size; i++)
                    {
                        currentAmplitude = Mathf.RoundToInt((float)(outputBuffer[i] * 255));
                        if (i != 0 && IsEqual(currentAmplitude, lastAmplitude, AMPLITUDE_DISTANCE)) //compare with a distance of 0 (between 0 and 255)
                        {
                            currentTiming += step;
                            continue;
                        }
                        else
                        {
                            AndroidOutputBuffer.Add(currentAmplitude);
                            lastAmplitude = currentAmplitude;
                            if (i != 0)//we dont want to add the duration on the first iteration
                            {
                                timeBuffer.Add(currentTiming + step);
                                currentTiming = 0;
                            }
                        }
                    }
                    timeBuffer.Add(currentTiming + step);

                    //sending haptic data to device
                    Interhaptics.Platforms.Mobile.GenericAndroidHapticAbstraction.Vibrate(timeBuffer.ToArray(), AndroidOutputBuffer.ToArray(),-1,true);
                    hapticPlaying = true;
                    expectedEndTimestamp = UnityEngine.Time.realtimeSinceStartup+((float)size / (float)SAMPLERATE);
                }
            }
            else 
            {
                if (hapticPlaying && UnityEngine.Time.realtimeSinceStartup> expectedEndTimestamp)
                {
                    //Stop haptic at the end of the effect
                    Interhaptics.Platforms.Mobile.GenericAndroidHapticAbstraction.Cancel();
                    hapticPlaying = false;
                
                }
            }
        }
#endregion
    }
}
#endif