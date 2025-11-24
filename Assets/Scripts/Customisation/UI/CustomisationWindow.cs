using Audio.Core;
using Audio.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Extensions;

public class CustomisationWindow : NumberSelectionWindow<Customisation>
{
    [SerializeField] private TimedSelection _timer;
    [SerializeField] private RectTransform _commandPromptBar;

    private int _numberOfChoices = 3;

    protected override void OnNumberSelected(Customisation data, string displayName)
    {
        GameObject playerObject = ActivePlayerManager.Instance.GetPlayerInstance(displayName);
        PlayerCustomisationController customisationController = playerObject.GetComponent<PlayerCustomisationController>();
        customisationController.SetCustomisationOption(data);
        AudioManager.Instance.PlaySFX(AudioConstIds.CARD_SELECT);
    }
    public override void OnItemDisplayed(NumberSelector<Customisation> selector, int index)
    {
        base.OnItemDisplayed(selector, index);
        CustomisationDisplay customisationDisplay = selector as CustomisationDisplay;
        customisationDisplay.ResetPosition();
        StartCoroutine(customisationDisplay.ShowAnim(0.33f * index));
    }
    public void DisplayRandomCustomisations()
    {
        List<Customisation> options = CustomisationManager.Instance.allCustomisations.RandomMultipleUnique(_numberOfChoices);
        DisplaySelections(options);
    }

    public void SetSelectionChoices(int numberOfChoices) => _numberOfChoices = numberOfChoices;

    public IEnumerator OnOpen(float duration, bool isSkippable)
    {
        DisplayRandomCustomisations();
        yield return StartCoroutine(_timer.WaitTime(duration, isSkippable));
    }

    public void OnClose()
    {
        ClearSelections();
        gameObject.SetActive(false);
    }
}
