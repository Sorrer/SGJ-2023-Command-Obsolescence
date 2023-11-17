using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
    public class SpawnEnemyBehaviour : MonoBehaviour
    {
        public EnemyMapReference mapReference;
        public Transform enemyContainer;

        public float spawnInterval = 1;
        
        [Serializable]
        public class EnemySpawningDetails
        {
            public EnemyTypes type;
            public int health;
            public GameObject enemyPrefab;
        }

        public List<EnemySpawningDetails> spawnList = new List<EnemySpawningDetails>();

        public enum EnemyTypes
        {
            BASIC
        }

        private void Start()
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }

        private IEnumerator SpawnEnemyCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                if (mapReference.mapLoaded)
                {
                    SpawnEnemy(mapReference.GetRandomValidPosition(), EnemyTypes.BASIC);
                }
            }
        }

        public void SpawnEnemy(Vector2Int gridPosition, EnemyTypes enemy)
        {
            // Hard coded enemy spawning

            EnemySpawningDetails foundEnemy = null;
            
            foreach(var dets in spawnList)
            {
                if (dets.type == enemy)
                {
                    foundEnemy = dets;
                    break;
                }
            }

            if (foundEnemy == null) return;

            var spawnPosition = LevelGenerator.Instance.GetWorldPosition(gridPosition);
            var spawnedEnemy = Instantiate(foundEnemy.enemyPrefab, enemyContainer);
            var enemyBehaviour = spawnedEnemy.GetComponent<EnemyBehaviour>();
                
            enemyBehaviour.SetPosition(spawnPosition);
            enemyBehaviour.SetHealth(foundEnemy.health);
            spawnedEnemy.GetComponent<EnemyPathProcesser>().GoTo(mapReference.goal, mapReference);
        }
        

    }
}