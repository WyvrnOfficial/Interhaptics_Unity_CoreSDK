/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
using System.Runtime.InteropServices;

namespace Interhaptics.Platforms.Sensa
{

    public sealed class RazerSensaProvider : IHapticProvider
    {

        #region HAPTIC CHARACTERISTICS FIELDS
        private const string DISPLAY_NAME = "Razer Sensa Platform";
        private const string DESCRIPTION = "Razer Sensa Platform";
        private const string MANUFACTURER = "Razer Inc.";
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
        private static class RazerSensaProviderNative
        {
            const string DLL_NAME = "Interhaptics.RazerProvider";

            [DllImport(DLL_NAME)]
            public static extern bool ProviderInit();
            [DllImport(DLL_NAME)]
            public static extern bool ProviderIsPresent();
            [DllImport(DLL_NAME)]
            public static extern bool ProviderClean();
            [DllImport(DLL_NAME)]
            public static extern void ProviderRenderHaptics();
        }

        [UnityEngine.Scripting.Preserve]
        public bool Init()
        {
            bool res = RazerSensaProviderNative.ProviderInit();

            if ((res) && (HapticManager.DebugSwitch))
            {
                UnityEngine.Debug.Log("Razer Sensa haptic provider started.");
            }
            return res;
        }

        [UnityEngine.Scripting.Preserve]
        public bool IsPresent()
        {
            return RazerSensaProviderNative.ProviderIsPresent();
        }

        [UnityEngine.Scripting.Preserve]
        public bool Clean()
        {
            return RazerSensaProviderNative.ProviderClean();
        }

        [UnityEngine.Scripting.Preserve]
        public void RenderHaptics()
        {
            RazerSensaProviderNative.ProviderRenderHaptics();
        }
        #endregion

    }

}
#endif