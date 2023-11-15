using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyPathProcesser : MonoBehaviour
    {
        public EnemyBehaviour target;

        public int currentIndex;
        public List<Vector2Int> currentPath;
        public bool pathing = false;
        public LevelGenerator level;
        public void StartPath(List<Vector2Int> path)
        {
            currentIndex = 0;
            currentPath = path;
            pathing = true;
            target.SetWalkTarget(currentPath[0]);
        }


        private void Update()
        {
            // TODO: When the map changes, invalidate the path and schedule a new path with the new map
            if (pathing)
            {
                if (target.reached)
                {
                    currentIndex++;

                    if (currentIndex < currentPath.Count)
                    {
                        target.SetWalkTarget(currentPath[currentIndex]);
                    }
                    else
                    {
                        pathing = false;
                    }
                } 
            }
            else
            {
                // TODO: Move this logic into the LevelGenerator system
                
                int width = level.GetWidth();
                int height = level.GetHeight();
                EnemyPathScheduler.MapInfo.TileInfo[] info = new EnemyPathScheduler.MapInfo.TileInfo[width * height];


                List<Vector2Int> validPosition = new List<Vector2Int>();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var tileInfo = new EnemyPathScheduler.MapInfo.TileInfo();

                        tileInfo.x = x;
                        tileInfo.y = y;
                        
                        tileInfo.weight = 1; // TODO: Get weight for tile
                        validPosition.Add(new Vector2Int(x,y));
                        info[x + y * width] = tileInfo;
                    }
                }
                
                var startPosition = validPosition[Random.Range(0, validPosition.Count)];
                var endPosition = validPosition[Random.Range(0, validPosition.Count)];

                EnemyPathScheduler.instance.Schedule(new EnemyPathScheduler.MapInfo()
                {
                    height = height,
                    width = width,
                    map = info
                }, this, startPosition, endPosition);

                target.transform.position = new Vector3(startPosition.x, startPosition.y);
            }
        }
    }
}