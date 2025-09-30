
/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine;
using Interhaptics.HapticBodyMapping;
using System.Runtime.InteropServices;

namespace Interhaptics.Platforms.IOS
{

    public sealed class IOSProvider : IHapticProvider
    {
#region HAPTIC CHARACTERISTICS FIELDS
        private const string DISPLAY_NAME = "iOS";
        private const string DESCRIPTION = "iOS device";
        private const string MANUFACTURER = "Apple";
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

        private static class RazerIOSProviderNative
        {
        
            const string DLL_NAME = "__Internal";

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
            bool result = RazerIOSProviderNative.ProviderInit();
            return result;
        }

        [UnityEngine.Scripting.Preserve]
        public bool IsPresent()
        {
            bool result = RazerIOSProviderNative.ProviderIsPresent();
            return result;
        }

        [UnityEngine.Scripting.Preserve]
        public bool Clean()
        {
            bool result = RazerIOSProviderNative.ProviderClean();
            return result;
        }

        [UnityEngine.Scripting.Preserve]
        public void RenderHaptics()
        {
            RazerIOSProviderNative.ProviderRenderHaptics();
        }
#endregion
    }
}
#endif
