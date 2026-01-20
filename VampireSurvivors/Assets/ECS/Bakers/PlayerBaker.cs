using Unity.Entities;

public class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<PlayerTag>(entity);

        AddComponent(entity, new MoveSpeed
        {
            Value = authoring.MoveSpeed
        });
    }
}