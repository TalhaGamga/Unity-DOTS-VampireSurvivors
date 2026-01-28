using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct ProjectilePoolInitSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ProjectilePool>();
        state.RequireForUpdate<ProjectilePoolConfig>();
    }

    public void OnUpdate(ref SystemState state)
    {
        Entity poolEntity = SystemAPI.GetSingletonEntity<ProjectilePool>();
        var buffer = SystemAPI.GetBuffer<ProjectilePoolElement>(poolEntity);

        // Run once
        if (buffer.Length > 0)
            return;

        var pool = SystemAPI.GetComponentRW<ProjectilePool>(poolEntity);
        var config = SystemAPI.GetComponentRO<ProjectilePoolConfig>(poolEntity);

        int capacity = pool.ValueRO.Capacity;
        if (capacity <= 0)
            return;

        buffer.EnsureCapacity(capacity);

        for (int i = 0; i < capacity; i++)
        {
            // ? REAL entity immediately (not temp)
            Entity proj = state.EntityManager.Instantiate(config.ValueRO.Prefab);

            state.EntityManager.SetComponentData(
                proj,
                LocalTransform.FromPosition(config.ValueRO.ParkingPosition)
            );

            state.EntityManager.SetComponentEnabled<ProjectileActive>(proj, false);

            buffer.Add(new ProjectilePoolElement { Projectile = proj });
        }

        pool.ValueRW.NextIndex = 0;

        // Optional: remove config so it can’t re-init
        state.EntityManager.RemoveComponent<ProjectilePoolConfig>(poolEntity);
    }
}
