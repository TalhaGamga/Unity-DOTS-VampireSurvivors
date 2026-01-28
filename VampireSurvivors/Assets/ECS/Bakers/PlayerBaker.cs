using Unity.Entities;

public class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new PlayerAttack
        {
            Cooldown = authoring.Cooldown,
            CooldownTimer = 0f,
            Range = authoring.Range,
            Damage = authoring.Damage,
            ProjectileSpeed = authoring.ProjectileSpeed,
            ProjectileRange = authoring.ProjectileRange
        });

        AddComponent(entity, new MoveSpeed
        {
            Value = authoring.MoveSpeed
        });

        AddComponent<PlayerInput>(entity);
        // optional tag for querying
        AddComponent<PlayerTag>(entity);
    }
}
