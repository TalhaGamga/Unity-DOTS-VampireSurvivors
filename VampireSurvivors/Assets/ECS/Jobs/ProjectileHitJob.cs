using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(ProjectileActive))]
public partial struct ProjectileHitJob : IJobEntity
{
    [ReadOnly] public NativeArray<float3> EnemyPositions;
    [ReadOnly] public NativeArray<Entity> EnemyEntities;

    public ComponentLookup<Damageable> DamageableLookup;
    public EntityCommandBuffer ECB;

    public void Execute(
        ref Projectile projectile,
        in LocalTransform transform,
        Entity projectileEntity)
    {
        float3 projPos = transform.Position;
        float hitRadiusSq = projectile.HitRadius * projectile.HitRadius;

        for (int i = 0; i < EnemyPositions.Length; i++)
        {
            float distSq = math.distancesq(projPos, EnemyPositions[i]);
            if (distSq > hitRadiusSq)
                continue;

            Entity enemy = EnemyEntities[i];

            var damageable = DamageableLookup.GetRefRW(enemy);
            damageable.ValueRW.Health -= projectile.Damage;

            ECB.SetComponentEnabled<ProjectileActive>(projectileEntity, false);

            return; 
        }
    }
}
