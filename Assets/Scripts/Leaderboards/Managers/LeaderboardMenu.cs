using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

// Generic base class for leaderboard menus
public abstract class LeaderboardMenu<T> : MonoBehaviour where T : class
{
    [SerializeField, FoldoutGroup("UI References")]
    private Transform _leaderboardPanel;
    [SerializeField, FoldoutGroup("UI References")]
    private CanvasGroup _leaderboardFadeGroup;

    [SerializeField, FoldoutGroup("Leaderboard Settings")]
    protected int _maxEntries = 10;
    [SerializeField, FoldoutGroup("Leaderboard Settings"), ReadOnly]
    protected int _currentEntries = 0;

    [SerializeField, FoldoutGroup("Leaderboard References")]
    protected LeaderboardDisplay<T> _leaderboardDisplay;
    [SerializeField, FoldoutGroup("Leaderboard References")]
    protected Transform _leaderboardEntriesParent;
    [SerializeField, FoldoutGroup("Leaderboard References")]
    protected List<LeaderboardDisplay<T>> _leaderboardEntries = new List<LeaderboardDisplay<T>>();

    protected virtual void Start()
    {
        CloseImmediate();
    }

    public virtual void OnOpen()
    {
        OnOpenEffects();
        SetupLeaderboard();
    }

    public virtual void OnClose()
    {
        OnCloseEffects();
    }

    public void SetupLeaderboard()
    {
        Cleanup();

        List<T> leaderboardData = GetLeaderboardEntries();

        int entriesToShow = Mathf.Min(leaderboardData.Count, _maxEntries);

        for (int i = 0; i < entriesToShow; i++)
        {
            if (leaderboardData[i] == null)
            {
                continue;
            }
            LeaderboardDisplay<T> entryObj = Instantiate(_leaderboardDisplay, _leaderboardEntriesParent);
            entryObj.InitializeEntry(leaderboardData[i], i + 1);
            _leaderboardEntries.Add(entryObj);
        }
    }

    public void Cleanup()
    {
        _leaderboardEntries.ForEach(entry => Destroy(entry.gameObject));
        _leaderboardEntries.Clear();
    }

    public void CloseImmediate()
    {
        _leaderboardFadeGroup.DOFade(0, 0).SetUpdate(true).OnComplete(() =>
        {
            _leaderboardFadeGroup.interactable = false;
            _leaderboardFadeGroup.blocksRaycasts = false;
        });
        _leaderboardPanel.transform.DOScale(0.8f, 0).SetUpdate(true).SetEase(Ease.InBack);
    }

    #region Abstract Methods

    public abstract List<T> GetLeaderboardEntries();


    #endregion Abstract Methods


    #region Virtual Methods

    // Virtual methods for concrete classes to override if needed
    protected virtual void OnOpenEffects()
    {
        _leaderboardFadeGroup.DOFade(1, 0.3f).From(0).SetUpdate(true).OnComplete(() =>
        {
            _leaderboardFadeGroup.interactable = true;
            _leaderboardFadeGroup.blocksRaycasts = true;
        });
        _leaderboardPanel.transform.DOScale(1, 0.3f).From(0.8f).SetUpdate(true).SetEase(Ease.OutBack);
    }
    protected virtual void OnCloseEffects()
    {
        _leaderboardFadeGroup.DOFade(0, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            _leaderboardFadeGroup.interactable = false;
            _leaderboardFadeGroup.blocksRaycasts = false;
        });
        _leaderboardPanel.transform.DOScale(0.8f, 0.3f).SetUpdate(true).SetEase(Ease.InBack);
    }

    protected virtual void OnOpenInitialise()
    {

    }

    protected virtual void OnCloseDeinitialise()
    {
    }

    protected virtual void OnEmptyLeaderboard() { }

    #endregion Virtual Methods
}