/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

namespace Interhaptics.HapticBodyMapping
{
    /// <summary>
    /// Definition of the different protocols supported by the Interhaptics Engine
    /// </summary>
    public enum EProtocol
    {
        UnknownProtocol = -1,
        PCM = 0,
        Clips = 1,
        RzInterhapticsProtocol = 2
    }

    /// <summary>
    /// Enumeration for different types of haptic perceptions.
    /// </summary>
    public enum Perception
    {
        None = -1,                      ///< No perception.
		Stiffness = 0,                  ///< Stiffness perception.
		Texture = Stiffness + 1,        ///< Texture perception.
		Vibration = Texture + 1,        ///< Vibration perception.
		AllPerception = Vibration + 1   ///< All types of perceptions.
	};

	/// <summary>
	/// Enumeration for types of haptic buffer data.
	/// </summary>
	public enum BufferDataType
    {
        Amplitude = 0,      ///< Data type for amplitude.
		PCM = 0,            ///< PCM data type, same as amplitude.
		Frequency = 1,      ///< Data type for frequency.
		Transient = 2       ///< Data type for transient.
	};

	/// <summary>
	/// Enumeration for identifying different body parts for haptic effects.
	/// </summary>
	public enum BodyPartID
    {
		// List of all body parts with specific identifiers - 56 bodyparts
		Bp_None = -1,

        Bp_Chest = 340,
        Bp_Waist = 325,
        Bp_Skull = 359,
        Bp_Neck = 358,

        Bp_Left_upper_arm = 212,
        Bp_Left_lower_arm = 214,

        Bp_Right_upper_arm = 112,
        Bp_Right_lower_arm = 114,

        Bp_Left_upper_leg = 226,
        Bp_Left_lower_leg = 228,

        Bp_Right_upper_leg = 126,
        Bp_Right_lower_leg = 128,

        Bp_Left_palm = 216,
        Bp_Right_palm = 116,

        Bp_Left_sole = 200,
        Bp_Right_sole = 100,

        Bp_Left_hallux = 233,
        Bp_Left_index_toe = 234,
        Bp_Left_middle_toe = 235,
        Bp_Left_ring_toe = 236,
        Bp_Left_pinky_toe = 237,

        Bp_Right_hallux = 133,
        Bp_Right_index_toe = 134,
        Bp_Right_middle_toe = 135,
        Bp_Right_ring_toe = 136,
        Bp_Right_pinky_toe = 137,

        Bp_Left_thumb_first = 244,
        Bp_Left_thumb_second = 245,
        Bp_Left_thumb_third = 246,

        Bp_Left_index_first = 247,
        Bp_Left_index_second = 248,
        Bp_Left_index_third = 249,

        Bp_Left_middle_first = 250,
        Bp_Left_middle_second = 251,
        Bp_Left_middle_third = 252,

        Bp_Left_ring_first = 253,
        Bp_Left_ring_second = 254,
        Bp_Left_ring_third = 255,

        Bp_Left_pinky_first = 256,
        Bp_Left_pinky_second = 257,
        Bp_Left_pinky_third = 258,

        Bp_Right_thumb_first = 144,
        Bp_Right_thumb_second = 145,
        Bp_Right_thumb_third = 146,

        Bp_Right_index_first = 147,
        Bp_Right_index_second = 148,
        Bp_Right_index_third = 149,

        Bp_Right_middle_first = 150,
        Bp_Right_middle_second = 151,
        Bp_Right_middle_third = 152,

        Bp_Right_ring_first = 153,
        Bp_Right_ring_second = 154,
        Bp_Right_ring_third = 155,

        Bp_Right_pinky_first = 156,
        Bp_Right_pinky_second = 157,
        Bp_Right_pinky_third = 158, //56 bodyparts
    };

	/// <summary>
	/// Enumeration for operator signs in haptic command data.
	/// </summary>
	public enum Operator
    {
        Minus = -1,
        Neutral = 0,
        Plus = 1,
    };

	/// <summary>
	/// Enumeration for lateral flag in haptic command data.
	/// </summary>
	public enum LateralFlag
    {
        Unknown_position = -1,
        Global = 0,
        Right = 1,
        Left = 2,
        Center = 3
    };

	/// <summary>
	/// Enumeration for group identification in haptic command data.
	/// </summary>
	public enum GroupID
    {
        Unknown = -1,

        All = 0,

        Top = 100,
        Down = 101,

        Arm = 200,
        Head = 201,
        Chest = 202,
        Waist = 203,
        Leg = 204,

        Upper_arm = 300,
        Lower_arm = 301,
        Hand = 302,
        Skull = 303,
        Neck = 304,
        Upper_leg = 305,
        Lower_leg = 306,
        Foot = 307,

        Palm = 400,
        Finguer = 401,
        Sole = 402,
        Toe = 403,

        Thumb = 500,
        Index = 501,
        Middle = 502,
        Ring = 503,
        Pinky = 504,
        Hallux = 505,
        Index_toe = 506,
        Middle_toe = 507,
        Ring_toe = 508,
        Pinky_toe = 509,

        First = 600,
        Second = 601,
        Third = 602,
    };

	/// <summary>
	/// Structure for command data in haptic systems.
	/// </summary>
	public struct CommandData
    {

        public CommandData(Operator _sign, GroupID _group, LateralFlag _side = LateralFlag.Global)
        {
            Sign = _sign;
            Group = _group;
            Side = _side;
        }

        // Variables ------------------------------------------------------------------------------------

        /// <summary>
        /// Sign of the operation (+/-)
        /// </summary>
        public Operator Sign;

		/// <summary>
		/// Targeted group.
		/// </summary>
		public GroupID Group;

        /// <summary>
        /// Targeted side.
        /// </summary>
        public LateralFlag Side;

    };

}