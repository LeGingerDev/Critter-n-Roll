using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantTargetDisplay : MonoBehaviour
{
    [SerializeField, FoldoutGroup("UI Elements")]
    private Image _baseImage;
    [SerializeField, FoldoutGroup("UI Elements")]
    private Image _borderImage;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI _participantText;


    [SerializeField, FoldoutGroup("Settings")]
    private Color[] _baseColours;
    [SerializeField, FoldoutGroup("Settings")]
    private Color[] _borderColours;

    private ParticipantTarget _target;

    public void Initialise(ParticipantTarget target)
    {
        _target = target;
        _baseImage.color = GetBaseColor();
        _borderImage.color = GetBorderColor();
        _participantText.text = GetParticipantText();
    }

    public Color GetBaseColor()
    {
        return _baseColours[(int)_target];
    }

    public Color GetBorderColor()
    {
        return _borderColours[(int)_target];
    }

    public string GetParticipantText()
    {
        switch (_target)
        {
            case ParticipantTarget.Everyone:
                return "Everyone!";
            case ParticipantTarget.ViewersOnly:
                return "Viewers Only!";
            case ParticipantTarget.PlayersOnly:
                return "Players Only!";
            default:
                return "Unknown Target";
        }
    }
}