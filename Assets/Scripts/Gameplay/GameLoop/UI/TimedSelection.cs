using UnityEngine;
using UnityEngine.UI;
using Core;
using System.Collections;
using TMPro;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

public class TimedSelection : BaseBehaviour
{
    [SerializeField] private bool isSkippable;
    [SerializeField] private Image _timerImage;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _infiniteSign;
    [SerializeField] private Button skipButton;

    private bool _isInfinite = false;
    private bool _isWaiting = false;
    private float _internalTimer;

    /// <summary>
    /// Expose whether the timer is currently running.
    /// </summary>
    public bool IsWaiting => _isWaiting;


    public void ToggleInfinite(bool isInfinite)
    {
        if (_isInfinite == isInfinite)
            return;

        _isInfinite = isInfinite;
        _infiniteSign.SetActive(isInfinite);
        _timerText.gameObject.SetActive(!isInfinite);
        UpdateSkipButtonVisibility();
    }

    public IEnumerator WaitTime(float duration, bool isSkippable = false)
    {
        _isWaiting = true;
        this.isSkippable = isSkippable;
        _internalTimer = duration;
        UpdateSkipButtonVisibility();
        UpdateTimer(_internalTimer, duration);

        while (_internalTimer > 0)
        {
            _internalTimer -= 1;
            UpdateTimer(_internalTimer, duration);
            yield return new WaitForSeconds(1);
        }

        _isWaiting = false;
    }

    [Button]
    public void ForceComplete()
    {
        if (_isWaiting)
        {
            _isWaiting = false;
            _internalTimer = 0;
            UpdateTimer(0, 1);
        }
    }

    [Button]
    public void SkipToEnd()
    {
        if (_isWaiting && !_isInfinite)
        {
            _internalTimer = 1f;
            UpdateTimer(_internalTimer, _internalTimer);
        }
    }

    public void SetToFullVisuals()
    {
        UpdateTimer(1f, 1f);
    }

    public void UpdateTimer(float currentTime, float maxDuration)
    {
        if (_timerImage != null)
        {
            _timerImage.fillAmount = currentTime / maxDuration;
        }

        if (_timerText != null)
        {
            _timerText.text = $"{currentTime:F0}";
            _timerText.transform
                .DOPunchScale(transform.localScale * 0.1f, 0.2f, 10, 0.5f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => _timerText.transform.localScale = Vector3.one);
        }
    }

    private void UpdateSkipButtonVisibility()
    {
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(isSkippable && !_isInfinite);
        }
    }
}