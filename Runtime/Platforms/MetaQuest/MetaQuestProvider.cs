/* ​
* Copyright (c) 2023 Go Touch VR SAS. All rights reserved. ​
* ​
*/

#if ENABLE_METAQUEST && (UNITY_EDITOR || UNITY_ANDROID || UNITY_STANDALONE_WIN)
using UnityEngine;
using System.Runtime.InteropServices;
using Interhaptics.HapticBodyMapping;


[assembly: UnityEngine.Scripting.AlwaysLinkAssembly]
[assembly: UnityEngine.Scripting.Preserve]
namespace Interhaptics.Platforms.XR
{

    public sealed class MetaQuestProvider : IHapticProvider
    {

#region HAPTIC CHARACTERISTICS FIELDS
        private const string DISPLAY_NAME = "Meta Quest";
        private const string DESCRIPTION = "XR controller for Meta Quest";
        private const string MANUFACTURER = "Meta";
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
        private static class MetaQuestProviderNative
        {
            const string DLL_NAME = "Interhaptics.MetaQuestProvider";

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

			// Check if a VR headset is present
			if (!MetaQuestProviderNative.ProviderIsPresent())
			{
				if (HapticManager.DebugSwitch)
				{
					UnityEngine.Debug.LogWarning("No VR headset detected. MetaQuest haptic provider will not initialize.");
				}
				return false; // Return false to indicate initialization failed due to lack of headset
			}

			bool res = MetaQuestProviderNative.ProviderInit();

            if ((res) && (HapticManager.DebugSwitch))
            {
                UnityEngine.Debug.Log("MetaQuest haptic provider started.");
            }
            return res;
        }

        [UnityEngine.Scripting.Preserve]
        public bool IsPresent()
        {
            return MetaQuestProviderNative.ProviderIsPresent();
        }

        [UnityEngine.Scripting.Preserve]
        public bool Clean()
        {
            return MetaQuestProviderNative.ProviderClean();
        }

        [UnityEngine.Scripting.Preserve]
        public void RenderHaptics()
        {
            MetaQuestProviderNative.ProviderRenderHaptics();
        }
#endregion

    }

}
#endif