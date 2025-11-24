using DG.Tweening;
using System.Collections;
using Tasks;
using UnityEngine;

public class LoadInLevelTask : TaskBase
{
    [SerializeField]
    private Transform _levelSpawnParent;

    public override IEnumerator ExecuteInternal()
    {
        Level level = LevelManager.Instance.GetSelectedLevel();
        LevelController levelController = level.levelPrefab;

        TogglePlayerStates(false);

        LevelController levelControllerGO = Instantiate(level.levelPrefab, _levelSpawnParent);
        levelControllerGO.Initialise(level);

        yield return MoveAllPlayers(levelControllerGO.SpawnPosition.position);
        yield return levelControllerGO.ShowLevel();

        TogglePlayerStates(true);

        LevelManager.Instance.SetLevelController(levelControllerGO);
        Publish(GameLoopEventIds.ON_LEVEL_STARTED, levelControllerGO.Level);
        yield return null;
    }

    public void TogglePlayerStates(bool toggleState)
    {
        foreach (var player in ActivePlayerManager.Instance.GetAllPlayerObjects())
        {
            player.GetComponentInChildren<Collider>().enabled = toggleState;
            player.GetComponentInChildren<Rigidbody>().isKinematic = !toggleState;
        }
    }

    public IEnumerator MoveAllPlayers(Vector3 position)
    {
        foreach (var player in ActivePlayerManager.Instance.GetAllPlayerObjects())
        {
            player.transform.DOMove(position, 0.75f);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);
    }
}