using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagerment.Splashscreen
{
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField, ReadOnly] private int _maxNumberOfTasks;
        [SerializeField, ReadOnly] private int _currentTask;

        [SerializeField, ReadOnly] private Slider _progressionBar;
        [SerializeField, ReadOnly] private TextMeshProUGUI _progressionText;

        private bool _hasInitialised;

        public void Initialise(int maxNumber)
        {
            if (_hasInitialised) return;

            _hasInitialised = true;

            _progressionText = GetComponentInChildren<TextMeshProUGUI>();
            _progressionBar = GetComponent<Slider>();
            _maxNumberOfTasks = maxNumber;
            UpdateProgression(0);
        }

        [Button]
        public void SetStatus(string message)
        {
            _progressionText.text = message;
            _currentTask++;
            UpdateProgression((float)_currentTask / _maxNumberOfTasks);
        }

        public void UpdateProgression(float percentage)
        {
            _progressionBar.value = percentage;
        }
    }
}