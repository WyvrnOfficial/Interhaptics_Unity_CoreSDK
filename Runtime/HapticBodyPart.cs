/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using System.Collections.Generic;
using Interhaptics.HapticBodyMapping;
using Interhaptics.Core;

namespace Interhaptics
{
	[UnityEngine.AddComponentMenu("Interhaptics/HapticBodyPart")]
	public class HapticBodyPart : MonoBehaviour
	{

		public GroupID BodyPart = GroupID.Hand;
		public LateralFlag Side = LateralFlag.Global;

		public List<CommandData> ToCommandData()
		{
			return new List<CommandData> { new CommandData(Operator.Plus, this.BodyPart, this.Side) };
		}

		public bool debugMode = false;
		public int HapticMaterialId { get; set; }
		[SerializeField]
		private double targetIntensity = 1;

		// Property for targetIntensity with a getter and a setter
		public double TargetIntensity
		{
			get => targetIntensity;
			set
			{
				targetIntensity = value;
			}
		}

		public void DebugMode(string debugMessage)
		{
			if (debugMode)
			{
				Debug.Log(debugMessage);
			}
		}
		public void UpdateTargetIntensity(double targetIntensityValue)
		{
			targetIntensity = targetIntensityValue;
			// Create the CommandData array for this HapticBodyPart
			CommandData[] commandDataArray = ToCommandData().ToArray();
			// Update the target intensity for this HapticBodyPart
			HAR.SetTargetIntensityMarshal(HapticMaterialId, commandDataArray, commandDataArray.Length, targetIntensity);
			DebugMode("UpdateTargetIntensity: " + targetIntensity);
		}

	}
}

