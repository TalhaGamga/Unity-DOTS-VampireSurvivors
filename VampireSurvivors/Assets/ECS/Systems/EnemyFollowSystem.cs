using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PlayerPositionSystem))]
public partial struct EnemyFollowSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float3 playerPos =
            SystemAPI.GetSingleton<PlayerPosition>().Value;

        var job = new EnemyFollowJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPos = playerPos
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}
