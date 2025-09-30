/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
#if UNITY_2019_3_OR_NEWER
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
#else
using UnityEngine.Experimental.LowLevel;
using UnityEngine.Experimental.PlayerLoop;
#endif


namespace Interhaptics
{
	/// <summary>
	/// Unity PlayerLoop use structure types to identify the different life cycles configured
	/// The following structure are custom made to identify the Interhaptics cycles
	/// </summary>

	internal struct InterhapticsPlayerLoop
    {
        // Replace the rendering loop cycle used previously
        internal struct UpdateHaptic { }
    }

    public static class HapticManager
    {

        private const float FIXED_TIMEFRAME = 0.03f;
        /// <summary>
        /// Debug switch to enable or disable the debug mode
        /// </summary>
        public static bool DebugSwitch { get; set; } = false;
        /// <summary>
        /// Switch to enable or disable the haptic rendering on Android if the platform is not supported
        /// </summary>
        public static bool MonoScriptingBackend { get; internal set; } = false;
		/// <summary>
		/// New variable to control stopping haptics on focus loss or pause, default set to true.
		/// </summary>
		public static bool StopHapticsOnFocusLoss { get; set; } = true;
		private static float lastCall = 0;

        [RuntimeInitializeOnLoadMethod]
        private static void AppStart()
        {
            GeneratePlayerLoopNodes();
            Application.quitting += OnApplicationQuit;
            Application.focusChanged += OnFocusChanged;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged += OnPauseStateChanged;
#endif

#if UNITY_EDITOR_OSX
            UnityEngine.Debug.LogWarning("Unity Editor in macOS will have dummy implementation of the Interhaptics Engine. To correctly debug in Unity use a Windows version of the Unity Editor.");
#endif

#if !UNITY_EDITOR_OSX
            Core.HAR.Init();
            Internal.HapticDeviceManager.DeviceInitLoop();
#if UNITY_ANDROID && !ENABLE_METAQUEST && !ENABLE_OPENXR && !UNITY_EDITOR
            Platforms.Mobile.GenericAndroidHapticAbstraction.Initialize();
#endif
#endif
        }

        private static void GeneratePlayerLoopNodes()
        {
#if UNITY_2019_3_OR_NEWER
            PlayerLoopSystem system = PlayerLoop.GetCurrentPlayerLoop();
#else
            PlayerLoopSystem system = PlayerLoop.GetDefaultPlayerLoop();
#endif
            PlayerLoopSystem updateHaptic = new PlayerLoopSystem()
            {
                updateDelegate = UpdateHaptic,
                type = typeof(InterhapticsPlayerLoop.UpdateHaptic)
            };

            if (!InsertPlayerLoopNodeAfter<PostLateUpdate.UpdateAudio>(ref system, updateHaptic) &&
                !InsertPlayerLoopNodeAfter<PostLateUpdate>(ref system, updateHaptic))
            {
                Debug.LogError("[Interhaptics] Impossible to initialize the haptic rendering loop");
                return;
            }

            PlayerLoop.SetPlayerLoop(system);
        }

        private static bool InsertPlayerLoopNodeAfter<T>(ref PlayerLoopSystem system, PlayerLoopSystem elementToInsert)
        {
            if (system.subSystemList == null)
            {
                return false;
            }

            // We search the life cycle identified by the type T
            for (var i = 0; i < system.subSystemList.Length; i++)
            {
                if (system.subSystemList[i].type == typeof(T))
                {
                    i++; // i was the index of the tick identify by T. By incrementing it, it become the index of the life cycle we want to insert
                    System.Array.Resize(ref system.subSystemList, system.subSystemList.Length + 1); // increase the subsystem length of 1
                    System.Array.Copy(system.subSystemList, i, system.subSystemList, i + 1, system.subSystemList.Length - i - 1); // shift the elements after i
                    system.subSystemList[i] = elementToInsert; // assign the index i to the life cycle we want to introduce
                    return true;
                }

                // if the subsystem isn't of type T, we search inside its children
                if (InsertPlayerLoopNodeAfter<T>(ref system.subSystemList[i], elementToInsert))
                {
                    return true;
                }
            }

            // arrive here only if no subsystem was of type T
            return false;
        }

        private static void UpdateHaptic()
        {
            if (!Application.isPlaying)
            {
                return;
            }
#if !ENABLE_METAQUEST && !ENABLE_OPENXR
            if (Time.realtimeSinceStartup - lastCall < FIXED_TIMEFRAME)
            {
                //Debug.Log("Skipping fixed time frame computation");
                return;
            }
#endif
            //Compute all haptics event
#if !UNITY_IOS && (!UNITY_ANDROID || ENABLE_METAQUEST || ENABLE_OPENXR || UNITY_EDITOR)
            Core.HAR.ComputeAllEvents(Time.realtimeSinceStartup);
#endif
            //Insert device rendering loop here
            Internal.HapticDeviceManager.DeviceRenderLoop();
            lastCall = Time.realtimeSinceStartup;
        }

        private static void OnFocusChanged(bool hasFocus)
        {
			// Check if haptics should be stopped when out of focus TODO: pause the haptic playback
			if ((!hasFocus) && (StopHapticsOnFocusLoss))
            {
                Core.HAR.ClearActiveEvents();
            }
        }

#if UNITY_EDITOR
        private static void OnPauseStateChanged(UnityEditor.PauseState state)
        {
            // TODO pause the haptic playback
            if ((UnityEditor.PauseState.Paused == state) && (StopHapticsOnFocusLoss))
            {
                Core.HAR.ClearActiveEvents();
            }
        }
#endif

		private static void OnApplicationQuit()
        {
#if !UNITY_EDITOR_OSX
            Internal.HapticDeviceManager.DeviceCleanLoop();
            Core.HAR.ClearActiveEvents();
            Core.HAR.ClearInactiveEvents();
            Core.HAR.Quit();
#endif

#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged -= OnPauseStateChanged;
#endif
            Application.focusChanged -= OnFocusChanged;
            Application.quitting -= OnApplicationQuit;
        }

    }

}