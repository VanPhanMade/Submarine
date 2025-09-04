using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class FishManager
{
    public static FishManager Instance { get; private set; }

    private NativeArray<Vector3> positions;
    private NativeArray<Vector3> velocities;

    public FishManager(int fishCount)
    {
        Instance = this;
        positions = new NativeArray<Vector3>(fishCount, Allocator.Persistent);
        velocities = new NativeArray<Vector3>(fishCount, Allocator.Persistent);
    }

    public void UpdateFishData(int index, Vector3 pos, Vector3 vel)
    {
        positions[index] = pos;
        velocities[index] = vel;
    }

    public JobHandle ScheduleNeighborJob(out NativeArray<Vector3> results)
    {
        results = new NativeArray<Vector3>(positions.Length, Allocator.TempJob);

        var job = new NeighborJob
        {
            positions = positions,
            velocities = velocities,
            results = results
        };

        return job.Schedule(positions.Length, 64);
    }

    public void Dispose()
    {
        if (positions.IsCreated) positions.Dispose();
        if (velocities.IsCreated) velocities.Dispose();
    }

    struct NeighborJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> positions;
        [ReadOnly] public NativeArray<Vector3> velocities;
        public NativeArray<Vector3> results;

        public void Execute(int index)
        {
            // Example neighbor rule: just copy velocity
            results[index] = velocities[index];
        }
    }
}