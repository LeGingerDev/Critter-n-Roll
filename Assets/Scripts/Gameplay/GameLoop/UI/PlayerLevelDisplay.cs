using TMPro;
using UnityEngine;

public class PlayerLevelDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelText;

    public void Initialise(PlayerUserData playerData)
    {
        _levelText.text = playerData.GetLevel().ToString();
    }
}
