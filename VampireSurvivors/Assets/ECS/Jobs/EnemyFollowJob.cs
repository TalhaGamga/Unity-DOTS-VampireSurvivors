using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(EnemyTag))]
[WithNone(typeof(Inactive), typeof(Disabled))]
public partial struct EnemyFollowJob : IJobEntity
{
    public float DeltaTime;
    public float3 PlayerPos;
    public int ActiveEnemyCount;

    public void Execute(
        ref LocalTransform transform,
        in MoveSpeed speed,
        in FormationIndex index,
        in DesiredRadius radius)
    {
        // Angle step based on how many enemies exist
        float angleStep = math.PI * 2f / ActiveEnemyCount;
        float angle = index.Value * angleStep;

        float3 desiredPos = PlayerPos + new float3(
            math.cos(angle),
            math.sin(angle),
            0f
        ) * radius.Value;

        float3 toDesired = desiredPos - transform.Position;

        // Smooth interpolation
        transform.Position += toDesired * speed.Value * DeltaTime;
    }
}
