using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct EnemySpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (spawner, spawnerEntity) in
                 SystemAPI.Query<RefRO<EnemySpawner>>().WithEntityAccess())
        {
            var prefab = spawner.ValueRO.EnemyPrefab;
            int count = spawner.ValueRO.Count;
            float radius = spawner.ValueRO.SpawnRadius;

            for (int i = 0; i < count; i++)
            {
                Entity enemy = ecb.Instantiate(prefab);
                ecb.AddComponent(enemy, new FormationIndex
                {
                    Value = i
                });
                float angle = (float)i / count * math.PI * 2f;
                float3 pos = new float3(
                    math.cos(angle),
                    math.sin(angle),
                    0f
                ) * radius;

                ecb.AddComponent(enemy, new DesiredFormation
                {
                    Angle = angle,
                    Radius = UnityEngine.Random.Range(5f, 8f)
                });

                ecb.AddComponent(enemy, new FormationState
                {
                    Velocity = float3.zero
                });

                ecb.SetComponent(enemy, LocalTransform.FromPosition(pos));
            }

            ecb.DestroyEntity(spawnerEntity);
        }

        ecb.Playback(state.EntityManager);
    }
}
