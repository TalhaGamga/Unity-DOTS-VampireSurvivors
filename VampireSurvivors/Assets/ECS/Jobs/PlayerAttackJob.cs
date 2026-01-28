using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerAttackJob : IJobEntity
{
    [ReadOnly] public NativeArray<float3> EnemyPositions;

    public float DeltaTime;
    public EntityCommandBuffer ECB;

    public void Execute(ref PlayerAttack attack, in LocalTransform playerTransform)
    {
        attack.CooldownTimer -= DeltaTime;
        if (attack.CooldownTimer > 0f) return;

        float3 playerPos = playerTransform.Position;
        float minDistSq = attack.Range * attack.Range;
        int bestEnemyIndex = -1;

        for (int i = 0; i < EnemyPositions.Length; i++)
        {
            float distSq = math.distancesq(playerPos, EnemyPositions[i]);
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                bestEnemyIndex = i;
            }
        }

        if (bestEnemyIndex == -1) return;

        float3 direction = math.normalizesafe(EnemyPositions[bestEnemyIndex] - playerPos);

        // Create spawn request entity
        var reqEntity = ECB.CreateEntity();
        ECB.AddComponent(reqEntity, new ProjectileSpawnRequest
        {
            Position = playerPos,
            Direction = direction,
            Damage = attack.Damage,
            Range = attack.ProjectileRange,
            Speed = attack.ProjectileSpeed
        });

        attack.CooldownTimer = attack.Cooldown;
    }
}
