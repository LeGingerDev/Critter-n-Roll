using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.UI
{
    [RequireComponent(typeof(RawImage))]
    public class PreserveAspectRawImage : MonoBehaviour
    {
        private RawImage _rawImage;
        private RectTransform _rectTransform;

        [FoldoutGroup("Padding")] [SerializeField]
        private float paddingLeft = 10f;

        [FoldoutGroup("Padding")] [SerializeField]
        private float paddingRight = 10f;

        [FoldoutGroup("Padding")] [SerializeField]
        private float paddingTop = 10f;

        [FoldoutGroup("Padding")] [SerializeField]
        private float paddingBottom = 10f;

        void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private IEnumerator Start()
        {
            yield return null;
            AdjustToFitAnchorsWithPadding();
        }

        [Button("Adjust To Fit Anchors With Padding")]
        private void AdjustToFitAnchorsWithPadding()
        {
            if (_rawImage.texture == null) return;

            AdjustToFitAnchorsWithPadding(_rawImage.texture.width, _rawImage.texture.height);
        }

        // Overloaded method to set size before texture is set
        public void AdjustToFitAnchorsWithPadding(float newTextureWidth, float newTextureHeight)
        {
            float textureAspect = newTextureWidth / newTextureHeight;

            RectTransform parentRectTransform = _rectTransform.parent as RectTransform;
            float parentWidth = parentRectTransform.rect.width - (paddingLeft + paddingRight);
            float parentHeight = parentRectTransform.rect.height - (paddingTop + paddingBottom);

            float anchorWidth = parentWidth * (_rectTransform.anchorMax.x - _rectTransform.anchorMin.x);
            float anchorHeight = parentHeight * (_rectTransform.anchorMax.y - _rectTransform.anchorMin.y);

            float newWidth, newHeight;

            // First, fit to width
            newWidth = anchorWidth;
            newHeight = newWidth / textureAspect;

            // If the new height is greater than the anchor height, fit to height instead
            if (newHeight > anchorHeight)
            {
                newHeight = anchorHeight;
                newWidth = newHeight * textureAspect;
            }

            // Adjust the RawImage size
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

            // Center the image within the anchor bounds
            CenterImage();
        }

        private void CenterImage()
        {
            // This assumes anchors are symmetrically placed and that the parent's RectTransform is the frame of reference.
            _rectTransform.anchoredPosition =
                new Vector2(paddingLeft - paddingRight, paddingTop - paddingBottom) * 0.5f;
        }
    }
}