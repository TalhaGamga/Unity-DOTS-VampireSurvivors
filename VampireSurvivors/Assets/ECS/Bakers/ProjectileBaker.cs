using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ProjectileBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new Projectile
        {
            Speed = authoring.Speed,
            Range = authoring.Range,
            Damage = 0f,
            Direction = float3.zero,
            TraveledDistance = 0f
        });

        AddComponent<ProjectileActive>(entity);
        SetComponentEnabled<ProjectileActive>(entity, false);
    }
}
