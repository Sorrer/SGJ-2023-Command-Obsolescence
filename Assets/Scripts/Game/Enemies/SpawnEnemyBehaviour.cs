using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
    public class SpawnEnemyBehaviour : MonoBehaviour
    {
        public EnemyMapReference mapReference;
        public Transform enemyContainer;
        
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

        private void FixedUpdate()
        {
            if (mapReference.mapLoaded)
            {
                for (int i = 0; i < 5; i++)
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