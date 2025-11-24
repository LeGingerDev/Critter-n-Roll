using Audio.Settings.Managers;
using Core.Application;
using Core.Events;
using Core.Singleton;
using DG.Tweening;
using SceneManagement;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoSingleton<SettingsMenu>
{
    [FoldoutGroup("UI Elements")]
    public Transform _settingsPanel;
    [FoldoutGroup("UI Elements")]
    public CanvasGroup panelCanvasGroup;
    [FoldoutGroup("Settings Elements")]
    public Slider masterVolumeSlider;
    [FoldoutGroup("Settings Elements")]
    public Slider musicVolumeSlider;
    [FoldoutGroup("Settings Elements")]
    public Slider sfxVolumeSlider;
    [FoldoutGroup("Settings Elements")]
    public TMP_Dropdown resolutionDropdown;
    [FoldoutGroup("Settings Elements")]
    public CustomToggle fullscreenToggle;

    [FoldoutGroup("Situational Elements")]
    public GameObject deleteUserDataButton;
    [FoldoutGroup("Situational Elements")]
    public GameObject backToMainMenuButton;

    public void Start()
    {
        CloseImmediate();
        InitialiseValues();
    }

    [Topic(SettingsEventIds.ON_SETTINGS_SELECTED)]
    public void Open(object sender, bool isOpenedByKey)
    {
        OpenEffect();
        InitialiseOnOpen();
        ToggleSituational(isOpenedByKey);

        
    }

    public void Close()
    {
        CloseEffect();
        DeinitialiseOnClose();
    }

    public void CloseImmediate()
    {
        panelCanvasGroup.DOFade(0, 0).SetUpdate(true).OnComplete(() =>
        {
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        });
        _settingsPanel.transform.DOScale(0.8f, 0).SetUpdate(true).SetEase(Ease.InBack);
    }

    public void OpenEffect()
    {
        panelCanvasGroup.DOFade(1, 0.3f).From(0).SetUpdate(true).OnComplete(() =>
        {
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
        });
        _settingsPanel.transform.DOScale(1, 0.3f).From(0.8f).SetUpdate(true).SetEase(Ease.OutBack);
    }

    public void CloseEffect()
    {
        panelCanvasGroup.DOFade(0, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        });
        _settingsPanel.transform.DOScale(0.8f, 0.3f).SetUpdate(true).SetEase(Ease.InBack);
    }

    public void InitialiseOnOpen()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);

        musicVolumeSlider.value = AudioSettingsManager.Instance.BgmVolume;
        sfxVolumeSlider.value = AudioSettingsManager.Instance.SfxVolume;
        masterVolumeSlider.value = AudioSettingsManager.Instance.MasterVolume;
    }

    public void DeinitialiseOnClose()
    {
        musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);
        masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);

        AudioSettingsManager.Instance.SaveSettings();
    }

    public void SetMusicVolume(float volume) => AudioSettingsManager.Instance.BgmVolume = volume;
    public void SetSFXVolume(float volume) => AudioSettingsManager.Instance.SfxVolume = volume;
    public void SetMasterVolume(float volume) => AudioSettingsManager.Instance.MasterVolume = volume;

    public void ToggleSituational(bool isOpenedByKey)
    {
        deleteUserDataButton.SetActive(!isOpenedByKey);
        backToMainMenuButton.SetActive(isOpenedByKey);
    }

    public void BackToMainMenu()
    {
        ActivePlayerManager.Instance.ClearAllActive();
        SceneManager.Instance.GoToLevel("MainMenu");
        Close();
    }

    public void ClearAllUsers()
    {
        ConfirmPopupData confirmData = new ConfirmPopupData
        {
            title = "Delete All User Data",
            message = "Are you sure you want to delete all user data? This action cannot be undone.",
            confirmButtonText = "Confirm",
            cancelButtonText = "Cancel",
            confirmType = ConfirmPopup.ConfirmType.Critical,
            onConfirm = () =>
            {
                PlayerDataManager.Instance.ClearAll();
            },
            onCancel = () =>
            {
            },
        };

        ConfirmPopup.Instance.Open(confirmData);
    }
    public void InitialiseValues()
    {
        fullscreenToggle.Initialise(SettingsManager.Instance.fullscreenToggle,
            () => SettingsManager.Instance.SetIsFullscreen(true),
            () => SettingsManager.Instance.SetIsFullscreen(false),
            false);

        var options = ResolutionUtilities.GetResolutionOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = SettingsManager.Instance.lastResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(index =>
        {
            SettingsManager.Instance.SetResolution(index);
        });
    }
}
