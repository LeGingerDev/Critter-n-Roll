using Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastDisplay : BaseBehaviour
{
    [SerializeField, Required] private TextMeshProUGUI _messageText;
    [SerializeField, Required] private Image _iconImage;
    [SerializeField, Required] private CanvasGroup _canvasGroup;
    [SerializeField, Required] private Image _backgroundImage;
    [SerializeField, Required] private Image _stripesImage;

    private RectTransform _rectTransform;
    private ToastData _currentData;
    private ToastColorData _currentColorData;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(ToastData data, ToastColorData toastColorData)
    {
        _currentColorData = toastColorData;
        _currentData = data;
        SetupColorDisplay();
        SetupDisplay();
    }

    private void SetupColorDisplay()
    {
        _backgroundImage.color = _currentColorData.backgroundColor;
        _stripesImage.color = _currentColorData.stripeColor;
    }

    private void SetupDisplay()
    {
        _messageText.text = _currentData.message;

        if (_currentData.icon != null && _iconImage != null)
        {
            _iconImage.sprite = _currentData.icon;
            _iconImage.gameObject.SetActive(true);
        }
        else if (_iconImage != null)
        {
            _iconImage.gameObject.SetActive(false);
        }
    }

    public RectTransform GetRectTransform() => _rectTransform;
    public CanvasGroup GetCanvasGroup() => _canvasGroup;
}

