using TMPro;
using UnityEngine;

namespace Core.Application
{
    public class FPSCounter : MonoBehaviour
    {
        [Tooltip("Drag your TextMeshProUGUI component here.")]
        public TextMeshProUGUI fpsText;

        // Variables to accumulate frames and time
        private int frameCount;
        private float deltaTime;
        private float fps;

        // How often to update the FPS reading (in seconds)
        [SerializeField, Tooltip("Time interval in seconds for FPS update.")]
        private float updateInterval = 0.5f;

        void Update()
        {
            // Count the frames and the total time elapsed (using unscaledDeltaTime to be unaffected by time scale)
            frameCount++;
            deltaTime += Time.unscaledDeltaTime;

            // When the update interval has passed, calculate the FPS and update the UI text
            if (deltaTime <= updateInterval) return;

            fps = frameCount / deltaTime;

            if (fpsText)
            {
                fpsText.text = $"FPS: {fps:F1}";
            }

            // Reset counters for the next interval
            frameCount = 0;
            deltaTime = 0f;
        }
    }
}