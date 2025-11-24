using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Timing
{
    public class PeriodicTimer : MonoBehaviour
    {
        public UnityEvent<bool> OnPeriodStateChanged;

        [SerializeField] private List<float> _times = new List<float>();
        [SerializeField] private float _pauseTimer;
        [SerializeField] private bool _startOnAwake = true;

        private Coroutine _timerCoroutine;

        private void Awake()
        {
            if (_startOnAwake) StartTimer();
        }

        public void StartTimer()
        {
            StopTimer();
            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        public void StopTimer()
        {
            if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        private IEnumerator TimerCoroutine()
        {
            while (true)
            {
                foreach (var t in _times)
                {
                    OnPeriodStateChanged?.Invoke(true);
                    yield return new WaitForSeconds(t);
                    OnPeriodStateChanged?.Invoke(false);
                    yield return new WaitForSeconds(_pauseTimer);
                }
            }
        }
    }
}