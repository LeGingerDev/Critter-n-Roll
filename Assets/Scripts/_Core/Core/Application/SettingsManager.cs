using Core.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Application
{
    public class SettingsManager : MonoSingleton<SettingsManager>
    {
        public const string SETTINGS_DATA_KEY = "settings";
        public const string SETTINGS_FILE_KEY = "Settings_SaveData";

        public bool fullscreenToggle = true;
        public int lastResolutionIndex = 0;

        protected override void Awake()
        {
            base.Awake();
            Load();
            SetIsFullscreen(fullscreenToggle);
            SetResolution(lastResolutionIndex);
        }

        public void SetIsFullscreen(bool isFullscreen)
        {
            ResolutionUtilities.SetFullScreen(isFullscreen);
            fullscreenToggle = isFullscreen;
            Save();
        }

        public void SetResolution(int index)
        {
            ResolutionUtilities.SelectResolutionOption(index, fullscreenToggle);
            lastResolutionIndex = index;
            Save();
        }

        public void Save()
        {
            var data = new SettingsDataDTO()
            {
                IsFullscreen = fullscreenToggle,
                ResolutionIndex = lastResolutionIndex
            };
            ES3.Save(SETTINGS_DATA_KEY, data, SETTINGS_FILE_KEY);
        }

        public void Load()
        {
            if (!ES3.FileExists(SETTINGS_FILE_KEY))
                return;

            if (!ES3.KeyExists(SETTINGS_DATA_KEY, SETTINGS_FILE_KEY))
                return;

            var data = ES3.Load<SettingsDataDTO>(SETTINGS_DATA_KEY, SETTINGS_FILE_KEY);
            fullscreenToggle = data.IsFullscreen;
            lastResolutionIndex = data.ResolutionIndex;
        }
    }


    public static class ResolutionUtilities
    {
        public static void SetFullScreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public static readonly Vector2Int[] Resolutions = new Vector2Int[]
        {
            new Vector2Int(1920, 1080),
            new Vector2Int(1600, 900),
            new Vector2Int(1366, 768),
            new Vector2Int(1280, 720),
            new Vector2Int(1024, 576)
        };

        public static List<string> GetResolutionOptions()
        {
            var options = new List<string>();
            foreach (var res in Resolutions)
            {
                options.Add(res.x + " x " + res.y);
            }
            return options;
        }

        public static void SelectResolutionOption(int optionIndex, bool isFullscreen)
        {
            if (optionIndex < 0 || optionIndex >= Resolutions.Length)
                return;
            var res = Resolutions[optionIndex];
            Screen.SetResolution(res.x, res.y, isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
        }
    }

    [Serializable]
    public class SettingsDataDTO
    {
        public bool IsFullscreen;
        public int ResolutionIndex;
    }

}