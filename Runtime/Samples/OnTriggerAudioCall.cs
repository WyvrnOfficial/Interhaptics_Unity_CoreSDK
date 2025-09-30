/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;
using Interhaptics.Utils;

namespace Interhaptics.Samples
{
    public class OnTriggerAudioCall : MonoBehaviour
    {
        [SerializeField]
        private AudioHapticSource audioHapticSource;

        private void OnTriggerEnter(Collider other)
        {
            audioHapticSource.PlayEventVibration();
        }

        private void OnTriggerExit(Collider other)
        {
            audioHapticSource.Stop();
        }
    }
}

