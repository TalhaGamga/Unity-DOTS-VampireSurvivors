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

        int activeCount = SystemAPI.GetSingleton<EnemyCount>().Value;

        var job = new EnemyFollowJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPos = playerPos,
            ActiveEnemyCount = math.max(1, activeCount)
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}
