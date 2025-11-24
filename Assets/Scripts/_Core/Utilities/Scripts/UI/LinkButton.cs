using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class LinkButton : BaseBehaviour
    {
        private Button _button;

        [SerializeField] private string _url;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        protected override void OnEnable()
        {
            _button?.onClick.RemoveAllListeners();
            _button?.onClick.AddListener(OpenUrl);
        }

        protected override void OnDisable()
        {
            _button?.onClick.RemoveAllListeners();
        }

        private void OpenUrl()
        {
            Application.OpenURL(_url);
        }
    }
}