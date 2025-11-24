using System;
using UnityEngine;

namespace General.Utility
{
    public class SubscribablePhysics : MonoBehaviour
    {
        public event Action<Collision> OnCollisionEnterEvent;
        public event Action<Collision> OnCollisionExitEvent;
        public event Action<Collision> OnCollisionStayEvent;

        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<Collider> OnTriggerExitEvent;
        public event Action<Collider> OnTriggerStayEvent;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent?.Invoke(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterEvent?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnCollisionExitEvent?.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            OnCollisionStayEvent?.Invoke(collision);
        }
    }
}