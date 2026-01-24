using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PlayerPositionSystem))]
public partial struct ElasticFormationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float3 playerPos = SystemAPI.GetSingleton<PlayerPosition>().Value;

        var job = new ElasticFormationJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPos = playerPos,
            SpringStrength = 8f,   // tweak this
            Damping = 6f           // tweak this
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}
