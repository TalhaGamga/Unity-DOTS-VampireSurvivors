using Unity.Entities;
using Unity.Mathematics;

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<EnemyTag>(entity);

        AddComponent(entity, new MoveSpeed
        {
            Value = authoring.MoveSpeed
        });

        AddComponent(entity, new FollowRange
        {
            Value = authoring.FollowRange
        });

        AddComponent<Inactive>(entity);

        AddComponent(entity, new FormationOffset
        {
            Value = new float3(
        UnityEngine.Random.Range(-1f, 1f),
        UnityEngine.Random.Range(-1f, 1f),
        0f
        )
        });

        AddComponent(entity, new DesiredRadius
        {
            // Spread enemies between 4.5 and 8 units
            Value = UnityEngine.Random.Range(1f, 5f)
        });
    }
}
