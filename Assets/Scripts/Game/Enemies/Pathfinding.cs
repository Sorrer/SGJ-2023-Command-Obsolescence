using System;
using System.Collections.Generic;
using System.Linq;
using C5;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Enemies
{
    public class Pathfinding : MonoBehaviour
    {
        public List<EnemyBehaviour> enemyBehaviours = new List<EnemyBehaviour>();
        
        // Nodes at X,Y

        public struct PathInfo
        {
            public List<TempNode> path;
            public float totalPathWeight;
        }
        
        public class TempNode
        {
            public int x;
            public int y;
            public float weight;
            public float distScore;
            public float fScore;
            public bool visited = false;
            
            // Integer cardinal directions
            // 1 2 3
            // 4 5 6
            // 7 8 9
            public TempNode nextNode = null;
            public IPriorityQueueHandle<TempNode> handler;

            public Vector2Int GetPosition()
            {
                return new Vector2Int(x, y);
            }
            
            public override bool Equals(object obj)
            {
                // If obj is not TempNode node == null
                TempNode node = obj as TempNode;

                if (node == null) return false;

                // Only check positions, no layering
                return node.x == this.x && node.y == this.y;
            }

            public override int GetHashCode()
            {
                return $"{x},{y}".GetHashCode();
            }
        }

        public class TempNodeComparer : IComparer<TempNode>
        {
            public int Compare(TempNode x, TempNode y)
            {
                
                var cal = x.fScore - y.fScore;

                if (cal < 0) return -1;
                if (cal > 0) return 1;
                return 0;
            }
        }
        
        public static PathInfo GeneratePath(ref TempNode[,] world, Vector2Int startingPosition, Vector2Int finishPosition)
        {

            // Using queue for breadth-first search, depth search will result in a quicker execution time, but the first answer being correct.
            IntervalHeap<TempNode> unvisitedNodes = new IntervalHeap<TempNode>(new TempNodeComparer());
            int xWidth = world.GetLength(0);
            int yHeight = world.GetLength(1);

            for (int x = 0; x < xWidth; x++)
            {
                for (int y = 0; y < yHeight; y++)
                {
                    if (!CanTraverse(world[x, y])) continue;
                    world[x, y].distScore = float.PositiveInfinity;
                    world[x, y].fScore = float.PositiveInfinity;
                    world[x, y].nextNode = null;

                    if (x == startingPosition.x && y == startingPosition.y)
                    {
                        world[x, y].distScore = 0;
                        world[x, y].fScore = 0;
                    }
                }
            }
            //Debug.Log(xWidth + " " + yHeight + " " + startingPosition);
            unvisitedNodes.Add(ref world[startingPosition.x, startingPosition.y].handler, world[startingPosition.x, startingPosition.y]);

            bool found = false;
            
            while (unvisitedNodes.Count > 0)
            {
                //Debug.Log("DOING AN ITERATION");
                // Traverse the shortest constant path
                var node = unvisitedNodes.DeleteMin();
                //Debug.Log("Unvisited count: " + unvisitedNodes.Count);
                node.visited = true;

                
                if (node.x == finishPosition.x && node.y == finishPosition.y)
                {
                    //Debug.Log("Found solution");
                    // Solution found
                    found = true;
                    break;
                }

                // Node the most cleanest looking code
                // Mostly looks this way because the actual nodes can be hard coded in position, otherwise it would be a neighbor list..

                // Integer cardinal directions
                // 1 2 3
                // 4 5 6
                // 7 8 9
                //ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x - 1, node.y + 1, finishPosition, 3);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x, node.y + 1, finishPosition);
                //ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x + 1, node.y + 1, finishPosition, 3);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x - 1, node.y, finishPosition,3);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x + 1, node.y, finishPosition,3);
                //ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x - 1, node.y - 1, finishPosition, 3);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x , node.y - 1, finishPosition);
                //ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x + 1, node.y - 1, finishPosition, 3);


            }

            if (!found)
            {
                return new PathInfo()
                {
                    path = null,
                    totalPathWeight = -1
                };
            }
            
            // Generate best path by getting the finishPosition
            List<TempNode> bestPath = new List<TempNode>();
            float totalPathWeight = 0;
            TempNode nextNode = world[finishPosition.x, finishPosition.y];
            while (nextNode != null)
            {
                bestPath.Insert(0, nextNode);
                totalPathWeight += nextNode.weight;
                nextNode = nextNode.nextNode;
            }

            PathInfo info = new PathInfo();

            info.path = bestPath;
            info.totalPathWeight = totalPathWeight;

            return info;
        }


        
        private static void ProcessPathNode(ref IntervalHeap<TempNode> unvisitedNodes, ref TempNode[,] world, int xWidth, int yHeight, int curX, int curY, int neighborX, int neighborY, Vector2Int finishPoint, int addedWeight = 0)
        {
            // Out of bounds
            if (neighborX < 0 || neighborX >= xWidth || neighborY < 0 || neighborY >= yHeight) return;

            // Already visited we can ignore
            

            if (!CanTraverse(world[neighborX, neighborY])) return;

            if (world[neighborX, neighborY].visited) return;

            if (world[neighborX, neighborY].handler == null)
            {
                unvisitedNodes.Add(ref world[neighborX, neighborY].handler, world[neighborX, neighborY]);
            }
            
            var distScore = world[curX, curY].distScore + GetWeight(world[neighborX, neighborY]) + addedWeight;
            //Debug.Log($"{world[curX, curY].distScore} - {world[neighborX, neighborY].distScore} ({curX},{curY}) ({neighborX},{neighborY}) {distScore}");
            if (distScore < world[neighborX, neighborY].distScore)
            {
                // Resort this item, since it has a new weight now (2(log n), but this will happen o(b) b=best path length)
                world[neighborX, neighborY].distScore = distScore;
                world[neighborX, neighborY].nextNode = world[curX, curY];
                world[neighborX, neighborY].fScore =
                    Mathf.Pow(finishPoint.x - neighborX, 2) + Mathf.Pow(finishPoint.y - neighborY, 2)  + distScore;
                
                if(world[neighborX,neighborY].handler != null) unvisitedNodes.Delete(world[neighborX, neighborY].handler);
                unvisitedNodes.Add(ref world[neighborX, neighborY].handler, world[neighborX, neighborY]);

            }
            
        }

        

        public static bool CanTraverse(TempNode node)
        {
            return node.weight >= 0;
        }

        public static float GetWeight(TempNode node)
        {
            // TODO: Describe weight best on how hard it is to kill the object + total spaces on the map (so it chooses to move first)
            // TODO: If we want to include hotspots as goals to destroy, we must determine that either the node itself has the lowest value in the system, so it will bias its movement towards it. Or the path planner should get the paths to the hotspots, and check if they are within bias range to focus and attack.
            // TODO: Add deviance to the paths, so enemies choose different paths to take to the end, so that they don't converge
                // Deviance should not be added by weight, but instead the best path should bias towards different length of paths (shorter, mid-range, longer paths)
                    // So some enemies would want to take the longer path, while some want to take the shorter path
                // This will have to be scaled and check against the if statement
            // TODO: In order to implement proper A Star (best first search) there would need to be the addition of heuristics that is min'd against. Aka the min heap instead of being the shortest path, would be the a best heuristic score (min(f))
                // Everything stays the stay in terms of logic, but the sortset will be sorting based on heuristic weight. (If statement weight/distance check still stays the same), but instead it infers the next node to chose based on heuristics)
                
            return node.weight;
        }





    }
}