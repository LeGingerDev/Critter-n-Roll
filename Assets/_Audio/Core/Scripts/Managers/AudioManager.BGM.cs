using Audio.Components;
using Audio.Models;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
namespace Audio.Managers
{
    public partial class AudioManager
    {
        [FoldoutGroup("BGM References"), SerializeField]
        private AudioController _bgmController;  // Controller dedicated to music

        /// <summary>
        /// Plays a background music track.
        /// If crossfade is enabled, you might want to gradually transition from the current track to the new one.
        /// </summary>
        [Button]
        public void PlayBGMTrack(AudioClipSO clip, bool crossfade = false)
        {
            if (_bgmController == null)
                return;

            // Prevent replaying the same track
            if (_bgmController.GetIsClipPlaying(clip))
            {
                Debug.Log("BGM track is already playing.");
                return;
            }

            if (crossfade)
            {
                HandleCrossFading(clip);
                return;
            }

            _bgmController.StopAllPlayers();
            _bgmController.PlayClip(clip, isOneShot: false);
        }

        public void PlayBGMTrack(string clipID, bool crossFade)
        {
            AudioClipSO clip = _audioClipsCollection.GetByID(clipID);
            if (clip == null)
            {
                Debug.LogError($"Clip with ID {clipID} not found.");
                return;
            }
            PlayBGMTrack(clip, crossFade);
        }

        private void HandleCrossFading(AudioClipSO clip)
        {
            AudioPlayer activePlayer = _bgmController.GetFirstAudioPlayerActive();
            AudioPlayer newPlayer = _bgmController.GetAvailablePlayer();

            StartCoroutine(HandleFadingTiming(activePlayer, newPlayer, clip, 1f));

        }

        private IEnumerator HandleFadingTiming(AudioPlayer from, AudioPlayer to, AudioClipSO clip, float fadeDuration)
        {
            if(from != null)
            {
                StartCoroutine(HandleFadingOfPlayer(from, 0f, fadeDuration));
                yield return new WaitForSeconds(fadeDuration / 2);
            }

            StartCoroutine(HandleFadingOfPlayer(to, clip.volume, fadeDuration));
            to.PlayAudio(clip, isOneShot: false, startVolume: 0f);
        }

        private IEnumerator HandleFadingOfPlayer(AudioPlayer audioPlayer, float targetValue, float duration)
        {
            if (audioPlayer == null)
            {
                Debug.LogError("AudioPlayer is null");
                yield break;
            }

            float startValue = audioPlayer.GetVolume();
            float currentTime = 0f;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioPlayer.SetVolume(Mathf.Lerp(startValue, targetValue, currentTime / duration));
                yield return null;
            }
            audioPlayer.SetVolume(targetValue);
            if(targetValue == 0f)
                audioPlayer.StopClip();
        }
        /// <summary>
        /// Stops a specific background music track.
        /// </summary>
        public void StopBGMTrack(AudioClipSO clip)
        {
            if (_bgmController == null)
                return;

            _bgmController.StopClip(clip);
        }

        public void StopBGMTrack(string clipID)
        {
            AudioClipSO clip = _audioClipsCollection.GetByID(clipID);
            if (clip == null)
            {
                Debug.LogError($"Clip with ID {clipID} not found.");
                return;
            }
            StopBGMTrack(clip);
        }

        // Optionally, add a method to stop all music tracks.
        public void StopAllBGMTracks()
        {
            _bgmController.StopAllPlayers();
        }
    }
}