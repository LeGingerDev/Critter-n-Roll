using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class CustomGridLayoutGroup : LayoutGroup
    {
        public enum RowAlignment
        {
            Left,
            Center,
            Right
        }

        public enum Constraint
        {
            FixedRowCount,
            FixedColumnCount,
            Flexible
        }

        public enum StartCorner
        {
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight,
            UpperCenter,
            LowerCenter
        }

        public enum Axis
        {
            Horizontal,
            Vertical
        }

        [SerializeField] private Vector2 _cellSize = new Vector2(100, 100);
        [SerializeField] private Vector2 _spacing = new Vector2(10, 10);
        [SerializeField] private StartCorner _startCorner = StartCorner.UpperLeft;
        [SerializeField] private Axis _startAxis = Axis.Horizontal;
        [SerializeField] private Constraint _constraint = Constraint.Flexible;
        [SerializeField] private RowAlignment _rowAlignment = RowAlignment.Left;
        [SerializeField] private int _constraintCount = 2;
        [SerializeField, Range(0, 2)] private float _aspectRatio = 1f;
        [SerializeField] private bool _fitToSize;

        [SerializeField] private List<int> _itemsPerRow = new List<int>(); // New List to specify items per row

        private int _rows, _columns;

        protected override void OnEnable()
        {
            base.OnEnable();
            CalculateLayout();
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            CalculateLayout();
        }

        public override void CalculateLayoutInputVertical()
        {
            CalculateLayout();
        }

        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0);
        }

        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1);
        }

        [Button]
        private void CalculateLayout()
        {
            CalculateGridDimensions();
            if (_fitToSize)
            {
                AdjustCellSizeToFit();
            }
            else
            {
                AdjustCellSizeBasedOnAspectRatio();
            }

            SetLayoutInputs();
        }

        private void CalculateGridDimensions()
        {
            float totalWidth = rectTransform.rect.width - padding.left - padding.right;
            float totalHeight = rectTransform.rect.height - padding.top - padding.bottom;

            if (_itemsPerRow != null && _itemsPerRow.Count > 0)
            {
                _rows = _itemsPerRow.Count;
                _columns = 0;
                foreach (int items in _itemsPerRow)
                {
                    if (items > _columns)
                        _columns = items;
                }
            }
            else if (_constraint == Constraint.FixedColumnCount)
            {
                _columns = _constraintCount;
                _rows = Mathf.CeilToInt((float)rectChildren.Count / _columns);
            }
            else if (_constraint == Constraint.FixedRowCount)
            {
                _rows = _constraintCount;
                _columns = Mathf.CeilToInt((float)rectChildren.Count / _rows);
            }
            else
            {
                if (_startAxis == Axis.Horizontal)
                {
                    _columns = Mathf.FloorToInt((totalWidth + _spacing.x) / (_cellSize.x + _spacing.x));
                    _rows = Mathf.CeilToInt((float)rectChildren.Count / _columns);
                }
                else
                {
                    _rows = Mathf.FloorToInt((totalHeight + _spacing.y) / (_cellSize.y + _spacing.y));
                    _columns = Mathf.CeilToInt((float)rectChildren.Count / _rows);
                }
            }
        }

        private void AdjustCellSizeToFit()
        {
            float totalWidth = rectTransform.rect.width - padding.left - padding.right;
            float totalHeight = rectTransform.rect.height - padding.top - padding.bottom;

            float cellWidthBasedOnWidth = (totalWidth - (_columns - 1) * _spacing.x) / _columns;
            float cellHeightBasedOnHeight = (totalHeight - (_rows - 1) * _spacing.y) / _rows;

            if (_startAxis == Axis.Horizontal)
            {
                _cellSize.x = Mathf.Min(cellWidthBasedOnWidth, cellHeightBasedOnHeight * _aspectRatio);
                _cellSize.y = _cellSize.x / _aspectRatio;
            }
            else
            {
                _cellSize.y = Mathf.Min(cellHeightBasedOnHeight, cellWidthBasedOnWidth / _aspectRatio);
                _cellSize.x = _cellSize.y * _aspectRatio;
            }
        }

        private void AdjustCellSizeBasedOnAspectRatio()
        {
            if (_startAxis == Axis.Horizontal)
            {
                _cellSize.y = _cellSize.x / _aspectRatio;
            }
            else
            {
                _cellSize.x = _cellSize.y * _aspectRatio;
            }
        }

        private void SetLayoutInputs()
        {
            float totalWidth = padding.left + padding.right + (_columns * _cellSize.x) + ((_columns - 1) * _spacing.x);
            float totalHeight = padding.top + padding.bottom + (_rows * _cellSize.y) + ((_rows - 1) * _spacing.y);

            SetLayoutInputForAxis(totalWidth, padding.left + padding.right, -1, 0);
            SetLayoutInputForAxis(totalHeight, padding.top + padding.bottom, -1, 1);
        }

        private void SetChildrenAlongAxis(int axis)
        {
            if (axis == 0)
            {
                // Set positions along the horizontal axis
                int i = 0;
                for (int row = 0; row < _rows && i < rectChildren.Count; row++)
                {
                    int itemsInRow = (_itemsPerRow != null && _itemsPerRow.Count > row) ? _itemsPerRow[row] : _columns;
                    itemsInRow = Mathf.Min(itemsInRow, rectChildren.Count - i);

                    float rowWidth = itemsInRow * _cellSize.x + (itemsInRow - 1) * _spacing.x;
                    float startX = CalculateStartXPosition(rowWidth);

                    for (int j = 0; j < itemsInRow && i < rectChildren.Count; j++, i++)
                    {
                        float posX = startX + j * (_cellSize.x + _spacing.x);
                        SetChildAlongAxis(rectChildren[i], 0, posX, _cellSize.x);
                    }
                }
            }
            else
            {
                // Set positions along the vertical axis
                float startY = padding.top;
                int i = 0;

                for (int row = 0; row < _rows && i < rectChildren.Count; row++)
                {
                    int itemsInRow = (_itemsPerRow != null && _itemsPerRow.Count > row) ? _itemsPerRow[row] : _columns;
                    itemsInRow = Mathf.Min(itemsInRow, rectChildren.Count - i);

                    for (int j = 0; j < itemsInRow && i < rectChildren.Count; j++, i++)
                    {
                        float posY;

                        if (_startCorner == StartCorner.LowerLeft || _startCorner == StartCorner.LowerRight ||
                            _startCorner == StartCorner.LowerCenter)
                        {
                            posY = rectTransform.rect.height - startY - _cellSize.y;
                        }
                        else
                        {
                            posY = startY;
                        }

                        SetChildAlongAxis(rectChildren[i], 1, posY, _cellSize.y);
                    }

                    startY += _cellSize.y + _spacing.y;
                }
            }
        }

        private float CalculateStartXPosition(float rowWidth)
        {
            switch (_rowAlignment)
            {
                case RowAlignment.Center:
                    return (rectTransform.rect.width - padding.left - padding.right - rowWidth) / 2f + padding.left;
                case RowAlignment.Right:
                    return rectTransform.rect.width - padding.right - rowWidth;
                case RowAlignment.Left:
                default:
                    return padding.left;
            }
        }
    }
}