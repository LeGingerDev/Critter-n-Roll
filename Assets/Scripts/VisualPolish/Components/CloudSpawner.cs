using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LGD.VisualPolish.Components
{

    public class CloudSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform _startPos;
        [SerializeField]
        private Transform _endPos;

        [SerializeField]
        private List<Cloud> _clouds = new List<Cloud>();
        [SerializeField]
        private bool _inReverse = false;
        private void Start()
        {
            StartCoroutine(SpawnClouds());
        }

        public IEnumerator SpawnClouds()
        {
            while (true)
            {
                if(Random.value < 0.6f) 
                {
                    yield return new WaitForSeconds(Random.Range(1f, 3f));
                    continue;
                }

                Cloud cloud = Instantiate(GetRandomCloud(), GetSpawnPosition(), Quaternion.identity);
                cloud.Initialise(_inReverse);
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        public Cloud GetRandomCloud() => _clouds[Random.Range(0, _clouds.Count)];

        public Vector3 GetSpawnPosition()
        {
            return Vector3.Lerp(_startPos.position, _endPos.position, Random.Range(0f, 1f));
        }
    }

}