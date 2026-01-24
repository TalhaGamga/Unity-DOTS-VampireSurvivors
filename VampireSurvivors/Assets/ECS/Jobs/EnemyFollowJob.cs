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

    public void Execute(
        ref LocalTransform transform,
        in MoveSpeed speed,
        in FollowRange range,
        in FormationOffset offset)
    {
        // Each enemy aims for a slightly different point near the player
        float3 target = PlayerPos + offset.Value;

        float3 toTarget = target - transform.Position;
        float distSq = math.lengthsq(toTarget);

        // Stop when close enough (prevents collapsing)
        if (distSq <= range.Value * range.Value)
            return;

        float3 dir = math.normalize(toTarget);

        transform.Position += dir * speed.Value * DeltaTime;
    }
}
