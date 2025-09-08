/*
© 2025 Van Phan. All Rights Reserved.
Behaves as the "Academy/Brain" system defined by Unity's
native implementation of ML-Agents.
*/

using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class FishManager : IDisposable
{
    #region FIELDS
    public static FishManager Instance { get; private set; }

    private NativeArray<Vector3> positions;
    private NativeArray<Vector3> velocities;

    private bool disposed;
    #endregion

    #region FISHMANAGER
    public FishManager(int fishCount)
    {
        if (Instance != null) throw new Exception("Class (FishManager): Multiple instances detected.");
        if (fishCount <= 0) throw new Exception("Class (FishManager): fishCount must be > 0.");

        Instance = this;

        positions = new NativeArray<Vector3>(fishCount, Allocator.Persistent);
        velocities = new NativeArray<Vector3>(fishCount, Allocator.Persistent);

        Application.quitting += OnAppQuit;

#if UNITY_EDITOR
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
#endif
    }

    public void UpdateFishData(int index, Vector3 pos, Vector3 vel)
    {
        if (disposed) throw new Exception("Class (FishManager): UpdateFishData called after dispose.");
        if (!positions.IsCreated || !velocities.IsCreated) throw new Exception("Class (FishManager): NativeArrays not created.");
        if ((uint)index >= (uint)positions.Length) throw new Exception($"Class (FishManager): Index {index} out of range.");

        positions[index] = pos;
        velocities[index] = vel;
    }

    /// <summary>
    /// Caller MUST: handle.Complete(); results.Dispose();
    /// </summary>
    public JobHandle ScheduleNeighborJob(out NativeArray<Vector3> results)
    {
        if (disposed) throw new Exception("Class (FishManager): ScheduleNeighborJob called after dispose.");
        if (!positions.IsCreated || !velocities.IsCreated) throw new Exception("Class (FishManager): NativeArrays not created.");

        results = new NativeArray<Vector3>(positions.Length, Allocator.TempJob);

        var job = new NeighborJob
        {
            positions = positions,
            velocities = velocities,
            results = results
        };

        return job.Schedule(positions.Length, 64);
    }
    #endregion

    #region DISPOSAL
    public void Dispose()
    {
        if (disposed) return;
        disposed = true;

        if (positions.IsCreated) positions.Dispose();
        if (velocities.IsCreated) velocities.Dispose();

        Application.quitting -= OnAppQuit;

#if UNITY_EDITOR
        AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
#endif
        if (Instance == this) Instance = null;

        GC.SuppressFinalize(this);
    }

    ~FishManager() { Dispose(); }

    private void OnAppQuit() => Dispose();
#if UNITY_EDITOR
    private void OnBeforeAssemblyReload() => Dispose();
#endif
    #endregion

    #region JOBS
    struct NeighborJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> positions;
        [ReadOnly] public NativeArray<Vector3> velocities;
        public NativeArray<Vector3> results;

        public void Execute(int index)
        {
            // TODO: replace with real boids math (alignment/cohesion/separation)
            results[index] = velocities[index];
        }
    }
    #endregion
}