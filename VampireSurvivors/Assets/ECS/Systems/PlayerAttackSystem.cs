using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(ProjectileSpawnSystem))] // ensure requests exist before spawn consumes them
public partial struct PlayerAttackSystem : ISystem
{
    private EntityQuery _enemyQuery;

    public void OnCreate(ref SystemState state)
    {
        _enemyQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<EnemyTag>(),
            ComponentType.ReadOnly<LocalTransform>()
        );
    }

    public void OnUpdate(ref SystemState state)
    {
        if (_enemyQuery.IsEmpty) return;

        // Build enemy positions array
        var enemyTransforms = _enemyQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);
        var enemyPositions = new NativeArray<float3>(enemyTransforms.Length, Allocator.TempJob);

        for (int i = 0; i < enemyTransforms.Length; i++)
            enemyPositions[i] = enemyTransforms[i].Position;

        // Use EndSimulation ECB so playback is guaranteed
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var job = new PlayerAttackJob
        {
            EnemyPositions = enemyPositions,
            DeltaTime = SystemAPI.Time.DeltaTime,
            ECB = ecb
        };

        state.Dependency = job.Schedule(state.Dependency);

        // Ensure enemyPositions lives until job finishes
        state.Dependency = enemyPositions.Dispose(state.Dependency);
    }
}
