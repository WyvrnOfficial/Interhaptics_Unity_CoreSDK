/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/
#if ENABLE_OPENXR
using UnityEngine;
using Interhaptics.HapticBodyMapping;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.XR.Management;



[assembly: UnityEngine.Scripting.AlwaysLinkAssembly]
[assembly: UnityEngine.Scripting.Preserve]
namespace Interhaptics.Platforms.XR
{

    public sealed class OpenXRProvider : IHapticProvider
    {
        IH_HapticsInput m_IH_HapticsInput;
        float minimumDuration = 0.016f;
        private const int NUMBER_OF_BP = 2;
        private static readonly Perception[] PERCEPTIONS = { Perception.Vibration, Perception.Vibration };
        private static readonly BodyPartID[] BODYPARTS = { BodyPartID.Bp_Left_palm, BodyPartID.Bp_Right_palm };
        private static readonly int[] X_DIM = { 1, 1 };
        private static readonly int[] Y_DIM = { 1, 1 };
        private static readonly int[] Z_DIM = { 1, 1 };
        private static readonly double[] SAMPLERATE = { 500, 500 };
        private static readonly bool[] HD = { false, false };
        private static readonly bool[] SPLIT_FREQUENCY = { false, false };
        private static readonly bool[] SPLIT_TRANSIENTS = { false, false };
        private static readonly EProtocol[] PROTOCOL = { EProtocol.PCM, EProtocol.PCM };

        private int myAvatarID = -1;

        #region HAPTIC CHARACTERISTICS FIELDS
        private const string DISPLAY_NAME = "OpenXR device";
        private const string DESCRIPTION = "XR controller for OpenXR device";
        private const string MANUFACTURER = "Unknown";
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

		/// <summary>
		/// Checks if XR (via XR Plugin Management) is initialized and the display subsystem is running.
		/// </summary>
		private bool IsXRActiveAndInitialized()
		{
			XRGeneralSettings xrSettings = XRGeneralSettings.Instance;
			if (xrSettings == null || xrSettings.Manager == null)
			{
                Debug.Log("Failed 1");
				return false;
			}

			// Check if XR plugin initialization is complete
			if (!xrSettings.Manager.isInitializationComplete)
			{
                Debug.Log("Failed 2");
				return false;
			}

			// Check if we have an active loader
			XRLoader activeLoader = xrSettings.Manager.activeLoader;
			if (activeLoader == null)
			{
                Debug.Log("Failed 3");
				return false;
			}

			// Check if a display subsystem is running
			XRDisplaySubsystem displaySubsystem = activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
			if (displaySubsystem == null)
			{
                Debug.Log("Failed 4");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if the current active XR loader is specifically OpenXR.
		/// </summary>
		private bool IsOpenXRLoaderActive()
		{
			XRGeneralSettings xrSettings = XRGeneralSettings.Instance;
			if (xrSettings == null || xrSettings.Manager == null)
			{
				return false;
			}

			XRLoader activeLoader = xrSettings.Manager.activeLoader;
			if (activeLoader == null)
			{
				return false;
			}

			// Typical OpenXR loader name is something like "OpenXR Loader",
			// but you can adjust as needed
            Debug.Log("activeLoader.name: " + activeLoader.name);
			return activeLoader.name.Contains("Open XR");
		}

		[UnityEngine.Scripting.Preserve]
		public bool Init()
		{
			// 1) Check if XR is active
			if (!IsXRActiveAndInitialized())
			{
				if (HapticManager.DebugSwitch)
				{
					Debug.LogWarning("OpenXRProvider: XR is not active or the XR display subsystem is not running.");
				}
				return false;
			}

			// 2) Check if the active loader is OpenXR
			if (!IsOpenXRLoaderActive())
			{
				if (HapticManager.DebugSwitch)
				{
					Debug.LogWarning("OpenXRProvider: The active XR loader is not OpenXR. Initialization aborted.");
				}
				return false;
			}

            if (UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.Head) == null)
            {
                UnityEngine.Debug.Log("XR HMD not found");
                return false;
            }
            m_IH_HapticsInput = new IH_HapticsInput();
            
            myAvatarID = Core.HAR.CreateBodyParts(2, PERCEPTIONS, BODYPARTS, X_DIM, Y_DIM, Z_DIM, SAMPLERATE, HD, SPLIT_FREQUENCY, SPLIT_TRANSIENTS, PROTOCOL);
            if (HapticManager.DebugSwitch) 
            {
            UnityEngine.Debug.Log("OpenXR haptic provider started.");
            }
            return true;
        }



        [UnityEngine.Scripting.Preserve]
        public bool IsPresent()
        {
            UnityEngine.XR.HapticCapabilities caps = new UnityEngine.XR.HapticCapabilities();
            bool isPresent = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.LeftHand).TryGetHapticCapabilities(out caps);
            isPresent |= UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand).TryGetHapticCapabilities(out caps);
            return isPresent;
        }

        [UnityEngine.Scripting.Preserve]
        public bool Clean()
        {
            return true;
        }

        [UnityEngine.Scripting.Preserve]
        public void RenderHaptics()
        {
            double[] outputBuffer;
            int size = Core.HAR.GetOutputBufferSize(myAvatarID, Perception.Vibration, BodyPartID.Bp_Left_palm, 0, 0, 0, BufferDataType.PCM);
            if (size > 0)
            {
                outputBuffer = new double[size];
                Core.HAR.GetOutputBuffer(outputBuffer, size, myAvatarID, Perception.Vibration, BodyPartID.Bp_Left_palm, 0, 0, 0, BufferDataType.PCM);
                OpenXRInput.SendHapticImpulse(m_IH_HapticsInput.HapticsXR.Left, (float)AverageFromBuffer(outputBuffer, size), minimumDuration, UnityEngine.InputSystem.XR.XRController.leftHand);
            }
            else
            {
                OpenXRInput.StopHaptics(m_IH_HapticsInput.HapticsXR.Left, UnityEngine.InputSystem.XR.XRController.leftHand);
            }

            size = Core.HAR.GetOutputBufferSize(myAvatarID, Perception.Vibration, BodyPartID.Bp_Right_palm, 0, 0, 0, BufferDataType.PCM);
            if (size > 0)
            {
                outputBuffer = new double[size];
                Core.HAR.GetOutputBuffer(outputBuffer, size, myAvatarID, Perception.Vibration, BodyPartID.Bp_Right_palm, 0, 0, 0, BufferDataType.PCM);
                OpenXRInput.SendHapticImpulse(m_IH_HapticsInput.HapticsXR.Right, (float)AverageFromBuffer(outputBuffer, size), minimumDuration, UnityEngine.InputSystem.XR.XRController.rightHand);
            }
            else
            {
                OpenXRInput.StopHaptics(m_IH_HapticsInput.HapticsXR.Right, UnityEngine.InputSystem.XR.XRController.rightHand);
            }
        }
        #endregion

        double AverageFromBuffer(double[] buffer, int bufferSize)
        {
            if (bufferSize <= 0)
            {
                return 0;
            }

            double average = 0;

            for (int i = 0; i < bufferSize; i++)
            {
                average += buffer[i];
            }

            return average / bufferSize;
        }

    }

}
#endif