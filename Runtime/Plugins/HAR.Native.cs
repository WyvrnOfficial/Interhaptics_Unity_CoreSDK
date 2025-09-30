/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using System.Runtime.InteropServices;
using Interhaptics.HapticBodyMapping;


namespace Interhaptics.Core
{
    public static partial class HAR
    {
#if UNITY_EDITOR_OSX
        // Implementation for UNITY_EDITOR_OSX
        // Methods will have dummy or macOS-specific implementations

        public static bool Init()
        {
            UnityEngine.Debug.LogWarning("Unity Editor in iOS will have dummy implementation of the Interhaptics Engine. To correctly debug in editor use a Windows version of the Editor.");
            // Dummy implementation for macOS Unity Editor
            return false;
        }

        public static void Quit()
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void SetGlobalIntensity(double _intensity)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static double GetGlobalIntensity()
        {
            // Dummy implementation for macOS Unity Editor
            return 1.0;
        }

        public static int AddParametricEffect([In] double[] _amplitude, int _amplitudeSize, [In] double[] _pitch, int _pitchSize, double _freqMin, double _freqMax, [In] double[] _transient, int _transientSize, bool _isLooping)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static void SetEventIntensity(int _hMaterialId, double _intensity)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void SetEventLoop(int _hMaterialId, int _loops)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void SetTargetIntensityMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size, double _intensity)
        {
            // Dummy implementation for macOS Unity Editor
        }

        private static int AddHM(string _content)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        private static bool UpdateHM(int _id, string _content)
        {
            // Dummy implementation for macOS Unity Editor
            return false;
        }

        public static double GetVibrationAmp(int _id, double _step, int _mode = 0)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static double GetVibrationFreq(int _id, double _step)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static double GetVibrationLength(int _id)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static double GetTextureAmp(int _id, double _step, int _mode = 0)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static double GetTextureFreq(int _id, double _step)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static double GetTextureLength(int _id)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static double GetStiffnessAmp(int _id, double _step)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static double GetStiffnessFreq(int _id, double _step)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static void ClearOutputBuffers(bool _resetClips)
		{
			// Dummy implementation for macOS Unity Editor
		}

        public static int GetOutputBufferSize(int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID, int _x, int _y, int _z, Interhaptics.HapticBodyMapping.BufferDataType _dataType)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static void GetOutputBuffer(double[] _outputBuffer, int _bufferSize, int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID, int _x, int _y, int _z, Interhaptics.HapticBodyMapping.BufferDataType _dataType)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static ulong GetVectorStartingTime(int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID, int _xDimension, int _yDimension, int _zDimension)
        {
            // Dummy implementation for macOS Unity Editor
            return 0;
        }

        public static int CreateBodyPart(Perception _perception, BodyPartID _bodyPartID, int _xDimension, int _yDimension, int _zDimension, double _sampleRate, bool _hd, bool _splitFrequency, bool _splitTransient, EProtocol _protocol)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static int CreateBodyParts(int _numberOfBP, Perception[] _perception, BodyPartID[] _bodyPartID, int[] _xDimension, int[] _yDimension, int[] _zDimension, double[] _sampleRate, bool[] _hd, bool[] _splitFrequency, bool[] _splitTransient, EProtocol[] _protocol)
        {
            // Dummy implementation for macOS Unity Editor
            return -1;
        }

        public static void DeleteBodyPart(int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void PlayEvent(int _hMaterialId, double _vibrationOffset, double _textureOffset, double _stiffnessOffset)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void StopEvent(int _hMaterialId)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void StopAllEvents()
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void ClearActiveEvents()
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void ClearInactiveEvents()
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void ClearEvent(int _hMaterialId)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void SetEventOffsets(int _hMaterialId, double _vibrationOffset, double _textureOffset, double _stiffnessOffset)
        {
            // Dummy implementation for macOS Unity Editor
        }

        private static void UpdateEventPositionsMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size, double _texturePosition, double _stiffnessPosition)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void ComputeAllEvents(double _curTime)
        {
            // Dummy implementation for macOS Unity Editor
        }

        private static void AddTargetToEventMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size)
        {
            // Dummy implementation for macOS Unity Editor
        }

        private static void RemoveTargetFromEventMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size)
        {
            // Dummy implementation for macOS Unity Editor
        }

        public static void RemoveAllTargetsFromEvent(int _hMaterialId)
        {
            // Dummy implementation for macOS Unity Editor
        }

        // ... other methods if needed

#else


#if (UNITY_IOS || UNITY_SWITCH) && !UNITY_EDITOR
        const string DLL_NAME = "__Internal";
#else
        const string DLL_NAME = "HAR";
#endif

		/// <summary>
		/// Initializes the different components and modules of the Interhaptics Engine.
		/// - Haptic Material Manager: module in charge of loading and storing haptic effects
		/// - Human Avatar Manager: module in charge of mapping between device, human avatar, and experience
		/// - Haptic Event Manager: module in charge of the control of haptic sources
		/// </summary>
		/// <returns>Always true even if a module is not properly initialized.</returns>
		[DllImport(DLL_NAME)]
        public static extern bool Init();
		/// <summary>
		/// Cleans the different components and modules of the Interhaptics Engine.
		/// To be called before the application is quit.
		/// </summary>
        [DllImport(DLL_NAME)]
		public static extern void Quit();
		/// <summary>
		/// Sets the global rendering intensity factor for the whole engine.
		/// </summary>
		/// <param name="_intensity">Positive value. 0 is nothing. Base value is 1.</param>
		[DllImport(DLL_NAME)]
		public static extern void SetGlobalIntensity(double _intensity);
		/// <summary>
		/// Gets the global rendering intensity factor for the whole engine.
		/// </summary>
		/// <returns>The global intensity. -1 if mixer is not initialized.</returns>
		[DllImport(DLL_NAME)]
		public static extern double GetGlobalIntensity();
		/// <summary>
		/// Creates a parametric effect using amplitude, pitch, and transient parameters.
		/// </summary>
		/// <param name="_amplitude">Array of amplitude values formatted as Time - Value pairs, with values between 0 and 1.</param>
		/// <param name="_amplitudeSize">Size of the amplitude array.</param>
		/// <param name="_pitch">Array of pitch values formatted as Time - Value pairs, with values between 0 and 1.</param>
		/// <param name="_pitchSize">Size of the pitch array.</param>
		/// <param name="_freqMin">Minimum value for the frequency range.</param>
		/// <param name="_freqMax">Maximum value for the frequency range.</param>
		/// <param name="_transient">Array of transient values formatted as Time - Amplitude - Frequency triples, with values between 0 and 1.</param>
		/// <param name="_transientSize">Size of the transient array.</param>
		/// <param name="_isLooping">Indicates whether the effect should loop.</param>
		/// <returns>ID of the created haptic source, or -1 if creation failed.</returns>
		[DllImport(DLL_NAME)]
		public static extern int AddParametricEffect([In] double[] _amplitude, int _amplitudeSize, [In] double[] _pitch, int _pitchSize, double _freqMin, double _freqMax, [In] double[] _transient, int _transientSize, bool _isLooping);
		/// <summary>
		/// Sets the haptic intensity factor for a specific source.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source. Same as the attached haptic effect.</param>
		/// <param name="_intensity">Intensity factor value. Always clamped above 0.</param>
		[DllImport(DLL_NAME)]
		public static extern void SetEventIntensity(int _hMaterialId, double _intensity);
		/// <summary>
		/// Sets the loop flag for a specific source
		/// </summary>
		/// <param name="_hMaterialID">ID of the source. Same as the attached haptic effect.</param>
		/// <param name="_numberOfLoops">Number of loops for the event. 0 means the source is not looping, 1 or more is the number of loops set</param>
		/// <returns></returns>
		[DllImport(DLL_NAME)] 
        public static extern void SetEventLoop(int _hMaterialID, int _numberOfLoops);
		/// <summary>
		/// Updates the haptic intensity for a specific target of a source.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source. Same as the attached haptic effect.</param>
		/// <param name="_target">Array of CommandData representing the target. A target contains a group of body parts, lateral flags, and exclusion flags.</param>
		/// <param name="_size">Size of the _target array.</param>
		/// <param name="_intensity">New intensity factor value. Always clamped above 0.</param>
		[DllImport(DLL_NAME)]
		public static extern void SetTargetIntensityMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size, double _intensity);
		/// <summary>
		/// Adds the content of an .haps file to the Interhaptics Engine for future use.
		/// </summary>
		/// <param name="_content">JSON content of the .haps file to be loaded. Needs to follow Interhaptics .haps format.</param>
		/// <returns>ID of the haptic effect to be used in other engine calls. -1 if loading failed.</returns>
		[DllImport(DLL_NAME)]
        private static extern int AddHM(string _content);
		/// <summary>
		/// Replaces the content of an already loaded haptic effect.
		/// Useful when the ID of the haptic effect needs to be persistent.
		/// </summary>
		/// <param name="_hMaterialID">ID of the haptic effect to be replaced.</param>
		/// <param name="_content">JSON content of the .haps file to be loaded. Needs to follow Interhaptics .haps format.</param>
		/// <returns>true if the effect was properly updated, false otherwise.</returns>
		[DllImport(DLL_NAME)]
        private static extern bool UpdateHM(int _id, string _content);

		[DllImport(DLL_NAME)]
        public static extern double GetVibrationAmp(int _id, double _step, int _mode = 0);

		[DllImport(DLL_NAME)]
        public static extern double GetVibrationFreq(int _id, double _step);
		/// <summary>
		/// Retrieves the length of the vibration for a given haptic effect.
		/// </summary>
		/// <param name="_id">The identifier for the haptic effect.</param>
		/// <returns>The length of the vibration.</returns>
		[DllImport(DLL_NAME)]
        public static extern double GetVibrationLength(int _id);
		[DllImport(DLL_NAME)]
        public static extern double GetTextureAmp(int _id, double _step, int _mode = 0);
		[DllImport(DLL_NAME)]
        public static extern double GetTextureFreq(int _id, double _step);
		[DllImport(DLL_NAME)]
        public static extern double GetTextureLength(int _id);
		[DllImport(DLL_NAME)]
        public static extern double GetStiffnessAmp(int _id, double _step);
		[DllImport(DLL_NAME)]
        public static extern double GetStiffnessFreq(int _id, double _step);
        /// <summary>
        /// ClearOutputBuffers
        /// </summary>
        /// <param name="_resetClips">Set to true to also clear the clips</param>
        [DllImport(DLL_NAME)]
        public static extern void ClearOutputBuffers(bool _resetClips = false);
		[DllImport(DLL_NAME)]
        public static extern int GetOutputBufferSize(int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID, int _x, int _y, int _z, Interhaptics.HapticBodyMapping.BufferDataType _dataType);
		[DllImport(DLL_NAME)]
        public static extern void GetOutputBuffer(double[] _outputBuffer, int _bufferSize, int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID, int _x, int _y, int _z, Interhaptics.HapticBodyMapping.BufferDataType _dataType);
		[DllImport(DLL_NAME)]
        public static extern ulong GetVectorStartingTime(int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID, int _xDimension, int _yDimension, int _zDimension);
		[DllImport(DLL_NAME)]
        public static extern int CreateBodyPart(Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID, int _xDimension, int _yDimension, int _zDimension, double _sampleRate, bool _hd, bool _splitFrequency, bool _splitTransient, Interhaptics.HapticBodyMapping.EProtocol _protocol);
        [DllImport(DLL_NAME)]
        public static extern int CreateBodyParts(int _numberOfBP, Interhaptics.HapticBodyMapping.Perception[] _perception, Interhaptics.HapticBodyMapping.BodyPartID[] _bodyPartID, int[] _xDimension, int[] _yDimension, int[] _zDimension, double[] _sampleRate, bool[] _hd, bool[] _splitFrequency, bool[] _splitTransient, Interhaptics.HapticBodyMapping.EProtocol[] _protocol);
        [DllImport(DLL_NAME)]
        public static extern void DeleteBodyPart(int _avatarID, Interhaptics.HapticBodyMapping.Perception _perception, Interhaptics.HapticBodyMapping.BodyPartID _bodyPartID);

		/// <summary>
		/// Starts the rendering playback of a haptic source.
		/// Sets the starting time to 0 + different offsets.
		/// If the event is already playing, it restarts with the new offsets.
		/// If the source does not already exist, it will be created.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source to play. Same as the attached haptic effect.</param>
		/// <param name="_vibrationOffset">Vibration offset.</param>
		/// <param name="_textureOffset">Texture offset.</param>
		/// <param name="_stiffnessOffset">Stiffness offset.</param>
		[DllImport(DLL_NAME)]
        public static extern void PlayEvent(int _hMaterialId, double _vibrationOffset, double _textureOffset, double _stiffnessOffset);
		/// <summary>
		/// Stops the rendering playback of a haptic source.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source to stop. Same as the attached haptic effect.</param>
		[DllImport(DLL_NAME)]
        public static extern void StopEvent(int _hMaterialId);
		/// <summary>
		/// Stops the rendering playback of all haptic sources. Active Events become inactive. Inactive events are cleared from memory.
		/// </summary>
		[DllImport(DLL_NAME)]
		public static extern void StopAllEvents();
		[DllImport(DLL_NAME)]
		/// <summary>
		/// Removes all active sources from the memory. Deleted sources can be recreated by calling the PlayEvent function.
		/// </summary>
		public static extern void ClearActiveEvents();
        [DllImport(DLL_NAME)]
		/// <summary>
		/// Removes all inactive sources from the memory. Inactive sources are kept in memory to avoid deletion
		/// and creation when playing and stopping a source.
		/// </summary>
		public static extern void ClearInactiveEvents();
		/// <summary>
		/// Clears a specific haptic source whether it is active or not.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source. Same as the attached haptic effect.</param>
		[DllImport(DLL_NAME)]
        public static extern void ClearEvent(int _hMaterialId);
		/// <summary>
		/// Sets the offsets for a specific haptic source.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source. Same as the attached haptic effect.</param>
		/// <param name="_vibrationOffset">Vibration offset.</param>
		/// <param name="_textureOffset">Texture offset.</param>
		/// <param name="_stiffnessOffset">Stiffness offset.</param>
		[DllImport(DLL_NAME)]
        public static extern void SetEventOffsets(int _hMaterialId, double _vibrationOffset, double _textureOffset, double _stiffnessOffset);
		/// <summary>
		/// Updates the spatial positions for a specific source target.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source. Same as the attached haptic effect.</param>
		/// <param name="_target">Array of CommandData to build a target. A target contains a group of body parts, lateral flags, and exclusion flags.</param>
		/// <param name="_size">Size of the _target array.</param>
		/// <param name="_texturePosition">New texture position.</param>
		/// <param name="_stiffnessPosition">New stiffness position.</param>
		[DllImport(DLL_NAME)]
        private static extern void UpdateEventPositionsMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size, double _texturePosition, double _stiffnessPosition);
		/// <summary>
		/// To be called in the application main loop to trigger the rendering of all haptic buffers
		/// at a specific time. The Interhaptics Engine will compare the current time with the last
		/// known value to build a buffer large enough to cover frame drops.
		/// Can be called from the main thread or in a parallel loop.
		/// Must be called at least once before triggering the device update event.
		/// </summary>
		/// <param name="_curTime">Current time in seconds.</param>
		[DllImport(DLL_NAME)]
        public static extern void ComputeAllEvents(double _curTime);
		/// <summary>
		/// Adds a target in range of the source. The Interhaptics Engine will remap device endpoints and in-range target
		/// to the device management layer for haptic playback.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source to add a target to. Same as the attached haptic effect.</param>
		/// <param name="_target">Pointer of CommandData to build a target. A target contains a group of bodyparts, lateral flags, and exclusion flags. Memory must be allocated before calling this entrypoint.</param>
		/// <param name="_size">Size of the _target array.</param>
		[DllImport(DLL_NAME)]
        private static extern void AddTargetToEventMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size);
		/// <summary>
		/// Removes a target from a source range. The Interhaptics Engine will remap device endpoints
		/// and in-range target to the device management layer for haptic playback.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source to remove a target from. Same as the attached haptic effect.</param>
		/// <param name="_target">Array of CommandData to build a target. A target contains a group of body parts, lateral flags, and exclusion flags.</param>
		/// <param name="_size">Size of the _target array.</param>
		[DllImport(DLL_NAME)]
        private static extern void RemoveTargetFromEventMarshal(int _hMaterialId, Interhaptics.HapticBodyMapping.CommandData[] _target, int _size);
		/// <summary>
		/// Removes all targets from a source range.
		/// </summary>
		/// <param name="_hMaterialId">ID of the source to remove targets from. Same as the attached haptic effect.</param>
		[DllImport(DLL_NAME)]
        public static extern void RemoveAllTargetsFromEvent(int _hMaterialId);

#endif

    }

}