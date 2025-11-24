using UnityEngine;

namespace Utilities.Mechanics
{
    public class RotateAround2D : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        public void Update()
        {
            if (rotationSpeed == 0)
                return;

            RotateObject();
        }

        public void RotateObject()
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}