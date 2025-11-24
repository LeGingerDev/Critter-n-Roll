using UnityEngine;
using UnityEngine.UI;
using Utilities.Data;

namespace Core.Utilities.Mechanics.Progressions
{
    public abstract class ProgressVisualiserBase : MonoBehaviour
    {
        [SerializeField] protected Image _progressImage; // assign your Image in the inspector

        // Store the ValueChange for later access.
        protected ValueChange _valueChange;

        /// <summary>
        /// Sets the progress based on the given value change data.
        /// </summary>
        /// <param name="valueChange">The current and maximum values.</param>
        public virtual void SetProgress(ValueChange valueChange)
        {
            if (valueChange.maxValue <= 0)
            {
                Debug.LogWarning("Max value should be greater than zero.");
                return;
            }

            // Store the current ValueChange instance
            _valueChange = valueChange;

            float percentage = Mathf.Clamp01(valueChange.GetPercentage());
            UpdateVisual(percentage);
        }

        /// <summary>
        /// Implement this method to update the UI element with the given fill percentage.
        /// </summary>
        /// <param name="fillAmount">The fill amount (from 0 to 1).</param>
        protected abstract void UpdateVisual(float fillAmount);
    }
}
