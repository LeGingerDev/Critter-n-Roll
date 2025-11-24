using System;
namespace Audio.Settings.Models
{
    [Serializable]
    public class AudioSettingsSaveDTO
    {
        public AudioSettingDTO masterSettings;
        public AudioSettingDTO sfxSettings;
        public AudioSettingDTO bgmSettings;
    }
}