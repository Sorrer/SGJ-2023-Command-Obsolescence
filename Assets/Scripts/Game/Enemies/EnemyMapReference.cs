using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyMapReference : MonoBehaviour
    {
        public Vector2Int goal;

        public EnemyPathScheduler.MapInfo.TileInfo[] currentMap = null;
        public int width;
        public int height;
        public List<Vector2Int> validMapLocations;

        // Base weight should be set to a higher number to properly convince 
        public int baseWeight = 1000;

        public bool mapLoaded = false;
        
        public void LoadMap(Tile[,] gameTilemap)
        {
            width = gameTilemap.GetLength(0);
            height = gameTilemap.GetLength(1);

            currentMap = new EnemyPathScheduler.MapInfo.TileInfo[width * height];
            validMapLocations = new List<Vector2Int>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tile = gameTilemap[x, y];

                    var tileInfo = new EnemyPathScheduler.MapInfo.TileInfo();
                    
                    tileInfo.x = x;
                    tileInfo.y = y;
                    
                    if (tile == null || tile.tileComponent == null)
                    {

                        tileInfo.weight = -1; // unwalkable
                        currentMap[x + (y *width)] = tileInfo;
                        continue;
                    }   
                    
                    validMapLocations.Add(new Vector2Int(x,y));
                    
                    var tileWeight = tile.tileComponent.GetWeight();

                    if (tile.tileComponent.IsTraversal())
                    {
                        tileInfo.weight = tileWeight + baseWeight;
                    }
                    else
                    {
                        tileInfo.weight = -1;
                    }
                    
                    currentMap[x + (y *width)] = tileInfo;

                }
            }

            mapLoaded = true;
        }
        
        public void UpdateTile(Vector2Int tilePos, TileComponent tile)
        {
            // Whenever something gets placed update the tilemap
            // TODO: Update enemy paths that cross this tile position
            int previousWeight = this.currentMap[tilePos.x + (tilePos.y * width)].weight;

            if (tile.IsTraversal())
            {
                this.currentMap[tilePos.x + (tilePos.y * width)].weight = tile.GetWeight() + baseWeight;
            }
            else
            {
                this.currentMap[tilePos.x + (tilePos.y * width)].weight = -1;
            }

            // Don't need to update if nothing changed
            if (previousWeight == this.currentMap[tilePos.x + (tilePos.y * width)].weight) return;

            foreach (var enemy in SpawnEnemyBehaviour.Instance.enemies)
            {
                enemy.Value.processer.GoTo(goal, this, true);
            }
        }



        public Vector2Int GetRandomValidPosition()
        {
            return validMapLocations[Random.Range(0, validMapLocations.Count)];
        }


        private void OnDrawGizmosSelected()
        {
            if (LevelGenerator.Instance == null) return;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    if (currentMap[x + (y * width)].weight == -1)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(LevelGenerator.Instance.GetWorldPosition(new Vector2Int(x, y)), 0.5f);
                    }
                }
            }
        }


        public List<Vector2Int> goals = new List<Vector2Int>();
        public Vector2Int GetGoalPosition(int i)
        {
            if (goals.Count == 0) return new Vector2Int(0, 0);

            if (i < 0 || i >= goals.Count)
            {
                return goals[0];
            }
            else
            {
                return goals[i];
            }
        }

        public void AddGoalPost(Vector2Int position)
        {
            goals.Add(position);
        }

        public void RemoveGoalPost(Vector2Int position)
        {
            int i = 0;
            for (i = 0; i < goals.Count; i++)
            {
                if (goals[i] == position)
                {
                    break;
                }
            }

            goals.RemoveAt(i);
        }
    }
}