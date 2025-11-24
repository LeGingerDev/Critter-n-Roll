using TMPro;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _playerNameText;
    [SerializeField]
    private TextMeshProUGUI _playerLevelText;
    [SerializeField]
    private PlayerLevelDisplay _playerLevelDisplay;
    private PlayerUserData _playerUserData;


    public PlayerUserData PlayerUserData => _playerUserData;

    public void Initialise(PlayerUserData playerUserData)
    {
        _playerUserData = playerUserData;
        _playerNameText.text = _playerUserData.ChatData.GetDisplayName();
        _playerLevelText.text = $"Lv.{_playerUserData.GetLevel()}";
        _playerLevelDisplay.Initialise(_playerUserData);
    }
}