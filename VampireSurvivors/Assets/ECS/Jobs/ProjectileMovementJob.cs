using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(ProjectileActive))]
public partial struct ProjectileMovementJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter ECB;

    public void Execute(
        [EntityIndexInQuery] int sortKey,
        ref Projectile projectile,
        ref LocalTransform transform,
        Entity entity)
    {
        float3 displacement = projectile.Direction * projectile.Speed * DeltaTime;

        transform.Position += displacement;
        projectile.TraveledDistance += math.length(displacement);

        if (projectile.TraveledDistance >= projectile.Range)
        {
            ECB.SetComponentEnabled<ProjectileActive>(sortKey, entity, false);
        }
    }
}
