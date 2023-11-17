using System.Collections;
using UnityEngine;

namespace Game.Enemies
{
    public class SpawnEnemyEntity : MonoBehaviour
    {

        public float SpawnInterval = 1;
        public SpawnEnemyBehaviour.EnemyTypes types = SpawnEnemyBehaviour.EnemyTypes.BASIC;
        
        private void Start()
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }

        private IEnumerator SpawnEnemyCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpawnInterval);
                if (SpawnEnemyBehaviour.Instance.mapReference.mapLoaded)
                {
                    SpawnEnemyBehaviour.Instance.SpawnEnemy(LevelGenerator.Instance.GetGridPosition(this.transform.position), types);
                }
            }
        }

    }
}