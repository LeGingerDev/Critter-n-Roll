using Core.Singleton;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ConfirmPopupColourSetter))]
public class ConfirmPopup : MonoSingleton<ConfirmPopup>
{
    public enum ConfirmType
    {
        Normal,
        Warning,
        Critical
    }

    [SerializeField, FoldoutGroup("Visual Elements")]
    private Transform _popupPanel;
    [SerializeField, FoldoutGroup("Visual Elements")]
    private CanvasGroup _panelFadeGroup;

    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI _titleText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI _messageText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI _confirmButtonText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI _cancelButtonText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private Button _confirmButton;
    [SerializeField, FoldoutGroup("UI Elements")]
    private Button _cancelButton;

    private ConfirmPopupData _currentPopupData;
    private ConfirmPopupColourSetter _colorSetter;

    protected override void Awake()
    {
        base.Awake();
        _colorSetter = GetComponent<ConfirmPopupColourSetter>();
    }

    private void Start()
    {
        CloseImmediate();
    }

    public void Open(ConfirmPopupData confirmPopupData)
    {
        _currentPopupData = null;

        _panelFadeGroup.DOFade(1, 0.3f).From(0).SetUpdate(true).OnComplete(() =>
        {
            _panelFadeGroup.interactable = true;
            _panelFadeGroup.blocksRaycasts = true;
        });
        _popupPanel.transform.DOScale(1, 0.3f).From(0.8f).SetUpdate(true).SetEase(Ease.OutBack);

        _colorSetter.Initialise(confirmPopupData.confirmType);
        Initialise(confirmPopupData);
    }

    public void Close()
    {
        _panelFadeGroup.DOFade(0, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            _panelFadeGroup.interactable = false;
            _panelFadeGroup.blocksRaycasts = false;
        });
        _popupPanel.transform.DOScale(0.8f, 0.3f).SetUpdate(true).SetEase(Ease.InBack);
    }
    public void CloseImmediate()
    {
        _panelFadeGroup.DOFade(0, 0).SetUpdate(true).OnComplete(() =>
        {
            _panelFadeGroup.interactable = false;
            _panelFadeGroup.blocksRaycasts = false;
        });
        _popupPanel.transform.DOScale(0.8f, 0).SetUpdate(true).SetEase(Ease.InBack);
    }

    public void Initialise(ConfirmPopupData confirmData)
    {
        _currentPopupData = confirmData;

        _titleText.text = _currentPopupData.title;
        _messageText.text = _currentPopupData.message;
        _confirmButtonText.text = _currentPopupData.confirmButtonText;
        _cancelButtonText.text = _currentPopupData.cancelButtonText;

        _confirmButton.onClick.AddListener(() => Confirm());
        _cancelButton.onClick.AddListener(() => Cancel());
    }

    public void Deinitialise()
    {
        _confirmButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
        _currentPopupData = null;
    }

    public void Confirm()
    {
        _currentPopupData.onConfirm?.Invoke();
        Close();
    }

    public void Cancel()
    {
        _currentPopupData.onCancel?.Invoke();
        Close();
    }
}
