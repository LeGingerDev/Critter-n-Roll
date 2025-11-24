using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class DynamicCellSizeGridLayout : MonoBehaviour
    {
        public enum Priority
        {
            Rows,
            Columns
        }

        [SerializeField] private Priority _layoutPriority = Priority.Columns;
        [SerializeField] private int _fixedCount = 4; // Default to 4 columns or rows, based on priority
        [SerializeField, Range(0, 2)] private float _sizeRatio = 0.5f; // Percentage value for the other dimension

        private GridLayoutGroup _gridLayout;
        private RectTransform _rectTransform;

        private void LateUpdate()
        {
            Initialize();
        }

        private void Initialize()
        {
            _gridLayout = GetComponent<GridLayoutGroup>();
            _rectTransform = GetComponent<RectTransform>();
            AdjustCellSize();
        }

        [Button]
        private void AdjustCellSize()
        {
            if (_gridLayout == null || _rectTransform == null) return;

            float spacing = _layoutPriority == Priority.Columns ? _gridLayout.spacing.x : _gridLayout.spacing.y;
            float size;

            if (_layoutPriority == Priority.Columns)
            {
                size = (_rectTransform.rect.width - spacing * (_fixedCount - 1) - _gridLayout.padding.left -
                        _gridLayout.padding.right) / _fixedCount;
                _gridLayout.cellSize =
                    new Vector2(size, size * _sizeRatio); // Modify the height based on the width and ratio
            }
            else // Priority.Rows
            {
                size = (_rectTransform.rect.height - spacing * (_fixedCount - 1) - _gridLayout.padding.top -
                        _gridLayout.padding.bottom) / _fixedCount;
                _gridLayout.cellSize =
                    new Vector2(size * _sizeRatio, size); // Modify the width based on the height and ratio
            }
        }

        private void OnValidate()
        {
            Initialize();
        }
    }
}