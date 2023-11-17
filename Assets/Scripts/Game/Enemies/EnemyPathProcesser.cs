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
       
        private EnemyMapReference currentMap;

        public void GoTo(Vector2Int worldPosition, EnemyMapReference map)
        {
            if (!LevelGenerator.Instance.IsWithinWorld(worldPosition))
            {
                Debug.LogError("Got a position that is outside the world.. ?");
                return;
            }
            
            currentMap = map;
            
            EnemyPathScheduler.instance.Schedule(new EnemyPathScheduler.MapInfo()
            {
                height = map.height,
                width = map.width,
                map = map.currentMap,
            }, this, LevelGenerator.Instance.ClampGridPositionToBounds(LevelGenerator.Instance.GetGridPosition(target.transform.position)), worldPosition);
        }
        
        public void StartPath(List<Vector2Int> path)
        {
            currentIndex = 0;
            currentPath = path;
            pathing = true;
            target.SetWalkTarget(LevelGenerator.Instance.GetWorldPosition(currentPath[currentIndex]));
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
                        target.SetWalkTarget(LevelGenerator.Instance.GetWorldPosition(currentPath[currentIndex]));
                    }
                    else
                    {
                        pathing = false;
                        // Reached end of the path
                        if (currentPath[currentIndex - 1] == currentMap.goal)
                        {
                            // Reached goal.. doo something?
                            target.Kill();
                            // De-increment lives?
                        }
                    }
                } 
            }
            else
            {
                // TODO: Move this logic into the LevelGenerator system
                return;
                // int width = level.GetWidth();
                // int height = level.GetHeight();
                // EnemyPathScheduler.MapInfo.TileInfo[] info = new EnemyPathScheduler.MapInfo.TileInfo[width * height];
                //
                //
                // List<Vector2Int> validPosition = new List<Vector2Int>();
                // for (int x = 0; x < width; x++)
                // {
                //     for (int y = 0; y < height; y++)
                //     {
                //         var tileInfo = new EnemyPathScheduler.MapInfo.TileInfo();
                //
                //         tileInfo.x = x;
                //         tileInfo.y = y;
                //         
                //         tileInfo.weight = 1; // TODO: Get weight for tile
                //         validPosition.Add(new Vector2Int(x,y));
                //         info[x + y * width] = tileInfo;
                //     }
                // }
                //
                // var endPosition = validPosition[Random.Range(0, validPosition.Count)];
                //
                // EnemyPathScheduler.instance.Schedule(new EnemyPathScheduler.MapInfo()
                // {
                //     height = height,
                //     width = width,
                //     map = info
                // }, this, level.ClampGridPositionToBounds(level.GetGridPosition(target.transform.position)), endPosition);

            }
        }
    }
}