/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interhaptics;

namespace Interhaptics
{
    [AddComponentMenu("Interhaptics/SpatialHapticSource")]
    [RequireComponent(typeof(Rigidbody))]
    public class SpatialHapticSource : Internal.HapticSource
    {

#region Enums
		public enum PlayMethod
        {
            Undefined,
            OnCollision,
            OnTrigger
        }

        public PlayMethod playMethod;
#endregion

#region Lifecycle
		protected override void Start()
        {
			isLooping = false;
			playAtStart = false;
			base.Start();
		}
		#endregion

		public override void PlayEventVibration()
		{
			base.PlayEventVibration();
		}

		/// <summary>
		/// Controls the vibration perception based on the full length of the haptic material; stops any residual haptics which might come from the controller after the haptic playback length
		/// </summary>
		/// <returns></returns>
		public override IEnumerator ControlVibration()
        {
            DebugMode(string.Format("Started playing  haptics on Spatial HS! + {0}",Time.time));
            Play();
            yield return new WaitForSeconds((float)hapticEffectDuration);
            DebugMode(string.Format("Finished playing haptics at timestamp on SpatialHS : + {0} at {1}", hapticEffectDuration, Time.time));
        }

#if UNITY_EDITOR
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void Reset()
        {
            SphereCollider sphere = GetComponent<SphereCollider>();
            BoxCollider box = GetComponent<BoxCollider>();

            if (sphere == null && box == null)
            {
                if (UnityEditor.EditorUtility.DisplayDialog("Choose a Component", "You are missing one of the required componets. Please choose one to add", "SphereCollider", "BoxCollider"))
                {
                    gameObject.AddComponent<SphereCollider>().isTrigger = true;
                }
                else
                {
                    gameObject.AddComponent<BoxCollider>().isTrigger = true;
                }
            }
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
#endif

        public void AddTarget(GameObject target)
        {
            if (target.TryGetComponent(out HapticBodyPart hapticBodyPart))
            {
				hapticBodyPart.HapticMaterialId = this.HapticMaterialId;
				AddTarget(hapticBodyPart.ToCommandData());
				hapticBodyPart.UpdateTargetIntensity(hapticBodyPart.TargetIntensity); // Update the target intensity when adding the target
			}
        }

        public void RemoveTarget(GameObject target)
        {
            if (target.TryGetComponent(out HapticBodyPart hbp))
            {
                RemoveTarget(hbp.ToCommandData());
            }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if ((playMethod == PlayMethod.OnCollision)&&(other.gameObject.GetComponent<HapticBodyPart>() != null))
            {
#if ENABLE_METAQUEST && UNITY_ANDROID && !UNITY_EDITOR
				Core.HAR.PlayHapticEffect(hapticMaterial, SourceIntensity, 1, 0, other.gameObject.GetComponent<HapticBodyPart>().Side);
#else
				ActivateHaptics(other.gameObject);
#endif
            }
		}


		protected virtual void OnTriggerEnter(Collider other)
        {
            if ((playMethod == PlayMethod.OnTrigger) && (other.gameObject.GetComponent<HapticBodyPart>()!=null))
            {
#if ENABLE_METAQUEST && UNITY_ANDROID && !UNITY_EDITOR
				Core.HAR.PlayHapticEffect(hapticMaterial, SourceIntensity, 1, 0, other.gameObject.GetComponent<HapticBodyPart>().Side);
                DebugMode("Haptic Triggered on " + hapticMaterial + " with intensity: " + SourceIntensity + " on " + other.gameObject.name);
#else
				ActivateHaptics(other.gameObject);
#endif
            }
        }

        private void ActivateHaptics(GameObject other)
        {
            AddTarget(other);
            
            if (hapticEffectDuration > 0)
            {
                PlayEventVibration();
            }
            else
            {
                Play();
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
#if ENABLE_METAQUEST && UNITY_ANDROID && !UNITY_EDITOR
			Core.HAR.StopEvent(HapticMaterialId);
#else
            Stop();
            RemoveTarget(other.gameObject);
#endif
        }

        protected virtual void OnCollisionExit(Collision other)
        {
#if ENABLE_METAQUEST && UNITY_ANDROID && !UNITY_EDITOR
			Core.HAR.StopEvent(HapticMaterialId);
#else
			Stop();
            RemoveTarget(other.gameObject);
#endif
		}
	}
}