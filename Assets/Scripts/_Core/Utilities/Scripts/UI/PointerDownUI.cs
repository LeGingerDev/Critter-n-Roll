using Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Utilities.Scripts.UI
{
    public class PointerDownUI : BaseBehaviour, IPointerDownHandler
    {
        [SerializeField] private UnityEvent _onPointerDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown?.Invoke();
        }
    }
}