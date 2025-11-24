using Audio.Components;
using Audio.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio.Managers
{
    public partial class AudioManager
    {
        [FoldoutGroup("SFX References"), SerializeField]
        private AudioController _sfxController;  // Controller dedicated to sound effects

        /// <summary>
        /// Plays a sound effect.
        /// Optional parameters allow specifying if it's a one-shot and an optional position for 3D sound.
        /// </summary>
        [Button]
        public void PlaySFX(AudioClipSO clip, bool isOneShot = true, Vector3? position = null)
        {
            if (_sfxController == null)
                return;

            _sfxController.PlayClip(clip, isOneShot, position);
        }

        public void PlaySFX(string clipId, bool isOneShot = true, Vector3? position = null)
        {
            AudioClipSO clip = _audioClipsCollection.GetByID(clipId);
            if (clip == null)
            {
                Debug.LogError($"Clip with ID {clipId} not found.");
                return;
            }
            PlaySFX(clip, isOneShot, position);
        }

        /// <summary>
        /// Stops a specific sound effect.
        /// </summary>
        public void StopSFX(AudioClipSO clip)
        {
            if (_sfxController == null)
                return;

            _sfxController.StopClip(clip);
        }

        public void StopSFX(string clipId)
        {
            AudioClipSO clip = _audioClipsCollection.GetByID(clipId);
            if (clip == null)
            {
                Debug.LogError($"Clip with ID {clipId} not found.");
                return;
            }
            StopSFX(clip);
        }

        /// <summary>
        /// Stops all currently playing sound effects.
        /// </summary>
        public void StopAllSFX()
        {
            if (_sfxController == null)
                return;

            _sfxController.StopAllPlayers();
        }
    }
}