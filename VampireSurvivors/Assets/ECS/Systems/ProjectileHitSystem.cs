using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(ProjectileMovementSystem))]
public partial struct ProjectileHitSystem : ISystem
{
    private EntityQuery _enemyQuery;

    public void OnCreate(ref SystemState state)
    {
        _enemyQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<EnemyTag>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadWrite<Damageable>()
        );
    }

    public void OnUpdate(ref SystemState state)
    {
        if (_enemyQuery.IsEmpty)
            return;

        var enemyEntities = _enemyQuery.ToEntityArray(state.WorldUpdateAllocator);
        var enemyTransforms = _enemyQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);

        var enemyPositions = new NativeArray<float3>(
            enemyTransforms.Length,
            Allocator.TempJob);

        for (int i = 0; i < enemyTransforms.Length; i++)
            enemyPositions[i] = enemyTransforms[i].Position;

        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        var job = new ProjectileHitJob
        {
            EnemyEntities = enemyEntities,
            EnemyPositions = enemyPositions,
            DamageableLookup = state.GetComponentLookup<Damageable>(),
            ECB = ecb
        };

        job.Schedule();

        state.Dependency.Complete();

        enemyPositions.Dispose();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
