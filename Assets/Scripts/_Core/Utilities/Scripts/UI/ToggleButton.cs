using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utility
{
    public class ToggleButton : MonoBehaviour
    {
        private event Action<bool> _onToggleClick;

        [FoldoutGroup("References"), SerializeField]
        private Sprite[] toggleSprites;

        [FoldoutGroup("References"), SerializeField]
        private Image _toggleImage;

        private Button _toggleButton;

        private bool _isToggled;

        private void Awake()
        {
            _toggleButton = GetComponent<Button>();
        }

        public void Initialise(Action<bool> onClickAction, bool isToggleTrue = true)
        {
            _isToggled = isToggleTrue;

            _onToggleClick = onClickAction;

            SwapSprites();

            _toggleButton.onClick.RemoveAllListeners();
            _toggleButton.onClick.AddListener(() => { Toggle(); });
        }

        public void Toggle()
        {
            _isToggled = !_isToggled;
            SwapSprites();
            _onToggleClick?.Invoke(_isToggled);
        }

        public void SwapSprites()
        {
            Sprite spriteToShow = _isToggled ? toggleSprites[1] : toggleSprites[0];
            _toggleImage.sprite = spriteToShow;
        }
    }
}