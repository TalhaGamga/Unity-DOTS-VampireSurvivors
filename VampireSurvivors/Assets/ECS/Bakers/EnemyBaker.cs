using Unity.Entities;
using Unity.Mathematics;

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        // Identity
        AddComponent<EnemyTag>(entity);
        AddComponent<Inactive>(entity);

        // Movement
        AddComponent(entity, new MoveSpeed
        {
            Value = authoring.MoveSpeed
        });

        AddComponent(entity, new EnemyVelocity
        {
            Value = float3.zero
        });

        // Follow behavior
        AddComponent(entity, new FollowRange
        {
            Value = authoring.FollowRange
        });

        AddComponent(entity, new Damageable { Health = authoring.Health });
    }
}
