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
        in FormationOffset offset,
        in DesiredRadius desiredRadius)
    {
        // Target with offset
        float3 target = PlayerPos + offset.Value;

        float3 toTarget = target - transform.Position;
        float dist = math.length(toTarget);

        // Distance error relative to desired radius
        float error = dist - desiredRadius.Value;

        // If close enough to desired ring ? stop
        if (math.abs(error) < 0.1f)
            return;

        float3 dir = math.normalize(toTarget);

        // Move inward or outward to correct spacing
        transform.Position += dir * error * speed.Value * DeltaTime;
    }
}
