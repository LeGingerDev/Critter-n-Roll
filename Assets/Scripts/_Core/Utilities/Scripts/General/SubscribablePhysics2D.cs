using System;
using UnityEngine;

namespace General.Utility
{
    public class SubscribablePhysics2D : MonoBehaviour
    {
        public event Action<Collision2D> OnCollisionEnterEvent;
        public event Action<Collision2D> OnCollisionExitEvent;
        public event Action<Collision2D> OnCollisionStayEvent;

        public event Action<Collider2D> OnTriggerEnterEvent;
        public event Action<Collider2D> OnTriggerExitEvent;
        public event Action<Collider2D> OnTriggerStayEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExitEvent?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerStayEvent?.Invoke(other);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnterEvent?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnCollisionExitEvent?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnCollisionStayEvent?.Invoke(collision);
        }
    }
}