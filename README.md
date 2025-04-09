To import the Interhaptics SDK into Unity using the Unity Package Manager, follow these steps:

1. Open the Unity Package Manager, then click the “+” button in the toolbar.
2. Select “Add package from git URL” from the menu.
3. In the text box that appears, enter the URL “https://github.com/Interhaptics/Unity_CoreSDK.git” (without the quotes) and click “Add.” 
4. If the installation is successful, the Interhaptics package will appear in the package list with the git tag.

* There are sample scenes for each platforms located in Assets\Scenes.
Requirements (XInput/XR scenes): 
1. Set Player Settings>>Active Input Handling: Input Manager (Old) or Both
2. Load Assets\InputManager\InputManager-GameInput_XInput.preset, or InputManager-XR.preset before running the scene in your Input Manager screen.
Requirements (mobile scene):
1. Set Scripting Backend to IL2CPP and check the target architecture for ARM64.

For specific setups (Mobile iOS or Android / XR / Console PlayStation or XBox) you can check directly the documentation at https://doc.wyvrn.com/docs/interhaptics-sdk/haptics-unity-sdk/

If you need help or want to talk haptics, join us on Discord on this links https://discord.com/invite/93jU9nAX4f
