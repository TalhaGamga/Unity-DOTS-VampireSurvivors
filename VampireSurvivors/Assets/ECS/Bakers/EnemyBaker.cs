using Unity.Entities;

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
    }
}
