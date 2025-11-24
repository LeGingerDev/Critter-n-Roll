using Core.Singleton;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastManager : MonoSingleton<ToastManager>
{
    [FoldoutGroup("Toast Prefabs")]
    [SerializeField, Required] private ToastDisplay _defaultToastPrefab;
    [FoldoutGroup("Toast Prefabs")]
    [SerializeField, Required] private Dictionary<ToastType, ToastColorData> _toastColorSchemes = new Dictionary<ToastType, ToastColorData>();

    [FoldoutGroup("Spawn Settings")]
    [SerializeField] private Canvas _toastCanvas;
    [FoldoutGroup("Spawn Settings")]
    [SerializeField] private Vector2 _stackSpacing;

    [FoldoutGroup("Animation Settings")]
    [SerializeField] private float _slideInDuration = 0.3f;
    [FoldoutGroup("Animation Settings")]
    [SerializeField] private float _slideDistance = 300f;
    [FoldoutGroup("Animation Settings")]
    [SerializeField] private float _displayDuration = 3f;
    [FoldoutGroup("Animation Settings")]
    [SerializeField] private float _fadeOutDuration = 0.5f;
    [FoldoutGroup("Animation Settings")]
    [SerializeField] private Ease _slideEase = Ease.OutBack;

    [FoldoutGroup("Location Anchors")]
    [SerializeField] private Dictionary<ToastLocation, Transform> _locationAnchors = new Dictionary<ToastLocation, Transform>();

    private Dictionary<ToastLocation, List<ToastDisplay>> _activeToasts = new Dictionary<ToastLocation, List<ToastDisplay>>();

    protected override void Awake()
    {
        base.Awake();
        InitializeLocationLists();
        ValidateSetup();
    }

    private void InitializeLocationLists()
    {
        foreach (ToastLocation location in Enum.GetValues(typeof(ToastLocation)))
        {
            _activeToasts[location] = new List<ToastDisplay>();
        }
    }

    private void ValidateSetup()
    {
        if (_toastCanvas == null)
        {
            Debug.LogError("ToastManager: Toast Canvas is not assigned!", this);
        }

        if (_toastColorSchemes.Count == 0)
        {
            Debug.LogError("ToastManager: No toast prefabs assigned!", this);
        }
    }

    public void SpawnToast(ToastData data, ToastLocation location = ToastLocation.Top)
    {
        if (!CanSpawnToast(data, location)) return;

        ToastDisplay toastPrefab = GetToastPrefab();
        if (toastPrefab == null) return;

        Transform anchor = GetLocationAnchor(location);
        if (anchor == null) return;

        // First, push all existing toasts down by one stack position
        PushExistingToastsDown(location);

        ToastDisplay toastInstance = CreateToastInstance(toastPrefab, data, anchor);

        // Insert at the front of the list (newest at top)
        _activeToasts[location].Insert(0, toastInstance);

        // Position the new toast at the anchor point (top position)
        Vector2 targetPosition = CalculateStackPosition(0, location);
        toastInstance.GetRectTransform().anchoredPosition = targetPosition;

        AnimateToastIn(toastInstance, location);
    }

    private void PushExistingToastsDown(ToastLocation location)
    {
        List<ToastDisplay> toasts = _activeToasts[location];

        for (int i = 0; i < toasts.Count; i++)
        {
            // Each existing toast moves down one position in the stack
            Vector2 newPosition = CalculateStackPosition(i + 1, location);
            toasts[i].GetRectTransform().DOAnchorPos(newPosition, _slideInDuration);
        }
    }

    private bool CanSpawnToast(ToastData data, ToastLocation location)
    {
        if (data == null)
        {
            Debug.LogWarning("ToastManager: Cannot spawn toast with null data!");
            return false;
        }

        if (string.IsNullOrEmpty(data.message))
        {
            Debug.LogWarning("ToastManager: Cannot spawn toast with empty message!");
            return false;
        }

        return true;
    }

    private ToastDisplay GetToastPrefab() => _defaultToastPrefab;
    private ToastColorData GetToastColorData(ToastType toastType) => _toastColorSchemes[toastType];
    private Transform GetLocationAnchor(ToastLocation location)
    {
        if (_locationAnchors.TryGetValue(location, out Transform anchor))
        {
            return anchor;
        }

        Debug.LogError($"ToastManager: No anchor found for location: {location}");
        return null;
    }

    private ToastDisplay CreateToastInstance(ToastDisplay prefab, ToastData data, Transform parent)
    {
        ToastDisplay instance = Instantiate(prefab, parent);
        instance.Initialize(data, GetToastColorData(data.type));
        return instance;
    }

    private Vector2 CalculateStackPosition(int stackIndex, ToastLocation location)
    {
        Vector2 offset = stackIndex * _stackSpacing;

        return location switch
        {
            ToastLocation.Top => new Vector2(0, -offset.y),
            ToastLocation.Bottom => new Vector2(0, offset.y),
            ToastLocation.Left => new Vector2(offset.x, 0),
            ToastLocation.Right => new Vector2(-offset.x, 0),
            _ => Vector2.zero
        };
    }

    private void AnimateToastIn(ToastDisplay toast, ToastLocation location)
    {
        RectTransform rectTransform = toast.GetRectTransform();
        Vector2 endPosition = rectTransform.anchoredPosition; // This is now already correctly set
        Vector2 startPosition = endPosition + GetSlideDirection(location);

        rectTransform.anchoredPosition = startPosition;

        rectTransform.DOAnchorPos(endPosition, _slideInDuration)
            .SetEase(_slideEase)
            .OnComplete(() => StartToastLifecycle(toast, location));
    }

    private Vector2 GetSlideDirection(ToastLocation location)
    {
        return location switch
        {
            ToastLocation.Top => new Vector2(0, _slideDistance),
            ToastLocation.Bottom => new Vector2(0, -_slideDistance),
            ToastLocation.Left => new Vector2(-_slideDistance, 0),
            ToastLocation.Right => new Vector2(_slideDistance, 0),
            _ => Vector2.zero
        };
    }

    private void StartToastLifecycle(ToastDisplay toast, ToastLocation location)
    {
        StartCoroutine(HandleToastLifecycle(toast, location));
    }

    private IEnumerator HandleToastLifecycle(ToastDisplay toast, ToastLocation location)
    {
        yield return new WaitForSeconds(_displayDuration);
        yield return StartCoroutine(FadeOutToast(toast));
        RemoveToastFromStack(toast, location);
    }

    private IEnumerator FadeOutToast(ToastDisplay toast)
    {
        CanvasGroup canvasGroup = toast.GetCanvasGroup();
        yield return canvasGroup.DOFade(0f, _fadeOutDuration).WaitForCompletion();
    }

    private void RemoveToastFromStack(ToastDisplay toast, ToastLocation location)
    {
        List<ToastDisplay> toasts = _activeToasts[location];
        int removedIndex = toasts.IndexOf(toast);

        if (removedIndex == -1)
        {
            Debug.LogWarning("ToastManager: Trying to remove toast that's not in the stack!");
            return;
        }

        toasts.RemoveAt(removedIndex);

        // Move all toasts that were below the removed one up by one position
        PullRemainingToastsUp(location, removedIndex);

        Destroy(toast.gameObject);
    }

    private void PullRemainingToastsUp(ToastLocation location, int removedIndex)
    {
        List<ToastDisplay> toasts = _activeToasts[location];

        // Only move toasts that were positioned after the removed toast
        for (int i = removedIndex; i < toasts.Count; i++)
        {
            Vector2 newPosition = CalculateStackPosition(i, location);
            toasts[i].GetRectTransform().DOAnchorPos(newPosition, _slideInDuration);
        }
    }

    [Button]
    public void TestMessage(ToastType type, ToastLocation location)
    {
        ToastData data = new ToastData("This is a test toast message!", type);
        SpawnToast(data, location);
    }
}

[Serializable]
public class ToastColorData
{
    public Color backgroundColor = Color.white;
    public Color stripeColor = Color.white;
}