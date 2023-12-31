﻿using System;
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
        public int weightVariationRange = 5;

        public void GoTo(Vector2Int worldPosition, EnemyMapReference map, bool tryUseNextPath = false)
        {
            if (!LevelGenerator.Instance.IsWithinWorld(worldPosition))
            {
                Debug.LogError("Got a position that is outside the world.. ?");
                return;
            }

            currentMap = map;

            var startPosition =
                LevelGenerator.Instance.ClampGridPositionToBounds(
                    LevelGenerator.Instance.GetGridPosition(target.transform.position));


            if (tryUseNextPath && currentIndex < currentPath.Count)
            {
                startPosition = currentPath[currentIndex];
            }
            

            EnemyPathScheduler.instance.Schedule(new EnemyPathScheduler.MapInfo()
                {
                    height = map.height,
                    width = map.width,
                    map = map.currentMap,
                }, this,
                
                    startPosition, worldPosition, weightVariationRange);
        }

        public void StartPath(List<Vector2Int> path)
        {
            if (path.Count == 0) return;
            
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
                        // If next target needs to be attacked then we will walk up to it and attack it

                        if (isCurrentTargetAttackable())
                        {
                            Debug.LogError("GONNA ATTACK");
                            var attackable = GetCurrentTargetAttackable();
                            var attackableLocation = attackable.GetWorldPosition();
                            var currentPosition = LevelGenerator.Instance.GetWorldPosition(target.GetGridPosition());

                            var diff =  currentPosition - attackableLocation;
                            
                            diff.Normalize();

                            diff *= 0.5f;
                            
                            target.SetAttackTarget(attackable, attackableLocation + diff, LevelGenerator.Instance.GetWorldPosition(LevelGenerator.Instance.GetGridPosition(attackableLocation)));
                        }
                        else
                        {
                            target.SetWalkTarget(LevelGenerator.Instance.GetWorldPosition(currentPath[currentIndex]));
                        }

                    }
                    else
                    {
                        pathing = false;
                        // Reached end of the path
                        if (currentPath[currentIndex - 1] == currentMap.goal)
                        {
                            DeathScreen.Instance.TakeDamage();
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

        public bool isCurrentTargetAttackable()
        {
            return GetCurrentTargetAttackable() != null;
        }

        public IAttackable GetCurrentTargetAttackable()
        {
            var tile = LevelGenerator.Instance.GetTileComponent(currentPath[currentIndex]);
                
            if (tile)
            {
                return tile.GetAttackable(); 
            }

            return null;
        }

        private void OnDrawGizmosSelected()
        {
            if (LevelGenerator.Instance == null) return;
            foreach (var path in currentPath)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(LevelGenerator.Instance.GetWorldPosition(path), 0.2f);
            }
        }
    }
}