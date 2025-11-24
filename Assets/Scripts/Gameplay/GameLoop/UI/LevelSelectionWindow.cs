using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Utilities.Extensions;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Audio.Core;
using Audio.Managers;

public class LevelSelectionWindow : NumberSelectionWindow<Level>
{
    [SerializeField] private List<Level> _allLevels;
    [SerializeField] private int _numberOfChoices = 3;
    [SerializeField] private TimedSelection _timer;
    [SerializeField] private RectTransform _commandPromptBar;
    protected override void OnNumberSelected(Level level, string username)
    {
        AudioManager.Instance.PlaySFX(AudioConstIds.CARD_SELECT);
    }

    [Button]
    public void DisplayRandomLevels()
    {
        _allLevels = LevelManager.Instance.Levels;
        List<Level> chosen = _allLevels.RandomMultipleUnique(_numberOfChoices).ToList();
        DisplaySelections(chosen);
    }

    public override void OnItemDisplayed(NumberSelector<Level> selector, int index)
    {
        base.OnItemDisplayed(selector, index);
        LevelSelector levelSelector = selector as LevelSelector;
        levelSelector.ResetPosition();
        StartCoroutine(levelSelector.ShowAnim(0.33f * index));
    }
   
    public override void OnHighestSelectorUpdated(NumberSelector<Level> highestSelector)
    {
        base.OnHighestSelectorUpdated(highestSelector);
        highestSelector.transform.DOScale(Vector3.one * 1.1f, 0.2f);
        List<NumberSelector<Level>> notHighest = _selectors.Where(s => s != highestSelector).ToList();
        notHighest.ForEach(s => s.transform.DOScale(Vector3.one, 0.2f).SetDelay(0.1f));
    }

    [Button]
    public void DEBUG_TriggerOpen()
    {
        StartCoroutine(OnOpen(5));
    }

    public IEnumerator OnOpen(float duration, bool isSkippable = true)
    {
        DisplayRandomLevels();
        _commandPromptBar.transform.DOMoveY(40f, 0.4f).SetEase(Ease.OutBack).From(-400f);
        yield return StartCoroutine(_timer.WaitTime(duration, isSkippable));
    }

    public void OnClose()
    {
        ClearSelections();
        gameObject.SetActive(false);
    }
}
