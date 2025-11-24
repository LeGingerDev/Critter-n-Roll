using UnityEngine;

namespace Utilities.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector2 ToV2(this Vector3 vector) => new Vector2(vector.x, vector.y);

        public static bool IsOnScreen(this Vector3 screenPoint, Camera camera)
        {
            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 &&
                   screenPoint.y < Screen.height;
        }

        public static Vector2 ScreenToCanvasPosition(this Vector3 screenPoint, RectTransform canvasRectTransform,
            Camera camera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, camera,
                out Vector2 position);
            return position;
        }

        public static Vector2 WorldToCanvasPositionClamped(this Vector3 worldPosition, Camera camera,
            RectTransform canvasRectTransform, float buffer = 50f) // buffer in pixels
        {
            // Convert world position to viewport position
            Vector3 viewportPosition = camera.WorldToViewportPoint(worldPosition);

            // Adjust viewport positions by buffer before clamping, converting buffer from pixels to viewport space
            float bufferX = buffer / canvasRectTransform.rect.width;
            float bufferY = buffer / canvasRectTransform.rect.height;
            viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0 + bufferX, 1 - bufferX);
            viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0 + bufferY, 1 - bufferY);

            // Convert the clamped viewport position to a position within the canvas
            float canvasWidth = canvasRectTransform.rect.width;
            float canvasHeight = canvasRectTransform.rect.height;

            // Calculate position within the canvas, with (0,0) being at the bottom left
            float xPosition = (viewportPosition.x * canvasWidth) - (canvasWidth / 2);
            float yPosition = (viewportPosition.y * canvasHeight) - (canvasHeight / 2);

            return new Vector2(xPosition, yPosition);
        }

        public static Vector2 ClampToScreenEdge(this Vector3 screenPoint, RectTransform canvasRectTransform,
            Camera camera)
        {
            float x = Mathf.Clamp(screenPoint.x, 0, Screen.width);
            float y = Mathf.Clamp(screenPoint.y, 0, Screen.height);
            return new Vector3(x, y).ScreenToCanvasPosition(canvasRectTransform, camera);
        }
    }
}