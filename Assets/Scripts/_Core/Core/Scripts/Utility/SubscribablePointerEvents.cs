using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace General.Utility
{
    public class SubscribablePointerEvents : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public event Action<PointerEventData> OnPointerEnterEvent;
        public event Action<PointerEventData> OnPointerExitEvent;
        public event Action<PointerEventData> OnPointerClickEvent;
        public event Action<PointerEventData> OnPointerDownEvent;
        public event Action<PointerEventData> OnPointerUpEvent;
        public event Action<PointerEventData> OnPointerMoveEvent; // NEW!

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent?.Invoke(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickEvent?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke(eventData);
        }

        public void OnPointerMove(PointerEventData eventData) // NEW!
        {
            OnPointerMoveEvent?.Invoke(eventData);
        }
    }
}