using TMPro;
using UnityEngine;

public class PlayerNamePlate : MonoBehaviour, IPlayerView
{
    [SerializeField]
    private TextMeshProUGUI _userLevelText;
    [SerializeField]
    private TextMeshProUGUI _userNameText;
    [SerializeField]
    private PlayerLevelDisplay _levelDisplay;

    public void Initialize(PlayerUserData playerData)
    {
        _userLevelText.text = $"{playerData.GetLevel()}";
        //_userLevelText.text = LevelUtilities.GetColorFromLevel(userData.GetLevel());
        _userNameText.color = playerData.ChatData.GetNameColor();
        _userNameText.text = playerData.ChatData.GetDisplayName();
        _levelDisplay.Initialise(playerData);
    }
}
