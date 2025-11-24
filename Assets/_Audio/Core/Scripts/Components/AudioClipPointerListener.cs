using Audio.Managers;
using Audio.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Audio.Components
{
    public class AudioClipPointerListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private bool _hasClickAudio;
        [SerializeField]
        private bool _hasHoverAudio;
        [SerializeField]
        private bool _hasReleaseAudio;
        [SerializeField]
        private bool _hasExitHoverAudio;

        [SerializeField, ShowIf("@_hasClickAudio")]
        private AudioPlayerData _clickAudio;
        [SerializeField, ShowIf("@_hasHoverAudio")]
        private AudioPlayerData _hoverAudio;
        [SerializeField, ShowIf("@_hasReleaseAudio")]
        private AudioPlayerData _releaseAudio;
        [SerializeField, ShowIf("@_hasExitHoverAudio")]
        private AudioPlayerData _exitHoverAudio;


        public void OnPointerDown(PointerEventData eventData)
        {
            if(_hasClickAudio)
            {
                PlaySFX(_clickAudio);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_hasHoverAudio)
            {
                PlaySFX(_hoverAudio);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_hasExitHoverAudio)
            {
                PlaySFX(_exitHoverAudio);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_hasReleaseAudio)
            {
                PlaySFX(_releaseAudio);
            }
        }

        public void PlaySFX(AudioPlayerData playerData)
        {
            AudioManager.Instance.PlaySFX(playerData.clipId, playerData.isOneShot, playerData.positionBased ? transform.position : null);
        }
    }

  
}