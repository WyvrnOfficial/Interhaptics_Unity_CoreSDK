/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

#if ENABLE_METAQUEST && (UNITY_EDITOR || UNITY_ANDROID || UNITY_STANDALONE_WIN)
using UnityEngine;
using System.Runtime.InteropServices;
using Interhaptics.HapticBodyMapping;
using UnityEngine.XR; // For XRDisplaySubsystem, etc.
using UnityEngine.XR.Management;


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
		public string DisplayName() => DISPLAY_NAME;
		[UnityEngine.Scripting.Preserve]
		public string Description() => DESCRIPTION;
		[UnityEngine.Scripting.Preserve]
		public string Manufacturer() => MANUFACTURER;
		[UnityEngine.Scripting.Preserve]
		public string Version() => VERSION;
		#endregion

		#region PROVIDER LOOP
		private static class MetaQuestProviderNative
		{
			private const string DLL_NAME = "Interhaptics.MetaQuestProvider";

			[DllImport(DLL_NAME)]
			public static extern bool ProviderInit();

			[DllImport(DLL_NAME)]
			public static extern bool ProviderIsPresent();

			[DllImport(DLL_NAME)]
			public static extern bool ProviderClean();

			[DllImport(DLL_NAME)]
			public static extern void ProviderRenderHaptics();
		}

		/// <summary>
		/// Checks if XR is initialized and a VR headset is currently running.
		/// </summary>
		private bool IsXRActiveAndInitialized()
		{
			// 1) Grab the XR General Settings
			XRGeneralSettings xrSettings = XRGeneralSettings.Instance;
			if (xrSettings == null || xrSettings.Manager == null)
			{
				return false;
			}

			// 2) Check if XR plugin has completed initialization
			if (!xrSettings.Manager.isInitializationComplete)
			{
				return false;
			}

			// 3) Check if there's an active loader
			var activeLoader = xrSettings.Manager.activeLoader;
			if (activeLoader == null)
			{
				return false;
			}

			// 4) Check if the display subsystem is running
			XRDisplaySubsystem displaySubsystem = activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
			// If there's no display subsystem, or it's not "running", we are not actually in VR
			if (displaySubsystem == null || !displaySubsystem.running)
			{
				return false;
			}

			return true;
		}

		[UnityEngine.Scripting.Preserve]
		public bool Init()
		{
			// -- Check if XR is active/initialized --
			if (!IsXRActiveAndInitialized())
			{
				if (HapticManager.DebugSwitch)
				{
					Debug.LogWarning(
						"MetaQuest haptic provider will not initialize because XR is not active or no VR headset is running."
					);
				}
				return false;
			}

			// -- Now ensure the active loader is specifically Oculus --
			XRGeneralSettings xrSettings = XRGeneralSettings.Instance;
			if (!xrSettings.Manager.activeLoader.name.Contains("Oculus"))
			{
				if (HapticManager.DebugSwitch)
				{
					Debug.LogWarning("Active XR loader is not Oculus. Aborting MetaQuest provider initialization.");
				}
				return false;
			}

			// -- Initialize the native provider --
			bool res = MetaQuestProviderNative.ProviderInit();
			if (res && HapticManager.DebugSwitch)
			{
				Debug.Log("MetaQuest haptic provider started successfully.");
			}
			else if (!res && HapticManager.DebugSwitch)
			{
				Debug.LogWarning("MetaQuest haptic provider failed to start for unknown reasons.");
			}

			return res;
		}

		[UnityEngine.Scripting.Preserve]
		public bool IsPresent()
		{
			// If you'd rather rely on the Unity XR check, you could do: return IsXRActiveAndInitialized();
			// But here, we keep the native call if it's needed for other logic:
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
