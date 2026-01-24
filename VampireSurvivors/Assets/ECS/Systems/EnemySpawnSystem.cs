using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct EnemySpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        // Runtime-safe random
        var rng = new Unity.Mathematics.Random(
            (uint)(SystemAPI.Time.ElapsedTime * 1000 + 1)
        );

        foreach (var (spawner, spawnerEntity) in
                 SystemAPI.Query<RefRO<EnemySpawner>>().WithEntityAccess())
        {
            var prefab = spawner.ValueRO.EnemyPrefab;
            int count = spawner.ValueRO.Count;

            float minR = spawner.ValueRO.MinSpawnRadius;
            float maxR = spawner.ValueRO.MaxSpawnRadius;

            for (int i = 0; i < count; i++)
            {
                Entity enemy = ecb.Instantiate(prefab);

                float angle = rng.NextFloat(0f, math.PI * 2f);
                float radius = rng.NextFloat(minR, maxR);

                float3 pos = new float3(
                    math.cos(angle),
                    math.sin(angle),
                    0f
                ) * radius;

                ecb.SetComponent(enemy, LocalTransform.FromPosition(pos));
            }

            ecb.DestroyEntity(spawnerEntity);
        }

        ecb.Playback(state.EntityManager);
    }
}
