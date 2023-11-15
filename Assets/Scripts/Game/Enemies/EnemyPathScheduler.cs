using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemies
{
    public class EnemyPathScheduler : MonoBehaviour
    {
        public Queue<ScheduledJob> jobs = new Queue<ScheduledJob>();
        
        public struct ScheduledJob
        {
            public EnemyBehaviour target;
            public ProcessEnemyPathJob job;
            public NativeList<Vector2Int> results;
            public NativeArray<MapInfo.TileInfo> tiles;
            public JobHandle handle;
        }

        public struct MapInfo
        {
            public struct TileInfo
            {
                public int x;
                public int y;
                public int weight;
            }

            public int width;
            public int height;
            public TileInfo[] map;
        }
        
        public struct ProcessEnemyPathJob : IJob
        {
            public Vector2Int startPosition;
            public Vector2Int endPosition;
            public int width;
            public int height;
            public NativeArray<MapInfo.TileInfo> tiles;

            public NativeList<Vector2Int> pathResults;

            public void Execute()
            {
                
                Pathfinding.TempNode[,] copiedMap = new Pathfinding.TempNode[width, height];
                
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        Pathfinding.TempNode newNode = new Pathfinding.TempNode();
                        newNode.x = x;
                        newNode.y = y;
                        newNode.weight = tiles[x + (y * height)].weight;

                        copiedMap[x, y] = newNode;
                    }
                }
                
                var bestPath = Pathfinding.GeneratePath(ref copiedMap, startPosition, endPosition);

                pathResults.ResizeUninitialized(bestPath.path.Count);
                for (int i = 0; i < bestPath.path.Count; i++)
                {
                    pathResults.Add(bestPath.path[i].GetPosition());
                }

            }
        }

        public void Schedule(MapInfo map, EnemyBehaviour enemy, Vector2Int startPosition, Vector2Int endPosition)
        {
            
            ScheduledJob schedule = new ScheduledJob()
            {
                target = enemy,
                results = new NativeList<Vector2Int>(Allocator.Persistent),
                tiles = new NativeArray<MapInfo.TileInfo>(map.map, Allocator.Persistent)
            };
            
            
            ProcessEnemyPathJob job = new ProcessEnemyPathJob()
            {
                startPosition = startPosition,
                endPosition = endPosition,
                tiles = schedule.tiles,
                pathResults = schedule.results,
                width = map.width,
                height = map.height
            };

            schedule.job = job;
            schedule.handle = job.Schedule();
            
            jobs.Enqueue(schedule);
        }

        public void Update()
        {
            ProcessFinishedJobs();
        }

        public void ProcessFinishedJobs()
        {
            Queue<ScheduledJob> toBeQueued = new Queue<ScheduledJob>();
            while (jobs.Count > 0)
            {
                var scheduledJob = jobs.Dequeue();
                if (scheduledJob.handle.IsCompleted)
                {
                    scheduledJob.handle.Complete(); // Force a completion

                    List<Vector2Int> path = new List<Vector2Int>(scheduledJob.results.Length);

                    foreach (var result in scheduledJob.results)
                    {
                        path.Add(result);
                    }
                    
                    scheduledJob.target.SetPath(path);
                    
                    scheduledJob.results.Dispose(); // Clean up the memory leak
                    scheduledJob.tiles.Dispose(); // Clean up the memory leak
                    
                }
                else
                {
                    toBeQueued.Enqueue(scheduledJob);
                }
            }

            while (toBeQueued.Count > 0)
            {
                jobs.Enqueue(toBeQueued.Dequeue());
            }

            
            
        }
        
        
    }
}