Requires new Input System fron Unity.
Requires the XR Plugin Management setup to work with OpenXR module.

Troubleshooting: 
- If no haptics is playing, check if the selected runtime support haptics through OpenXR.
- To access haptics through the Open XR provider, the Meta Quest provider has to be removed from the from the Interhaptics/Runtime/Platforms.
- Open XR haptics do not have HD haptics for Meta Quest 3/Pro. To be able to use HD haptics please use the Meta Quest provider.
- The Haptic provider for Meta Quest or OpenXR is switched automatically depending on the platform used for the build. However, if there is a conflict with Meta Quest/ GameInput/Razer Sensa provider, remove the Meta Quest/GameInput/Razer Sensa folder from the Interhaptics/Runtime/Platforms directory.