using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyPathScheduler : MonoBehaviour
    {

        public static EnemyPathScheduler instance;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(this);   
                Debug.LogError("Found multiple enemy path schedulers, please only have one");
            }
            else
            {
                instance = this;
            }
        }

        public Queue<ScheduledJob> jobs = new Queue<ScheduledJob>();
        
        public struct ScheduledJob
        {
            public EnemyPathProcesser target;
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

                public TileInfo Copy()
                {
                    return new TileInfo()
                    {
                        x = x,
                        y = y,
                        weight = weight
                    };
                }
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
            public int weightVariance;
            public NativeList<Vector2Int> pathResults;

            public void Execute()
            {
                
                Pathfinding.TempNode[,] copiedMap = new Pathfinding.TempNode[width, height];

                var random = new Unity.Mathematics.Random();
                
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        Pathfinding.TempNode newNode = new Pathfinding.TempNode();
                        newNode.x = x;
                        newNode.y = y;
                        newNode.weight = tiles[x + (y * width)].weight;
                        newNode.weight += newNode.weight == -1 ? 0 : random.NextInt(weightVariance);
                        copiedMap[x, y] = newNode;
                    }
                }

                
                var bestPath = Pathfinding.GeneratePath(ref copiedMap, startPosition, endPosition);

                if (bestPath.path != null)
                {
                    for (int i = 0; i < bestPath.path.Count; i++)
                    {
                        pathResults.Add(new Vector2Int(bestPath.path[i].x, bestPath.path[i].y));
                    }
                    
                }

            }
        }

        public void Schedule(MapInfo map, EnemyPathProcesser enemy, Vector2Int startPosition, Vector2Int endPosition, int weightVariance = 0)
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
                height = map.height,
                weightVariance = weightVariance
            };

            schedule.job = job;
            schedule.handle = job.Schedule();
            
            jobs.Enqueue(schedule);
        }

        public void Update()
        {
            ProcessJobs();
        }

        private void ProcessJobs()
        {
            // TODO: Have jobs get scheduled here, and only allow x amount of jobs to be scheduled at a time
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
                        path.Add(new Vector2Int(result.x, result.y));
                    }

                    scheduledJob.target.StartPath(path);
                    
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