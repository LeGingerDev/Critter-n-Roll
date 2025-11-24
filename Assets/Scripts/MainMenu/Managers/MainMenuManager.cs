using Audio.Core;
using Audio.Managers;
using Tasks;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private TaskManager _startGameTM;

    private void Start()
    {
        AudioManager.Instance.PlayBGMTrack(AudioConstIds.MAIN_MENU, true);
    }

    public void StartGame()
    {
        StartCoroutine(_startGameTM.Execute());
    }

    public void CloseApplication()
    {
        ConfirmPopupData closeData = new ConfirmPopupData
        {
            title = "Close Application",
            message = "Are you sure you want to close the application?",
            confirmButtonText = "Yes",
            cancelButtonText = "No",
            onConfirm = () => Application.Quit(),
            onCancel = () => { }
        };

        ConfirmPopup.Instance.Open(closeData);
    }
}
