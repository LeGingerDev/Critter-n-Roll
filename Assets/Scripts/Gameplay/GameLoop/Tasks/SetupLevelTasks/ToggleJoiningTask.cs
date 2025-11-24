using System.Collections;
using System.Collections.Generic;
using Tasks;
using UnityEngine;

public class ToggleJoiningTask: TaskBase
{
    [SerializeField]
    private JoinCommandUI _joinCommandUI;
    [SerializeField]
    private TimedSelection _joinTimedSelection;
    [SerializeField]
    private float _joinTimer = 60;


    public override IEnumerator ExecuteInternal()
    {
        Initialise();
        // Wait until at least one player joins
        yield return new WaitUntil(() => ActivePlayerManager.Instance.GetActivePlayers().Count > 0);

        TriggerCustomisationPoll(_joinTimer - 1f);
        // Start join timer
        _joinTimedSelection.gameObject.SetActive(true);
        _joinTimedSelection.ToggleInfinite(false);
        Coroutine joinTimerCoroutine = StartCoroutine(_joinTimedSelection.WaitTime(_joinTimer, true));

        // Monitor player count while the timer is running
        while (true)
        {
            // If no players, restart task immediately
            if (ActivePlayerManager.Instance.GetActivePlayers().Count == 0)
            {
                // Clean up coroutine before restarting
                CancelCustomisationPoll();
                StopCoroutine(joinTimerCoroutine);
                RestartTask();
                yield break; // exit current coroutine execution to restart
            }

            // Check if the timer has completed
            if (!_joinTimedSelection.IsWaiting)
                break;

            yield return null; // Wait for next frame before checking again
        }
        HidePoll();
        yield return new WaitForEndOfFrame();
        Cleanup();
    }
    //TODO: Move to a manager for Poll Creation. It should be dynamic
    public void TriggerCustomisationPoll(float duration)
    {
        
        PollController pollController = FindFirstObjectByType<PollController>(FindObjectsInactive.Include);
        PollArguments pollArguments = new PollArguments("Want to change animal?", ParticipantTarget.PlayersOnly,duration);
        pollArguments.SetOptions(new List<Poll>
        {
            new Poll("yes", Color.red, () => CustomisationManager.Instance.SetCustomisationFlag(true)),
            new Poll("no", Color.blue, () => CustomisationManager.Instance.SetCustomisationFlag(false))
        });
        pollController.gameObject.SetActive(true);
        pollController.TriggerPoll(pollArguments);
    }

    public void CancelCustomisationPoll()
    {
        PollController pollController = FindFirstObjectByType<PollController>(FindObjectsInactive.Include);
        pollController.CancelPoll();
    }

    public void HidePoll()
    {
        PollController pollController = FindFirstObjectByType<PollController>(FindObjectsInactive.Include);
        pollController.ForceFinish();
        pollController.gameObject.SetActive(false);
    }

    public void Initialise()
    {
        _joinCommandUI.ToggleJoinVisual(true);
        _joinTimedSelection.gameObject.SetActive(true);

        // Ensure infinite waiting mode at start
        _joinTimedSelection.SetToFullVisuals(); // Reset timer to full
        _joinTimedSelection.ToggleInfinite(true);
    }

    public void Cleanup()
    {
        // Finalize task upon successful completion
        _joinTimedSelection.gameObject.SetActive(false);
        _joinCommandUI.ToggleJoinVisual(false);
    }
}
