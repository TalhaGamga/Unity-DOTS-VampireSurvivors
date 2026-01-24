using Unity.Entities;

public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
{
    public override void Bake(EnemySpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new EnemySpawner
        {
            EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
            Count = authoring.Count,
            MinSpawnRadius = authoring.MinSpawnRadius,
            MaxSpawnRadius = authoring.MaxSpawnRadius
        });
    }
}
