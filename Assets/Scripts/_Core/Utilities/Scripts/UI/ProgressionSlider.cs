using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.UI
{
    public class ProgressionSlider : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField]
        private Slider _slider;

        [FoldoutGroup("References"), SerializeField]
        private TextMeshProUGUI _valueText;

        [FoldoutGroup("Stats"), SerializeField, ReadOnly]
        private float _minValue;

        [FoldoutGroup("Stats"), SerializeField, ReadOnly]
        private float _maxValue;

        [FoldoutGroup("Stats"), SerializeField, ReadOnly]
        private float _currentValue;

        public void Initialise(float min, float max, float current)
        {
            _minValue = min;
            _maxValue = max;
            _currentValue = current;

            // Ensure the slider is filled if current is greater than max.
            float percentage = (current >= max) ? 1f : current.MapTo(min, max);
            _slider.value = percentage;

            // The text now should reflect the filled state correctly if current is greater than max.
            _valueText.text = $"{(int)Mathf.Min(current, max)} / {(int)max}";
        }
    }
}