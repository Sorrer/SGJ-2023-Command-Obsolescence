using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class TestSchedule : MonoBehaviour
    {
        public int width;
        public int height;
        public EnemyPathProcesser enemy;
        public EnemyPathScheduler scheduler;

        private void FixedUpdate()
        {
            for (int i = 0; i < 10; i++)
            {
                ScheduleMaze();
            }
        }

        public void ScheduleMaze()
        {

            List<Vector2Int> validPosition = new List<Vector2Int>();
            EnemyPathScheduler.MapInfo.TileInfo[] nodes = new EnemyPathScheduler.MapInfo.TileInfo[width * height];
            
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var val =  Random.value > 0.8 ? -1 : 1 + Random.Range(2,1000);
                    
                    var node = new EnemyPathScheduler.MapInfo.TileInfo();
                    node.x = x;
                    node.y = y;
                    node.weight = val;
                    nodes[x + y  * width] = node;

                    if (node.weight >= 1)
                    {
                        validPosition.Add(new Vector2Int(x,y));
                    }
                }
            }

            var startPosition = validPosition[Random.Range(0, validPosition.Count)];
            var endPosition = validPosition[Random.Range(0, validPosition.Count)];
            
            scheduler.Schedule(new EnemyPathScheduler.MapInfo()
            {
                map = nodes,
                width = width,
                height = height
            }, enemy, startPosition, endPosition);
        }

    }
}