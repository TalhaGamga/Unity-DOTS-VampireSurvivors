using Unity.Entities;

public struct EnemySpawner : IComponentData
{
    public Entity EnemyPrefab;
    public int Count;
    public float SpawnRadius;
}
