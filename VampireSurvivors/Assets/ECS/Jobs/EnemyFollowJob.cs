using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(EnemyTag))]
[WithNone(typeof(Inactive))]
public partial struct EnemyFollowJob : IJobEntity
{
    public float DeltaTime;
    public float3 PlayerPos;

    public void Execute(
        ref LocalTransform transform,
        in MoveSpeed speed,
        in FollowRange range)
    {
        float3 toPlayer = PlayerPos - transform.Position;
        float distSq = math.lengthsq(toPlayer);

        if (distSq <= range.Value * range.Value)
            return;

        float3 dir = math.normalize(toPlayer);
        transform.Position += dir * speed.Value * DeltaTime;
    }
}
