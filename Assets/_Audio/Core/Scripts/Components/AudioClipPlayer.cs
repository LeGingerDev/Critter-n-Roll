using Audio.Managers;
using Audio.Models;
using UnityEngine;
namespace Audio.Components
{
    public class AudioClipPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioPlayerData _audioPlayerData;

        public void PlaySFX()
        {
            AudioManager.Instance.PlaySFX(_audioPlayerData.clipId, _audioPlayerData.isOneShot, _audioPlayerData.positionBased ? transform.position : null);
        }
    }
}