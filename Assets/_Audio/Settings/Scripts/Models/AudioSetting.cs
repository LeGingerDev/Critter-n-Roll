using System;
using UnityEngine;
namespace Audio.Settings.Models
{

    [CreateAssetMenu(fileName = "AudioSettings_", menuName = "BagOfDucks/Audio/Create Audio Settings")]
    public class AudioSetting : ScriptableObject
    {
        [Range(-80, 20)]
        public float audioValue = 0;
        public bool isMuted = false;

        public void SetAudioValue(float value)
        {
            audioValue = value;
        }

        public void SetIsMuted(bool isMuted)
        {
            this.isMuted = isMuted;
        }

        public AudioSettingDTO SerializeToDTO()
        {
            return new AudioSettingDTO
            {
                audioValue = audioValue,
                isMuted = isMuted
            };
        }

        public void DeserializeFromDTO(AudioSettingDTO dto)
        {
            audioValue = dto.audioValue;
            isMuted = dto.isMuted;
        }
    }
}