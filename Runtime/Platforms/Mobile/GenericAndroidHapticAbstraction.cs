/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

namespace Interhaptics.Platforms.Mobile
{

    public class GenericAndroidHapticAbstraction
    {
        // Component Parameters
        public static logLevel LogLevel = logLevel.Warning;

        // Can be modified to tune the experience
        // Can be changed in runtime
        public static int AmplitudeThreshold = 100;

        // Vibrator References
        private static UnityEngine.AndroidJavaObject vibrator = null;
        private static UnityEngine.AndroidJavaClass vibrationEffectClass = null;
        private static int defaultAmplitude = 255;

        // Api Level
        private static int ApiLevel = 1;
        private static bool doesSupportVibrationEffect() => ApiLevel >= 26;    // available only from Api >= 26
        private static bool doesSupportPredefinedEffect() => ApiLevel >= 29;   // available only from Api >= 29

        #region Initialization
        private static bool isInitialized = false;

        public static void Initialize()
        {
            // load references safely
            if (isInitialized == false && UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
            {
                // Add APP VIBRATION PERMISSION to the Manifest
            #if UNITY_ANDROID
            if (UnityEngine.Application.isConsolePlatform)
            {
                UnityEngine.Handheld.Vibrate();
            }
            #endif
                // Get Api Level
                using (UnityEngine.AndroidJavaClass androidVersionClass = new UnityEngine.AndroidJavaClass("android.os.Build$VERSION"))
                {
                    ApiLevel = androidVersionClass.GetStatic<int>("SDK_INT");
                }

                // Get UnityPlayer and CurrentActivity
                using (UnityEngine.AndroidJavaClass unityPlayer = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (UnityEngine.AndroidJavaObject currentActivity = unityPlayer.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity"))
                {
                    if (currentActivity != null)
                    {
                        vibrator = currentActivity.Call<UnityEngine.AndroidJavaObject>("getSystemService", "vibrator");

                        // if device supports vibration effects, get corresponding class
                        if (doesSupportVibrationEffect())
                        {
                            vibrationEffectClass = new UnityEngine.AndroidJavaClass("android.os.VibrationEffect");
                            defaultAmplitude = UnityEngine.Mathf.Clamp(vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE"), -1, 255);
                        }

                        // if device supports predefined effects, get their IDs
                        if (doesSupportPredefinedEffect())
                        {
                            PredefinedEffect.EFFECT_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_CLICK");
                            PredefinedEffect.EFFECT_DOUBLE_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_DOUBLE_CLICK");
                            PredefinedEffect.EFFECT_HEAVY_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_HEAVY_CLICK");
                            PredefinedEffect.EFFECT_TICK = vibrationEffectClass.GetStatic<int>("EFFECT_TICK");
                        }
                    }
                }

                logAuto("Vibration component initialized", logLevel.Info);
                isInitialized = true;
            }
        }
        #endregion

        #region Vibrate Public
        /// <summary>
        /// Vibrate for Milliseconds, with Amplitude (if available).
        /// If amplitude is -1, amplitude is Disabled. If -1, device DefaultAmplitude is used. Otherwise, values between 1-255 are allowed.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void Vibrate(long milliseconds, int amplitude = -1, bool cancel = false)
        {
            string funcToStr() => string.Format("Vibrate ({0}, {1}, {2})", milliseconds, amplitude, cancel);
            Initialize(); // make sure script is initialized
            if (isInitialized == false)
            {
                logAuto(funcToStr() + ": Not initialized", logLevel.Warning);
            }
            else if (HasVibrator() == false)
            {
                logAuto(funcToStr() + ": Device doesn't have Vibrator", logLevel.Warning);
            }
            else
            {
                if (cancel)
                {
                    Cancel();
                }
                if (doesSupportVibrationEffect())
                {
                    // validate amplitude
                    amplitude = UnityEngine.Mathf.Clamp(amplitude, -1, 255);
                    if (amplitude == -1)
                    {
                        amplitude = 255; // if -1, disable amplitude (use maximum amplitude)
                    }
                    if (amplitude != 255 && HasAmplitudeControl() == false)
                    { // if amplitude was set, but not supported, notify developer
                        logAuto(funcToStr() + ": Device doesn't have Amplitude Control, but Amplitude was set", logLevel.Warning);
                    }
                    //if (amplitude == 0) amplitude = defaultAmplitude; // if 0, use device DefaultAmplitude

                    // if amplitude is not supported, use 255; if amplitude is -1, use systems DefaultAmplitude. Otherwise use user-defined value.
                    amplitude = HasAmplitudeControl() == false ? 255 : amplitude;
                    vibrateEffect(milliseconds, amplitude);
                    logAuto(funcToStr() + ": Effect called", logLevel.Info);
                }
                else
                {
                    vibrateLegacy(milliseconds);
                    logAuto(funcToStr() + ": Legacy called", logLevel.Info);
                }
            }
        }

        /// <summary>
        /// Vibrate Pattern (pattern of durations, with format Off-On-Off-On and so on).
        /// Amplitudes can be Null (for default) or array of Pattern array length with values between 1-255.
        /// To cause the pattern to repeat, pass the index into the pattern array at which to start the repeat, or -1 to disable repeating.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void Vibrate(long[] pattern, int[] amplitudes = null, int repeat = -1, bool cancel = false)
        {
            string funcToStr() => string.Format("Vibrate (pattern, amplitudes, repeat, cancel)");

            Initialize(); // make sure script is initialized
            if (isInitialized == false)
            {
                logAuto(funcToStr() + ": Not initialized", logLevel.Warning);
            }
            else if (HasVibrator() == false)
            {
                logAuto(funcToStr() + ": Device doesn't have Vibrator", logLevel.Warning);
            }
            else
            {
                // check Amplitudes array length
                if (amplitudes != null && amplitudes.Length != pattern.Length)
                {
                    logAuto(funcToStr() + ": Length of Amplitudes array is not equal to Pattern array. Amplitudes will be ignored.", logLevel.Warning);
                    amplitudes = null;
                }
                // limit amplitudes between 1 and 255
                if (amplitudes != null)
                {
                    clampAmplitudesArray(amplitudes);
                }

                // vibrate
                if (cancel)
                {
                    Cancel();
                }
                if (doesSupportVibrationEffect())
                {
                    if (amplitudes != null && HasAmplitudeControl() == true)
                    {
                        //Case High Capabilities
                        vibrateEffect(pattern, amplitudes, repeat);
                        logAuto(funcToStr() + ": Effect with amplitudes called", logLevel.Info);
                        return;
                    }
                }

                logAuto(funcToStr() + ": called but device has no amplitude control", logLevel.Warning);
                //Case Medium & low capabilities
                vibrateEffect(TimingsFromAmplitudes(amplitudes, pattern[1]), repeat);
            }
        }

        /// <summary>
        /// Vibrate predefined effect (described in Vibration.PredefinedEffect). Available from Api Level >= 29.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void VibratePredefined(int effectId, bool cancel = false)
        {
            string funcToStr() => string.Format("VibratePredefined ({0})", effectId);

            Initialize(); // make sure script is initialized
            if (isInitialized == false)
            {
                logAuto(funcToStr() + ": Not initialized", logLevel.Warning);
            }
            else if (HasVibrator() == false)
            {
                logAuto(funcToStr() + ": Device doesn't have Vibrator", logLevel.Warning);
            }
            else if (doesSupportPredefinedEffect() == false)
            {
                logAuto(funcToStr() + ": Device doesn't support Predefined Effects (Api Level >= 29)", logLevel.Warning);
            }
            else
            {
                if (cancel)
                {
                    Cancel();
                }
                vibrateEffectPredefined(effectId);
                logAuto(funcToStr() + ": Predefined effect called", logLevel.Info);
            }
        }
        #endregion

        #region Public Properties & Controls
        public static float PulseFromBuffer(int[] _buffer)
        {
            if (_buffer.Length > 0)
            {
                float result = 0;
                for (int i = 0; i < _buffer.Length; i++)
                {
                    result += _buffer[i];
                }

                return UnityEngine.Mathf.Clamp(result / _buffer.Length, 0, 255);
            }

            return 0;
        }

        public static long[] ParsePattern(string pattern)
        {
            if (pattern == null)
            {
                return new long[0];
            }
            pattern = pattern.Trim();
            string[] split = pattern.Split(',');

            long[] timings = new long[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                if (int.TryParse(split[i].Trim(), out int duration))
                {
                    timings[i] = duration < 0 ? 0 : duration;
                }
                else
                {
                    timings[i] = 0;
                }
            }

            return timings;
        }

        public static long[] TimingsFromAmplitudes(int[] amplitudes, long amplitudeDuration)
        {
            if (amplitudes == null)
            {
                return new long[] { 0, 0 };
            }

            System.Collections.Generic.List<long> timings = new System.Collections.Generic.List<long>();

            int currentAmplitudeIndex = 0;
            long currentTimingValue = 0;

            while (currentAmplitudeIndex < amplitudes.Length)
            {
                //get duration under threshold
                currentTimingValue = 0;
                while (currentAmplitudeIndex < amplitudes.Length && amplitudes[currentAmplitudeIndex] < AmplitudeThreshold)
                {
                    currentTimingValue += amplitudeDuration;
                    currentAmplitudeIndex++;
                }
                timings.Add(currentTimingValue);

                //get duration over threshold
                currentTimingValue = 0;
                while (currentAmplitudeIndex < amplitudes.Length && amplitudes[currentAmplitudeIndex] >= AmplitudeThreshold)
                {
                    currentTimingValue += amplitudeDuration;
                    currentAmplitudeIndex++;
                }
                timings.Add(currentTimingValue);
            }

            return timings.ToArray();
        }
        /// <summary>
        /// Returns Android Api Level
        /// </summary>
        public static int GetApiLevel() => ApiLevel;

        /// <summary>
        /// Returns Default Amplitude of device, or 0.
        /// </summary>
        public static int GetDefaultAmplitude() => defaultAmplitude;

        /// <summary>
        /// Returns true if device has vibrator
        /// </summary>
        public static bool HasVibrator()
        {
            return vibrator != null && vibrator.Call<bool>("hasVibrator");
        }

        /// <summary>
        /// Return true if device supports amplitude control
        /// </summary>
        public static bool HasAmplitudeControl()
        {
            if (HasVibrator() && doesSupportVibrationEffect())
            {
                return vibrator.Call<bool>("hasAmplitudeControl"); // API 26+ specific
            }
            else
            {
                return false; // no amplitude control below API level 26
            }
        }

        /// <summary>
        /// Tries to cancel current vibration
        /// </summary>
        public static void Cancel()
        {
            if (HasVibrator())
            {
                vibrator.Call("cancel");
                logAuto("Cancel (): Called", logLevel.Info);
            }
        }
        #endregion

        #region Vibrate Internal
        #region Vibration Callers
        private static void vibrateEffect(long milliseconds, int amplitude)
        {
            using (UnityEngine.AndroidJavaObject effect = createEffect_OneShot(milliseconds, amplitude))
            {
                vibrator.Call("vibrate", effect);
            }
        }

        private static void vibrateLegacy(long milliseconds)
        {
            vibrator.Call("vibrate", milliseconds);
        }

        private static void vibrateEffect(long[] pattern, int repeat)
        {
            using (UnityEngine.AndroidJavaObject effect = createEffect_Waveform(pattern, repeat))
            {
                vibrator.Call("vibrate", effect);
            }
        }

        private static void vibrateLegacy(long[] pattern, int repeat)
        {
            vibrator.Call("vibrate", pattern, repeat);
        }

        private static void vibrateEffect(long[] pattern, int[] amplitudes, int repeat)
        {
            using (UnityEngine.AndroidJavaObject effect = createEffect_Waveform(pattern, amplitudes, repeat))
            {
                vibrator.Call("vibrate", effect);
            }
        }

        private static void vibrateEffectPredefined(int effectId)
        {
            using (UnityEngine.AndroidJavaObject effect = createEffect_Predefined(effectId))
            {
                vibrator.Call("vibrate", effect);
            }
        }
        #endregion

        #region Vibration Effect
        /// <summary>
        /// Wrapper for public static VibrationEffect createOneShot (long milliseconds, int amplitude). API >= 26
        /// </summary>
        private static UnityEngine.AndroidJavaObject createEffect_OneShot(long milliseconds, int amplitude)
        {
            return vibrationEffectClass.CallStatic<UnityEngine.AndroidJavaObject>("createOneShot", milliseconds, amplitude);
        }

        /// <summary>
        /// Wrapper for public static VibrationEffect createPredefined (int effectId). API >= 29
        /// </summary>
        private static UnityEngine.AndroidJavaObject createEffect_Predefined(int effectId)
        {
            return vibrationEffectClass.CallStatic<UnityEngine.AndroidJavaObject>("createPredefined", effectId);
        }
        
        /// <summary>
        /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int[] amplitudes, int repeat). API >= 26
        /// </summary>
        private static UnityEngine.AndroidJavaObject createEffect_Waveform(long[] timings, int[] amplitudes, int repeat)
        {
            return vibrationEffectClass.CallStatic<UnityEngine.AndroidJavaObject>("createWaveform", timings, amplitudes, repeat);
        }
        
        /// <summary>
        /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int repeat). API >= 26
        /// </summary>
        private static UnityEngine.AndroidJavaObject createEffect_Waveform(long[] timings, int repeat)
        {
            return vibrationEffectClass.CallStatic<UnityEngine.AndroidJavaObject>("createWaveform", timings, repeat);
        }
        #endregion
        #endregion

        #region Internal
        private static void logAuto(string text, logLevel level)
        {
            if (level == logLevel.Disabled)
            {
                level = logLevel.Info;
            }

            if (text != null)
            {
                if (level == logLevel.Warning && LogLevel == logLevel.Warning)
                {
                    UnityEngine.Debug.LogWarning(text);
                }
                else if (level == logLevel.Info && LogLevel >= logLevel.Info)
                {
                    UnityEngine.Debug.Log(text);
                }
            }
        }

        private static string arrToStr(long[] array) => array == null ? "null" : string.Join(", ", array);
        private static string arrToStr(int[] array) => array == null ? "null" : string.Join(", ", array);

        private static void clampAmplitudesArray(int[] amplitudes)
        {
            for (int i = 0; i < amplitudes.Length; i++)
            {
                amplitudes[i] = UnityEngine.Mathf.Clamp(amplitudes[i], 0, 255);
            }
        }
        #endregion

        public static class PredefinedEffect
        {
            public static int EFFECT_CLICK;         // public static final int EFFECT_CLICK
            public static int EFFECT_DOUBLE_CLICK;  // public static final int EFFECT_DOUBLE_CLICK
            public static int EFFECT_HEAVY_CLICK;   // public static final int EFFECT_HEAVY_CLICK
            public static int EFFECT_TICK;          // public static final int EFFECT_TICK
        }

        public enum logLevel
        {
            Disabled,
            Info,
            Warning,
        }

    }

}
