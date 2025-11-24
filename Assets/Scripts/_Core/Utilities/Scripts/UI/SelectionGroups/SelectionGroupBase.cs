using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Utilities.UI.SelectionGroups
{
    public abstract class SelectionGroupBase<T> : BaseBehaviour
    {
        [SerializeField] protected Button _leftButton;
        [SerializeField] protected Button _rightButton;

        [SerializeField] protected List<T> _items;
        protected int _currentIndex;
        private bool _isInitialised;

        public virtual void Initialise(List<T> items, int startIndex = 0)
        {
            if (_isInitialised || items == null || items.Count == 0)
                return;

            _items = new List<T>(items);
            _currentIndex = Mathf.Clamp(startIndex, 0, _items.Count - 1);

            _leftButton.onClick.RemoveAllListeners();
            _rightButton.onClick.RemoveAllListeners();

            _leftButton.onClick.AddListener(() => ChangeSelection(-1));
            _rightButton.onClick.AddListener(() => ChangeSelection(1));

            _isInitialised = true;
            UpdateSelection();
        }

        [Button]
        public void ChangeSelection(int direction)
        {
            _currentIndex = (int)Mathf.Repeat(_currentIndex + direction, _items.Count);
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            T selectedItem = _items[_currentIndex];
            OnSelectionChanged(selectedItem);
        }

        public void ToggleInteractable(bool canInteract)
        {
            _leftButton.gameObject.SetActive(canInteract);
            _rightButton.gameObject.SetActive(canInteract);
        }

        protected abstract void OnSelectionChanged(T item);
    }
}