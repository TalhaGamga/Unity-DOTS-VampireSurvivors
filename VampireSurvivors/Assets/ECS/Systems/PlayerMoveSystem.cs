using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PlayerInputSystem))]
public partial struct PlayerMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new PlayerMoveJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}