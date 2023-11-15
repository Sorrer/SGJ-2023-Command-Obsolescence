using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class TestPathFindingBehaviour : MonoBehaviour
    {

        public int width = 15;
        public int height = 15;

        private Pathfinding.TempNode[,] cleanMaze = null;
        private Pathfinding.TempNode[,] maze = null;
        private Vector2Int startPosition;
        private Vector2Int endPosition;
        private List<Pathfinding.TempNode> path;
        private List<Pathfinding.TempNode> bestPath;

        private void Start()
        {
            GenerateMaze();
        }

        private void Update()
        {
            GenerateMaze();
        }

        [ContextMenu("Generate Maze")]
        public void GenerateMaze()
        {

            List<Vector2Int> validPosition = new List<Vector2Int>();
            Pathfinding.TempNode[,] nodes = new Pathfinding.TempNode[width, height];
            Pathfinding.TempNode[,] cleanNodes = new Pathfinding.TempNode[width, height];
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var val =  Random.value > 0.8 ? -1 : 1 + Random.Range(2,1000);
                    var node = new Pathfinding.TempNode();
                    node.x = x;
                    node.y = y;
                    node.weight = val;
                    nodes[x, y] = node;

                    if (node.weight >= 1)
                    {
                        validPosition.Add(new Vector2Int(x,y));
                    }
                    
                    node = new Pathfinding.TempNode();
                    node.x = x;
                    node.y = y;
                    node.weight = val >= 1 ? 1 : -1;
                    cleanNodes[x, y] = node;
                }
            }

            startPosition = validPosition[Random.Range(0, validPosition.Count)];
            endPosition = validPosition[Random.Range(0, validPosition.Count)];

            maze = nodes;
            cleanMaze = cleanNodes;

            var data = Pathfinding.GeneratePath(ref maze, startPosition, endPosition);

            path = data.path;
            
            data = Pathfinding.GeneratePath(ref cleanMaze, startPosition, endPosition);

            bestPath = data.path;

        }


        private void OnDrawGizmos()
        {

            float maxDistScore = 0;
            float maxWeightScore = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (float.IsPositiveInfinity(maze[x, y].distScore)) continue;
                    if (maze[x, y].distScore > maxDistScore) maxDistScore = maze[x, y].distScore;
                    if (maze[x, y].weight > maxWeightScore) maxWeightScore = maze[x, y].weight;

                    Gizmos.color = maze[x, y].visited ? Color.white : Color.black;
                    Gizmos.DrawWireCube(new Vector3(x, y, -10), new Vector3(1,1,1));
                }
            }

            // Draw maze
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var node = maze[x, y];
                        
                    if (node != null)
                    {


                        if (node.weight < 0)
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawCube(new Vector3(node.x, node.y, 0), new Vector3(1,1,1));
                        }
                        else
                        {
                            if (float.IsPositiveInfinity(node.distScore))
                            {
                                Gizmos.color = Color.yellow;
                                if (node.visited)
                                {
                                    Gizmos.DrawWireSphere(new Vector3(node.x, node.y, 0), 0.5f);
                                }
                                else
                                {
                                    Gizmos.DrawWireCube(new Vector3(node.x, node.y, 0), new Vector3(1,1,1));
                                }
                            }
                            else
                            {
                                Gizmos.color = Color.Lerp(Color.white, Color.red, node.distScore / maxDistScore);
                                Gizmos.DrawSphere(new Vector3(node.x, node.y, 0), 0.5f);
                                Gizmos.color = Color.Lerp(Color.white, Color.blue, node.weight / maxWeightScore);
                                Gizmos.DrawSphere(new Vector3(node.x, node.y, 2.0f), 0.2f);
                                
                            }
                        }
                        
                    }
                }
            }
            
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(new Vector3(startPosition.x, startPosition.y, 0), new Vector3(1,1,1));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(endPosition.x, endPosition.y, 0), new Vector3(1,1,1));

            if (path != null)
            {
                foreach (var node in path)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(new Vector3(node.x, node.y, 1.0f), Vector3.one * 0.1f);
                }
            }
            
            if (bestPath != null)
            {
                foreach (var node in bestPath)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(new Vector3(node.x, node.y, 2.0f), Vector3.one * 0.2f);
                }
            }
        }
        
    }
}