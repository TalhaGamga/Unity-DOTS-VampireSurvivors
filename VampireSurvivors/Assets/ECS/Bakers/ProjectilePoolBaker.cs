using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ProjectilePoolBaker : Baker<ProjectilePoolAuthoring>
{
    public override void Bake(ProjectilePoolAuthoring authoring)
    {
        var poolEntity = GetEntity(TransformUsageFlags.None);

        AddComponent(poolEntity, new ProjectilePool
        {
            NextIndex = 0,
            Capacity = authoring.Capacity
        });

        AddBuffer<ProjectilePoolElement>(poolEntity);

        // Store prefab entity + parking pos in a separate config component
        AddComponent(poolEntity, new ProjectilePoolConfig
        {
            Prefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
            ParkingPosition = authoring.ParkingPosition
        });
    }
}

public struct ProjectilePoolConfig : IComponentData
{
    public Entity Prefab;
    public float3 ParkingPosition;
}
