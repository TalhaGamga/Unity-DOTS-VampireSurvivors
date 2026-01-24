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
        ref EnemyVelocity velocity,
        in MoveSpeed speed)
    {
        float3 toPlayer = PlayerPos - transform.Position;
        float distSq = math.lengthsq(toPlayer);

        if (distSq < 0.0001f)
            return;

        float3 desiredDir = math.normalize(toPlayer);
        float3 desiredVelocity = desiredDir * speed.Value;

        // ?? THIS is the magic (Vampire Survivors feel)
        float steeringSharpness = 0.1f; // higher = snappier, lower = floatier
        velocity.Value = math.lerp(
            velocity.Value,
            desiredVelocity,
            1f - math.exp(-steeringSharpness * DeltaTime)
        );

        transform.Position += velocity.Value * DeltaTime;
    }
}
