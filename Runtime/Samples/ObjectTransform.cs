/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;

namespace Interhaptics.Samples
{
    public class ObjectTransform : MonoBehaviour
    {
        public float speed = 1f;
        public float leftLimit = -1.5f;
        public float rightLimit = 1.5f;
        public bool moveAtStart = false;
        public bool buttonPressed = false;

        private float targetX;
        private bool movingRight = true;
		private Vector3 originalPosition;
		private bool returningToOrigin = false;

		private void Start()
        {
			originalPosition = transform.position;
			targetX = rightLimit;
        }

        private void Update()
        {
            if ((moveAtStart)||(buttonPressed))
            {
                MoveSpatialHapticSource();
            }
        }

        public void ResetPosition()
        {
			transform.position = originalPosition;
			targetX = rightLimit;
			movingRight = true;
			returningToOrigin = false;
            buttonPressed = false;
		}

        public void MoveSpatialHapticSource()
        {
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );

			if (Mathf.Approximately(transform.position.x, targetX))
            {
				if (returningToOrigin)
				{
					moveAtStart = false; // Stop moving after returning to origin if started due to moveAtStart
                    ResetPosition(); // Reset position to origin
					return; // Exit early as we've returned to the origin
				}

				if (movingRight)
                {
                    targetX = leftLimit;
                }
                else
                {
                    targetX = originalPosition.x;
                    returningToOrigin = true;
                }
                movingRight = !movingRight;
            }
        }
    }
}

