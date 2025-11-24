using Core;
using Core.Events;
using System.Collections;
using UnityEngine;
using YourNamespace;
[RequireComponent(typeof(EnvironmentSpawnManager))]
public class EnvironmentController : BaseBehaviour
{
    private Environment _environment;
    private EnvironmentSpawnManager _environmentSpawnManager;
    private LoopPathController _loopPathController;

    public Environment Environment => _environment;
    public void Initialise(Environment environmentData)
    {
        _environment = environmentData;
        _environmentSpawnManager = GetComponent<EnvironmentSpawnManager>();
        _loopPathController = GetComponentInChildren<LoopPathController>(true);
    }

    public IEnumerator ShowEnvironment()
    {
        bool isFinished = false;

        _environmentSpawnManager.MoveAllElementsUp(isInstant: true);
        yield return new WaitForEndOfFrame();
        Debug.LogWarning("About to show the environment spawning in");
        _environmentSpawnManager.MoveAllElementsDown(isInstant: false, () => isFinished = true);
        yield return new WaitUntil(() => isFinished);
    }

    public IEnumerator HideEnvironment()
    {
        bool isFinished = false;
        _environmentSpawnManager.MoveAllElementsUp(isInstant: false, () => isFinished = true);
        yield return new WaitUntil(() => isFinished);
    }

    [Topic(GameLoopEventIds.ON_PLAYER_FINISHED)]
    public void OnPlayerFinished(object sender, PlayerController player)
    {
        PlayerMovementController playerMovementController = player.GetComponent<PlayerMovementController>();
        StartCoroutine(PlayerFinishedHandler(playerMovementController));
    }

    public IEnumerator PlayerFinishedHandler(PlayerMovementController playerMovementController)
    {
        playerMovementController.StartLoopedPathMovement(_loopPathController, 5f);
        yield return null;
    }
}