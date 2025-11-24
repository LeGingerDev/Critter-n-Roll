using Audio.Settings.Models;
using Core.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
namespace Audio.Settings.Managers
{
    public class AudioSettingsManager : MonoSingleton<AudioSettingsManager>
    {
        [FoldoutGroup("References"), SerializeField]
        private AudioMixer _audioMixer;

        [FoldoutGroup("Defaults"), SerializeField]
        private AudioSetting _masterDefaultSettings;
        [FoldoutGroup("Defaults"), SerializeField]
        private AudioSetting _sfxDefaultSettings;
        [FoldoutGroup("Defaults"), SerializeField]
        private AudioSetting _bgmDefaultSettings;

        [FoldoutGroup("Runtime"), SerializeField]
        private AudioSettingDTO _masterRuntimeSettings;   
        [FoldoutGroup("Runtime"), SerializeField]
        private AudioSettingDTO _sfxRuntimeSettings;
        [FoldoutGroup("Runtime"), SerializeField]
        private AudioSettingDTO _bgmRuntimeSettings;

        public AudioSettingDTO SfxRuntimeSettings => _sfxRuntimeSettings;
        public AudioSettingDTO BgmRuntimeSettings => _bgmRuntimeSettings;
        public AudioSettingDTO MasterRuntimeSettings => _masterRuntimeSettings;
        [Button]
        public float SfxVolume
        {
            get => _sfxRuntimeSettings.audioValue;
            set
            {
                _sfxRuntimeSettings.audioValue = value;
                _audioMixer.SetFloat("SFXVolume", value);
                Publish(AudioEventIds.ON_SETTINGS_UPDATED);
            }
        }
        [Button]
        public float BgmVolume
        {
            get => _bgmRuntimeSettings.audioValue;
            set
            {
                _bgmRuntimeSettings.audioValue = value;
                _audioMixer.SetFloat("BGMVolume", value);
                Publish(AudioEventIds.ON_SETTINGS_UPDATED);
            }
        }
        [Button]
        public float MasterVolume
        {
            get => _masterRuntimeSettings.audioValue;
            set
            {
                _masterRuntimeSettings.audioValue = value;
                _audioMixer.SetFloat("MasterVolume", value);
                Publish(AudioEventIds.ON_SETTINGS_UPDATED);
            }
        }

        private void Start()
        {
            SetSavedData(AudioSettingsSaveManager.Instance.LoadAudioSettings());
        }

        public void SetSavedData(AudioSettingsSaveDTO saveData)
        {
            if (saveData == null)
            {
                SetDefaults();
            }
            else
            {
                _masterRuntimeSettings = saveData.masterSettings;
                _sfxRuntimeSettings = saveData.sfxSettings;
                _bgmRuntimeSettings = saveData.bgmSettings;
            }

            ForceUpdate();
        }

        public void SetDefaults()
        {
            _masterRuntimeSettings = _masterDefaultSettings.SerializeToDTO();
            _sfxRuntimeSettings = _sfxDefaultSettings.SerializeToDTO();
            _bgmRuntimeSettings = _bgmDefaultSettings.SerializeToDTO();
        }

        public void ForceUpdate()
        {
            MasterVolume = MasterVolume;
            SfxVolume = SfxVolume;
            BgmVolume = BgmVolume;
        }

        [Button]
        public void SaveSettings() => AudioSettingsSaveManager.Instance.SaveAudioSettings();
    }
}