using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PlayerAttackSystem))]
public partial struct ProjectileSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ProjectilePool>();
        state.RequireForUpdate<ProjectileSpawnRequest>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        Entity poolEntity = SystemAPI.GetSingletonEntity<ProjectilePool>();
        var pool = SystemAPI.GetComponentRW<ProjectilePool>(poolEntity);
        var poolBuffer = SystemAPI.GetBuffer<ProjectilePoolElement>(poolEntity);

        int capacity = pool.ValueRO.Capacity;
        int index = pool.ValueRO.NextIndex;

        foreach (var (request, requestEntity) in
                 SystemAPI.Query<RefRO<ProjectileSpawnRequest>>().WithEntityAccess())
        {
            // Safety: if buffer not filled, do nothing
            if (poolBuffer.Length == 0)
                break;

            Entity projectile = poolBuffer[index].Projectile;
            index = (index + 1) % capacity;

            float3 dir = request.ValueRO.Direction;
            dir.z = 0f;
            dir = math.normalizesafe(dir, new float3(1, 0, 0));
            float angle = math.atan2(dir.y, dir.x);

            ecb.SetComponent(projectile, new LocalTransform
            {
                Position = request.ValueRO.Position,
                Rotation = quaternion.RotateZ(angle),
                Scale = 1f
            });


            ecb.SetComponent(projectile, new Projectile
            {
                Speed = request.ValueRO.Speed,
                Damage = request.ValueRO.Damage,
                Range = request.ValueRO.Range,
                Direction = request.ValueRO.Direction,
                TraveledDistance = 0f,
                HitRadius = 0.25f // IMPORTANT: set something non-zero if you use it
            });

            ecb.SetComponentEnabled<ProjectileActive>(projectile, true);

            ecb.DestroyEntity(requestEntity);
        }

        pool.ValueRW.NextIndex = index;
    }
}
