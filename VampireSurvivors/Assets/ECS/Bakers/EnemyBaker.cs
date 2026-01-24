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
        0f,
        UnityEngine.Random.Range(-1f, 1f)
        )
        });

    }
}
