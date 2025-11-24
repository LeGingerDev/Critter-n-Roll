using Audio.Core;
using Audio.Managers;
using Audio.Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Audio.Components
{
    //Controls all AudioPlayers beneath it. Sits under the AudioManager Object.
    public class AudioController : MonoBehaviour
    {
        [FoldoutGroup("Pooling"), SerializeField]
        private AudioPlayer _pooledAudioPlayer;
        [FoldoutGroup("Pooling"), SerializeField]
        private int _startAudioPlayers = 10;

        private List<AudioPlayer> _audioPlayers = new List<AudioPlayer>();

        private void Start()
        {
            for (int i = 0; i < _startAudioPlayers; i++)
            {
                AddAudioPlayer();
            }
        }


        public void PlayClip(AudioClipSO clip, bool isOneShot = false, Vector3? position = null)
        {
            AudioPlayer player = GetAvailablePlayer();
            player.PlayAudio(clip, isOneShot, position);
        }

        public void StopClip(AudioClipSO clip)
        {
            AudioPlayer player = GetPlayerPlayingClip(clip);
            if (player == null)
                return;
            player.StopClip();
        }

        public void StopAllPlayers() => _audioPlayers.ForEach(x => x.StopClip());
        public AudioPlayer GetAvailablePlayer() => IsPlayerAvailable() ? _audioPlayers.FirstOrDefault(x => !x.IsPlaying()) : AddAudioPlayer();
        public AudioPlayer GetPlayerPlayingClip(AudioClipSO clip) => _audioPlayers.FirstOrDefault(x => x.IsPlaying(clip));
        public bool IsPlayerAvailable() => _audioPlayers.Any(x => !x.IsPlaying());
        public bool GetIsClipPlaying(AudioClipSO clip) => _audioPlayers.Any(x => x.IsPlaying(clip));
        public AudioPlayer GetFirstAudioPlayerActive() => _audioPlayers.FirstOrDefault(x => x.IsPlaying());
        public AudioPlayer AddAudioPlayer()
        {
            AudioPlayer newPlayer = Instantiate(_pooledAudioPlayer, transform);
            _audioPlayers.Add(newPlayer);
            return newPlayer;
        }
    }
}