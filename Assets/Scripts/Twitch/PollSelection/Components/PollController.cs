using ScoredProductions.StreamLinked.IRC;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PollController : TwitchBaseListener
{
    [SerializeField]
    private TextMeshProUGUI _pollQuestionText;
    [SerializeField]
    private PollItem _pollItemPrefab;
    [SerializeField]
    private Transform _pollItemParent;
    [SerializeField]
    private ParticipantTargetDisplay _participantTargetDisplay;
    [SerializeField]
    private TimedSelection _timedSelector;
    [SerializeField]
    private List<Color> _pollColors = new List<Color>();

    private List<PollItem> _pollItems = new List<PollItem>();
    private bool _isActive;

    private HashSet<string> _pollIds = new HashSet<string>();
    private PollArguments _pollArguments;

    private List<Color> _usedColours = new List<Color>();

    [Button]
    public void TestTrigger(ParticipantTarget target)
    {
        PollArguments pollArguments = new PollArguments("Test Poll", target, 100f);
        pollArguments.SetOptions(new List<Poll>
        {
            new Poll("yes", Color.red, () => Debug.Log("Option 1 selected")),
            new Poll("no", Color.blue, () => Debug.Log("Option 2 selected")),
            new Poll("maybe", Color.green, () => Debug.Log("Option 3 selected"))
        });
        TriggerPoll(pollArguments);
    }

    public void TriggerPoll(PollArguments pollArguments)
    {
        StartCoroutine(Initialise(pollArguments));
    }

    public void CancelPoll()
    {
        if (_isActive)
        {
            StopAllCoroutines();
            Cleanup();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Initialise(PollArguments pollArguments)
    {
        Cleanup();
        CreatePoll(pollArguments);
        _isActive = true;
        yield return _timedSelector.WaitTime(pollArguments.TotalTime);
        _isActive = false;

        PollItem finalPollItem = GetMostVoted();
        if(IsATie())
        {
            finalPollItem = GetRandomPollItem();
        }

        finalPollItem.Poll.OnPollSelected?.Invoke();
    }

    public void CreatePoll(PollArguments pollArguments)
    {
        _pollArguments = pollArguments;
        _pollQuestionText.text = pollArguments.Question;
        _participantTargetDisplay.Initialise(pollArguments.Target);

        _pollItems.Clear();
        _pollIds.Clear();
        foreach (var option in pollArguments.Options)
        {
            PollItem pollItem = Instantiate(_pollItemPrefab, _pollItemParent);
            option.pollColor = GetRandomColor();

            pollItem.Initialise(option);
            _pollItems.Add(pollItem);
        }
    }

    public void ForceFinish()
    {
        _timedSelector.ForceComplete();
    }

    public void Cleanup()
    {
        _isActive = false;
        _pollItems.ForEach(p => Destroy(p.gameObject));
        _pollItems.Clear();
        _pollIds.Clear();
        _pollQuestionText.text = string.Empty;
        _pollColors.AddRange(_usedColours);
        _usedColours.Clear();
    }

    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        if (!_isActive)
            return;

        if (commandKey != "o")
            return;

        if(!CanVote( msg))
            return;

        if (_pollIds.Contains(sender))
            return;

        string commandText = (string)args;

        if (!ContainsCommand(commandText))
            return;

        HandleVote(commandText, sender);
    }

    public void HandleVote(string command, string senderId)
    {
        _pollIds.Add(senderId);

        PollItem pollItem = GetPollItemByCommand(command);
        pollItem.AddVote(_pollIds.Count);

        _pollItems.ForEach(p => p.UpdateVisuals(_pollIds.Count));

    }

    public bool ContainsCommand(string commandText)
    {
        return _pollItems.Any(i => i.Poll.commandText == commandText);
    }

    public PollItem GetPollItemByCommand(string commandText)
    {
        if (string.IsNullOrEmpty(commandText))
            return null;
        PollItem pollItem = _pollItems.FirstOrDefault(i => i.Poll.commandText == commandText);

        if (pollItem == null)
        {
            Debug.LogError($"Poll item with command '{commandText}' not found.");
            return null;
        }
        return pollItem;
    }
    public PollItem GetRandomPollItem() => _pollItems[UnityEngine.Random.Range(0, _pollItems.Count)];
    public PollItem GetMostVoted() => _pollItems.OrderByDescending(i => i.NumberOfVotes).FirstOrDefault();
    public bool IsATie()
    {
        float maxVotes = _pollItems.Max(i => i.NumberOfVotes);
        return _pollItems.Count(i => i.NumberOfVotes == maxVotes) > 1;
    }

    public Color GetRandomColor()
    {
        Color color = _pollColors[UnityEngine.Random.Range(0, _pollColors.Count)];
        _pollColors.Remove(color);
        _usedColours.Add(color);
        return color;
    }

    public bool CanVote(TwitchMessage msg)
    {
        switch (_pollArguments.Target)
        {
            case ParticipantTarget.Everyone:
                return true;
            case ParticipantTarget.PlayersOnly:
                return ActivePlayerManager.Instance.IsAPlayer(msg);
            case ParticipantTarget.ViewersOnly:
                return !ActivePlayerManager.Instance.IsAPlayer(msg);
            default:
                Debug.LogError($"Unknown participant target: {_pollArguments.Target}.");
                return false;
        }
    }
}

[Serializable]
public class PollArguments
{
    public string Question;
    public List<Poll> Options = new List<Poll>();
    public float TotalTime;
    public ParticipantTarget Target;

    public PollArguments(string question, ParticipantTarget target, float totalTime)
    {
        Question = question;
        Target = target;
        TotalTime = totalTime;
    }

    public void SetOptions(List<Poll> options)
    {
        Options = options;
    }
}
[Serializable]
public class Poll
{
    public string commandText;
    public Action OnPollSelected;
    public Color pollColor;
    public Poll(string commandText, Color color, Action onPollSelected)
    {
        this.commandText = commandText;
        this.OnPollSelected = onPollSelected;
        this.pollColor = color;
    }
}

public enum ParticipantTarget
{
    PlayersOnly,
    ViewersOnly,
    Everyone
}
