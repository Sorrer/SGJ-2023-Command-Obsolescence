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

                    tileInfo.weight = tile.tileComponent.GetWeight() + baseWeight;
                    
                    currentMap[x + (y *width)] = tileInfo;
                }
            }

            mapLoaded = true;
        }
        
        public void UpdateTile(Vector2Int tilePos, TileComponent tile)
        {
                // Whenever something gets placed update the tilemap
                // TODO: Update enemy paths that cross this tile position
                
                this.currentMap[tilePos.x + (tilePos.y * width)].weight = tile.GetWeight();
        }


        public Vector2Int GetRandomValidPosition()
        {
            return validMapLocations[Random.Range(0, validMapLocations.Count)];
        }
        
    }
}