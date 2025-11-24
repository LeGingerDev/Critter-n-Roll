using Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PollItem : BaseBehaviour
{
    [SerializeField]
    private int _numberOfVotes = 0;

    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI _commandText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI _numberOfSelected;
    [SerializeField, FoldoutGroup("UI Elements")]
    private Image _pollFillImage;

    private Poll _poll;
    public Poll Poll => _poll;
    public int NumberOfVotes => _numberOfVotes;  
    public void Initialise(Poll poll)
    {
        _poll = poll;
        _commandText.text = $"!o {poll.commandText}";
        _pollFillImage.color = poll.pollColor;

        UpdateVisuals(0);
    }

    public void AddVote(int totalVotesCasted)
    {
        _numberOfVotes++;
    }

    public void UpdateVisuals(int totalVotesCasted)
    {
        _numberOfSelected.text = $"{GetPercentageOfVotes(totalVotesCasted):F2}% ({_numberOfVotes})";
        _pollFillImage.fillAmount = GetPercentageOfVotes(totalVotesCasted) / 100f;
    }

    public float GetPercentageOfVotes(int totalVotesCasted)
    {
        if (totalVotesCasted <= 0)
            return 0f;
        return (float)_numberOfVotes / totalVotesCasted * 100f;
    }

}
