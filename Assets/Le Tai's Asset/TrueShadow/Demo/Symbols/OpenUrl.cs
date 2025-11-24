// Copyright (c) Le Loc Tai <leloctai.com> . All rights reserved. Do not redistribute.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LeTai.TrueShadow.Demo
{
    public class OpenUrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public string buttonURL;
        Button button;

        void Start()
        {
            button = GetComponent<Button>();
            if (button)
                button.onClick.AddListener(() => Open(buttonURL));
        }

        public void Open(string url)
        {
            Application.OpenURL(url);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(buttonURL))
                Open(buttonURL);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}
