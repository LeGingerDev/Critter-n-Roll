using System;
using System.Collections;
using UnityEngine;

namespace Utilities.Extensions
{
    public static class Rigidbody2DExtension
    {
        public static IEnumerator Knockback(this Rigidbody2D rb2D, int direction, float force, float height, float time,
            Action onComplete)
        {
            Vector2 knockbackDirection = new Vector2(direction * force, height);
            rb2D.linearVelocity = Vector3.zero;
            rb2D.linearVelocity = knockbackDirection;


            yield return new WaitForSeconds(0.5f);

            onComplete?.Invoke();
        }

        /// <summary>
        /// Adjusts the Rigidbody2D's speed by a specified percentage. 
        /// </summary>
        public static void AdjustSpeed(this Rigidbody2D rb, float percentage)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (rb.linearVelocity.magnitude * percentage);
        }

        /// <summary>
        /// Sets the Rigidbody2D's speed to a specific value using it's current direction of movement.
        /// </summary>
        public static void SetSpeed(this Rigidbody2D rb, float newSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * newSpeed;
        }

        /// <summary>
        /// Reverses the direction of the Rigidbody2D's movement.
        /// </summary>
        public static void ReverseDirection(this Rigidbody2D rb)
        {
            rb.linearVelocity *= -1;
        }

        /// <summary>
        /// Moves the Rigidbody2D towards a target position while maintaining its current speed.
        /// </summary>
        public static void MoveTowards(this Rigidbody2D rb, Vector2 targetPosition)
        {
            Vector2 direction = (targetPosition - rb.position).normalized;
            rb.linearVelocity = direction * rb.linearVelocity.magnitude;
        }

        /// <summary>
        /// Moves the Rigidbody2D towards a specified target position at a given speed.
        /// </summary>
        public static void MoveTowards(this Rigidbody2D rb, Vector2 targetPosition, float speed)
        {
            Vector2 direction = (targetPosition - rb.position).normalized; // Direction towards the target
            rb.linearVelocity =
                direction * speed; // Set the velocity in the direction towards the target at the specified speed
        }
    }
}