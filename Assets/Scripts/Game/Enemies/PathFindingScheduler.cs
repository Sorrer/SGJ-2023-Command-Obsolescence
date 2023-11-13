using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
    public class PathFindingScheduler : MonoBehaviour
    {
        public List<EnemyBehaviour> enemyBehaviours = new List<EnemyBehaviour>();
        
        // Nodes at X,Y

        public struct PathInfo
        {
            public Stack<TempNode> path;
            public float totalPathWeight;
        }
        
        public class TempNode
        {
            public int x;
            public int y;
            public float weight;
            public bool visited = false;
            // Integer cardinal directions
            // 1 2 3
            // 4 5 6
            // 7 8 9
            public TempNode nextNode = null;

            public override bool Equals(object obj)
            {
                // If obj is not TempNode node == null
                TempNode node = obj as TempNode;

                if (node == null) return false;

                // Only check positions, no layering
                return node.x == this.x && node.y == this.y;
            }
        }

        public class TempNodeComparar : IComparer<TempNode>
        {
            public int Compare(TempNode x, TempNode y)
            {
                var cal = x.weight - y.weight;

                if (cal < 0) return -1;
                if (cal > 0) return 1;
                return 0;
            }
        }
        
        public PathInfo GeneratePath(TempNode[,] world, Vector2Int startingPosition, Vector2Int finishPosition)
        {

            // Using queue for breadth-first search, depth search will result in a quicker execution time, but the first answer being correct.
            SortedSet<TempNode> unvisitedNodes = new SortedSet<TempNode>(new TempNodeComparar());
            int xWidth = world.GetLength(0);
            int yHeight = world.GetLength(1);

            for (int x = 0; x < xWidth; x++)
            {
                for (int y = 0; y < yHeight; y++)
                {
                    world[x, y].weight = float.PositiveInfinity;
                    unvisitedNodes.Add(world[x, y]);
                }
            }
            
            while (unvisitedNodes.Count > 0)
            {
                // Traverse the shortest constant path
                var node = unvisitedNodes.Min;
                unvisitedNodes.Remove(node);
                node.visited = true;

                if (node.x == finishPosition.x && node.y == finishPosition.y)
                {
                    // Solution found
                    break;
                }
                

                // Node the most cleanest looking code
                // Mostly looks this way because the actual nodes can be hard coded in position, otherwise it would be a neighbor list..

                // Integer cardinal directions
                // 1 2 3
                // 4 5 6
                // 7 8 9
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x - 1, node.y + 1);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x, node.y + 1);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x + 1, node.y + 1);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x - 1, node.y);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x + 1, node.y);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x - 1, node.y - 1);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x , node.y - 1);
                ProcessPathNode(ref unvisitedNodes, ref world, xWidth, yHeight, node.x, node.y, node.x + 1, node.y - 1);


            }
            
            
            // Generate best path by getting the finishPosition
            Stack<TempNode> bestPath = new Stack<TempNode>();
            float totalPathWeight = 0;
            TempNode nextNode = world[finishPosition.x, finishPosition.y];
            while (nextNode != null && (nextNode.x == startingPosition.x && nextNode.y == startingPosition.y))
            {
                bestPath.Push(nextNode);
                totalPathWeight += nextNode.weight;
                nextNode = nextNode.nextNode;
            }

            PathInfo info = new PathInfo();

            info.path = bestPath;
            info.totalPathWeight = totalPathWeight;

            return info;
        }


        
        private void ProcessPathNode(ref SortedSet<TempNode> unvisitedNodes, ref TempNode[,] world, int xWidth, int yHeight, int curX, int curY, int neighborX, int neighborY)
        {
            // Out of bounds
            if (neighborX < 0 || neighborX >= xWidth || neighborY < 0 && neighborY >= yHeight) return;

            // Already visited we can ignore
            

            if (!CanTraverse(world[neighborX, neighborY])) return;

            if (!world[neighborX, neighborY].visited) return;
            
            var weight = world[curX, curY].weight + GetWeight(world[neighborX, neighborY]);

            if (weight < world[neighborX, neighborY].weight)
            {
                // Resort this item, since it has a new weight now (2(log n), but this will happen o(b) b=best path length)
                world[neighborX, neighborY].weight = weight;
                world[neighborX, neighborY].nextNode = world[curX, curY];
                
                unvisitedNodes.Remove(world[neighborX, neighborY]);
                unvisitedNodes.Add(world[neighborX, neighborY]);
            }
            
        }

        

        public bool CanTraverse(TempNode node)
        {
            return node.weight == -1;
        }

        public float GetWeight(TempNode node)
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