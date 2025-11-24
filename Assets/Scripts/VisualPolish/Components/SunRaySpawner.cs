using LGD.Utilities;
using System.Collections;
using UnityEngine;
namespace LGD.VisualPolish.Components
{
    public class SunRaySpawner : MonoBehaviour
    {
        public GameObject _sunRay;
        public Collider _spawnArea;
        public IEnumerator Start()
        {
            yield return new WaitForSeconds(3f); // Initial delay before starting the spawning
            while (true)
            {
                CreateSunray();
                yield return new WaitForSeconds(Random.Range(3f, 10f));
            }
        }

        public void CreateSunray()
        {
            if (_sunRay == null || _spawnArea == null)
            {
                Debug.LogWarning("SunRay or SpawnArea is not set.");
                return;
            }
            Vector3 spawnPosition = _spawnArea.GetRandomPointInside();
            GameObject sunRayInstance = Instantiate(_sunRay, spawnPosition, Quaternion.identity);
            sunRayInstance.transform.SetParent(transform); // Optional: Set parent to this spawner
            Destroy(sunRayInstance, 15f);
        }
    }
}