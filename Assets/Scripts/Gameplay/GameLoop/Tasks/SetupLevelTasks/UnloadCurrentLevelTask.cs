using Sirenix.Utilities;
using System.Collections;
using Tasks;
using UnityEngine;

public class UnloadCurrentLevelTask : TaskBase
{
    [SerializeField]
    private Transform _environmentParent;

    public override IEnumerator ExecuteInternal()
    {
        yield return UnloadCurrentLevel();
        yield return UnloadCurrentEnvironment();
        yield return LoadInNewEnvironment();
    }

    public IEnumerator UnloadCurrentLevel()
    {
        if (LevelManager.Instance.GetPreviousLevel() == null || LevelManager.Instance.CurrentlySpawnedController == null)
            yield break;

        LevelController levelController = LevelManager.Instance.CurrentlySpawnedController;
        yield return levelController.HideLevel();
        Destroy(levelController.gameObject);
        //TODO: 
        FindObjectsByType<EndGate>(FindObjectsSortMode.None).ForEach(e => e.TriggerBlock());
    }

    public IEnumerator UnloadCurrentEnvironment()
    {
        if(LevelManager.Instance.GetPreviousLevel() == null || LevelManager.Instance.CurrentlySpawnedEnvironmentController == null)
            yield break;

        if (LevelManager.Instance.AreEnvironmentsTheSame())
            yield break;

        EnvironmentController environmentController = LevelManager.Instance.CurrentlySpawnedEnvironmentController;
        yield return environmentController.HideEnvironment();
        //environmentController.gameObject.SetActive(false);
        Destroy(environmentController.gameObject);
    }

    public IEnumerator LoadInNewEnvironment()
    {
        if(LevelManager.Instance.AreEnvironmentsTheSame())
            yield break;

        Environment environment = LevelManager.Instance.GetSelectedLevel().environment;
        EnvironmentController environmentController = environment.environmentPrefab;
        EnvironmentController environmentControllerGO = Instantiate(environmentController, _environmentParent);
        environmentControllerGO.Initialise(environment);
        yield return environmentControllerGO.ShowEnvironment();
        LevelManager.Instance.SetEnvironmentController(environmentControllerGO);
    }    
}
