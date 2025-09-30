#2025.08.20
# v1.8.1

+ Updated of the OpenXR and MetaQuest pipeline

---

#2025.08.20
# v1.8.0

+ Updated haptic engine to 1.13.2
+ Major rework of the iOS pipeline. This should not impact your project but please contact us if you face any issue

---

#2024.10.15
# v1.7.2

+ Added support for Meta Quest 3

---

#2024.10.01
# v1.7.1

+ Fixed an error that could be thrown when quitting the application
+ Updated Sensa provider to work with Synapse 4

---

#2024.09.12
# v1.7.0

+ Merged community pull request 01 (fix on iOS engine init)
+ Updated .haps library to V6 format
+ Fixed an issue that could lead to crash if 2 controllers of different manufacturers were plugged 
+ Updated haptic engine and device libraries  

---

#2024.01.16
# v1.6.0

+ Various fixes on Code based APIs
+ Various fixes on Source based APIs
+ Fixes for Mobile, XR, and Controller rendering
+ Fixes for MacOS compilation
+ Updated haptic engine
+ Added support for StreamingAssets
+ Known/observed issues: iOS haptics on some phones, clicky OpenXR on Quest 3 controller

---

#2024.01.16
# v1.5.0
+ Added support to Sensa HD Haptics platform
+ Fixed error logging in MacOS Editor play mode

---

#2023.12.22
# v1.4.0
+ Added various API to play simple effects
    + PlayHapticEffect()
    + PlayConstant()
    + PlayTransient()
    + Play()
    + PlayAdvanced()
    + PlayHapticPreset()

---

#2023.11.29
# v1.3.0
+ Updated XInput provider to GameInput provider (GameInputProvider.dll supplants XInputProvider.dll) with XInput used as fallback. Vibrations can be mapped on left|right, trigger|body or whole controller for GameInput devices (e.g., XBox Controller - One, Series X, XBox compatible controllers: Razer Wolverine)
+ Exposed new methods from the Interhaptics Engine for Intensity Contro (Global, Haptic Source and Target)
+ HapticManager changed from MonoBehaviour to Static Class through Unity's PlayerLoop => There is no need to add the HapticManager component to a GameObject in your scene for haptics to function 
+ Parametric haptic effects implemented - ParametricHapticSource.cs
+ Main methods documented in HAR.Native.cs with Doxygen formatting
+ Looiping implementation: maximum loops and maximum loop time functionality across the board for Haptic Sources
+ StopAllHaptics and ResumeAllHaptics implemented in Global Intensity
+ Cleaned up legacy mobile pipeline as a result of changes in iOS 17
+ Improved haptic rendering for iOS
+ Corrected namespace for EventHapticSource - Interhaptics.Internal -> Interhaptics.Utils
+ Haptic compability check script added
+ Several QoL improvements in Editor scripts 

---

# 2023.08.28
# v1.2.3

+ Updated Scenes for mobile (Android/iOS) and XInput providers
+ Fixed crashes on Android when Scripting Backend is Mono. Haptics will not work on Android unless switching Scripting Backend to IL2CPP.
+ Fixed haptics playing on Pause/Not in Focus/Exit PlayMode
+ Debug switches added for Haptic Manager, Haptic Sources and Legacy Mobile Haptic Effects
+ Updated dlls  

---

# 2023.06.14
# v1.2.2

+ Fix an issue that could prevent some Android phones to not render haptics

---

# 2023.06.05
# v1.2.1

+ OpenXR haptic support

---

# 2023.04.13
# v1.1.1

+ XInput sample scene. Installed input presets in Assets\InputManager for XInput or XR depending on the sample scene.
+ Updated dlls

---

# 2023.03.16
# v1.1.0

+ v1.0.7 and prior Mobile SDK moved to Legacy
+ New Mobile SDK aligned with the main SDK, using Haptic Sources
+ Mobile Sample scene
+ XInput support. Build PC games with haptics for controllers !
+ Dummy Haptic provider removed
+ Unity script bugfixing
+ Haptic engine update

---

# 2023.02.09
# v1.0.7

+ SpatialHapticSource clean and split with EventHapticSource
+ SampleScene_XR added; added assets for scene inside - materials; models, prefabs.
+ custom body parts for controllers instead of one body part
+ added 3d quest 2 controller model; solved haptic residual OnTriggerExit and multiple HapticSources playing at the same time on both controllers
+ added XRControllerHapticSource class as example which inherits from SpatialHapticSource
+ stop haptics when exiting trigger collision fix; check for Stop or Continue haptics when exiting collider
+ added AudioHapticsSource to work in sync with Unity AudioSource components

---

# 2023.01.26
# v1.0.6

+ Fixed wrong typecast
+ Backend update for iOS
+ Fix an issue that could throw errors in MacOS editor play mode

---

# 2023.01.24
# v1.0.5

+ Added VerboseMode and Play/Stop Parameters for Trigger and Collision (on by default); exposed HapticMaterialId
+ Update HapticSource and SpatialHapticSource with custom editors
+ HSExtension moved to SpatialHapticSource to play on Trigger and Collision (fixed to changed Trigger to false)
+ Correct location for Editor scripts
+ Extracted VibrationControl method to mitigate residual haptics
+ Fixed haptics backend linking issue

---

# 2023.01.13
# v1.0.4

+ Added missing HAR entry points
+ Remove unused code
+ Fix debug dependencies

---

# 2022.12.01
# v1.0.2

+ Add slider scripts for the mobile SDK

---

# 2022.11.29
# v1.0.1

+ Add audio samples to complete the Audio/Haptics synchronized sample assets
