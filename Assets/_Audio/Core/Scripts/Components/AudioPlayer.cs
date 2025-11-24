using Audio.Models;
using UnityEngine;
namespace Audio.Components
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioClipSO _currentAudioClip;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudio(AudioClipSO clip, bool isOneShot = false, Vector3? position = null, float startVolume = -1f)
        {
            transform.position = position.HasValue ? position.Value : Vector3.zero;

            _currentAudioClip = clip;
            _audioSource.spatialBlend = position.HasValue ? 1f : 0f;
            _audioSource.pitch = clip.GetRandomPitch();
            _audioSource.volume = startVolume != -1f? startVolume : clip.volume;
            _audioSource.loop = clip.isLooping;
            _audioSource.clip = clip.GetRandomClip();
            _audioSource.spatialBlend = clip.spatialBlend;

            if (isOneShot)
            {
                PlayAudioOneShot(clip);
                return;
            }

            _audioSource.Play();
        }

        private void PlayAudioOneShot(AudioClipSO clip)
        {
            _audioSource.PlayOneShot(clip.GetRandomClip(), clip.volume);
        }

        public void SetVolume(float volume) => _audioSource.volume = volume;
        public float GetVolume() => _audioSource.volume;

        public bool IsPlaying() => _audioSource.isPlaying;
        public bool IsPlaying(AudioClipSO clip)
        {
            if(_currentAudioClip == null || !IsPlaying())
                return false;

            return _currentAudioClip == clip;
        }

            
        public void StopClip()
        {
            _audioSource.Stop();

            _currentAudioClip = null;
            _audioSource.clip = null;
        }

        public void ResetController()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            _audioSource.volume = 0f;
        }

        public void OnValidate()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
                _audioSource.playOnAwake = false;
            }
        }
    }
}