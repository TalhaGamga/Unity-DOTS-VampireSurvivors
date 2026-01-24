using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(EnemyTag))]
[WithNone(typeof(Inactive), typeof(Disabled))]
public partial struct ElasticFormationJob : IJobEntity
{
    public float DeltaTime;
    public float3 PlayerPos;

    // Tuning parameters (VERY important)
    public float SpringStrength;   // how strong the formation pulls back
    public float Damping;          // how much wobble is reduced

    public void Execute(
        ref LocalTransform transform,
        ref FormationState state,
        in DesiredFormation formation)
    {
        // Compute rest position
        float3 restPos = PlayerPos + new float3(
            math.cos(formation.Angle),
            math.sin(formation.Angle),
            0f
        ) * formation.Radius;

        // Spring force
        float3 displacement = restPos - transform.Position;
        float3 force = displacement * SpringStrength;

        // Integrate velocity
        state.Velocity += force * DeltaTime;

        // Damping (critical!)
        state.Velocity *= math.exp(-Damping * DeltaTime);

        // Apply motion
        transform.Position += state.Velocity * DeltaTime;
    }
}
