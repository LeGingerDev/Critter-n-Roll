using Audio.Settings.Models;
using Core.Singleton;
using Newtonsoft.Json;
using UnityEngine;
namespace Audio.Settings.Managers
{
    public class AudioSettingsSaveManager : MonoSingleton<AudioSettingsSaveManager>
    {
        public void SaveAudioSettings()
        {
            AudioSettingsSaveDTO saveData = new AudioSettingsSaveDTO()
            {
                masterSettings = AudioSettingsManager.Instance.MasterRuntimeSettings,
                sfxSettings = AudioSettingsManager.Instance.SfxRuntimeSettings,
                bgmSettings = AudioSettingsManager.Instance.BgmRuntimeSettings
            };

            string savedSettings = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString("AudioSettings", savedSettings);
        }
        public AudioSettingsSaveDTO LoadAudioSettings()
        {
            string savedSettings = PlayerPrefs.GetString("AudioSettings");
            if(string.IsNullOrEmpty(savedSettings))
            {
                return null;
            }
            AudioSettingsSaveDTO saveData = JsonConvert.DeserializeObject<AudioSettingsSaveDTO>(savedSettings);
            return saveData;
        }

        public void ClearSettings()
        {
            PlayerPrefs.DeleteKey("AudioSettings");
        }
    }
}