using System.Collections.Generic;
using System.Linq;
using ScoredProductions.StreamLinked.IRC;
using UnityEngine;
using Utilities.Extensions;

public abstract class NumberSelectionWindow<T> : TwitchBaseListener
{
    [SerializeField] private NumberSelector<T> _selectorPrefab;
    [SerializeField] private Transform _selectorParent;

    protected readonly List<NumberSelector<T>> _selectors = new List<NumberSelector<T>>();
    private NumberSelector<T> _currentHighestSelection;
    protected abstract void OnNumberSelected(T data, string username);

    public void ClearSelections()
    {
        foreach (var s in _selectors) Destroy(s.gameObject);
        _selectors.Clear();
    }

    public void DisplaySelections(IEnumerable<T> items)
    {
        ClearSelections();
        int i = 1;
        foreach (var item in items)
        {
            var sel = Instantiate(_selectorPrefab, _selectorParent);
            sel.Initialise(item, i, OnNumberSelected);
            _selectors.Add(sel);
            i++;
            OnItemDisplayed(sel, i - 1);
        }
    }

    public virtual void OnItemDisplayed(NumberSelector<T> selector, int index)
    {

    }

    public bool AreAllSelectionsEqual()
    {
        if (_selectors.Count == 0) return false;
        int firstCount = _selectors[0].GetNumberOfSelected();
        return _selectors.All(s => s.GetNumberOfSelected() == firstCount);
    }

    public T GetHighestSelectionData()
    {
        if (_selectors.Count == 0)
            return default;

        int maxVotes = _selectors.Max(s => s.GetNumberOfSelected());
        if (maxVotes == 0)
            return _selectors.Random().GetData();

        // All selectors that share the top vote count
        var tied = _selectors.Where(s => s.GetNumberOfSelected() == maxVotes).ToList();
        // Return the first (or random) among them:
        return tied[0].GetData();
    }

    public NumberSelector<T> GetHighestSelection()
    {
        if (_selectors.Count == 0)
            return null;
        int maxVotes = _selectors.Max(s => s.GetNumberOfSelected());
        if (maxVotes == 0)
            return _selectors.Random();
        // All selectors that share the top vote count
        var tied = _selectors.Where(s => s.GetNumberOfSelected() == maxVotes).ToList();
        // Return the first (or random) among them:
        return tied[0];
    }

    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        if (commandKey != "v" || !(args is int num)) return;
        HandleNumberSelect(num, msg.GetDisplayName());
    }

    private void HandleNumberSelect(int numberSelected, string displayName)
    {
        var current = _selectors.FirstOrDefault(s => s.ContainsPlayer(displayName));
        if (current == null)
        {
            var chosen = _selectors.FirstOrDefault(s => s.IsSelectionNumber(numberSelected));
            if (chosen == null) return;
            chosen.AddPlayer(displayName);
            UpdateHighestSelected();
            return;
        }

        if (current.IsSelectionNumber(numberSelected)) return;

        current.RemovePlayer(displayName);
        var next = _selectors.FirstOrDefault(s => s.IsSelectionNumber(numberSelected));
        if (next == null) return;
        next.AddPlayer(displayName);

        UpdateHighestSelected();
    }

    public void UpdateHighestSelected()
    {
        if (AreAllSelectionsEqual())
        {
            _currentHighestSelection = null;
            return;
        }
        if (_currentHighestSelection == GetHighestSelection())
            return;
        OnHighestSelectorUpdated(GetHighestSelection());
    }

    public virtual void OnHighestSelectorUpdated(NumberSelector<T> selector)
    {
        _currentHighestSelection = selector;
    }

    //Add in a method for checking who can vote.
}