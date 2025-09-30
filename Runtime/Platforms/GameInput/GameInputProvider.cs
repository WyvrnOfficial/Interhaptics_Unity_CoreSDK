/* ​
* Copyright © 2025 Wyvrn. All rights reserved. ​
* ​
*/

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !ENABLE_METAQUEST && !ENABLE_OPENXR
using System.Runtime.InteropServices;


namespace Interhaptics.Platforms.GameInput
{

    public sealed class GameInputProvider : IHapticProvider
    {

        #region HAPTIC CHARACTERISTICS FIELDS
        private const string DISPLAY_NAME = "GameInput";
        private const string DESCRIPTION = "Controller APIs communication layer for GameInput";
        private const string MANUFACTURER = "Microsoft";
        private const string VERSION = "1.0";
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
        private static class GameInputProviderNative
        {
            const string DLL_NAME = "Interhaptics.GameInputProvider";

            [DllImport(DLL_NAME)]
            public static extern bool ProviderInit();
            [DllImport(DLL_NAME)]
            public static extern bool ProviderIsPresent();
            [DllImport(DLL_NAME)]
            public static extern bool ProviderClean();
            [DllImport(DLL_NAME)]
            public static extern void ProviderRenderHaptics();
            [DllImport(DLL_NAME)]
            public static extern bool IsGameInputSupported();
            [DllImport(DLL_NAME)]
            public static extern void SetTriggerMode(bool transcodeStiffness);
        }

        [UnityEngine.Scripting.Preserve]
        public bool Init()
        {
            bool res = GameInputProviderNative.ProviderInit();

            if ((res) && (HapticManager.DebugSwitch))
            {
                UnityEngine.Debug.Log("GameInput haptic provider started.");
            }
            return res;
        }

        [UnityEngine.Scripting.Preserve]
        public bool IsPresent()
        {
            return GameInputProviderNative.ProviderIsPresent();
        }

        [UnityEngine.Scripting.Preserve]
        public bool Clean()
        {
            return GameInputProviderNative.ProviderClean();
        }

        [UnityEngine.Scripting.Preserve]
        public void RenderHaptics()
        {
            GameInputProviderNative.ProviderRenderHaptics();
        }

        [UnityEngine.Scripting.Preserve]
        public bool IsGameInputSupported()
        {
            return GameInputProviderNative.IsGameInputSupported();
        }

        [UnityEngine.Scripting.Preserve]
        public void SetTriggerMode(bool transcodeStiffness)
        {
            GameInputProviderNative.SetTriggerMode(transcodeStiffness);
        }
        #endregion

    }

}
#endif 