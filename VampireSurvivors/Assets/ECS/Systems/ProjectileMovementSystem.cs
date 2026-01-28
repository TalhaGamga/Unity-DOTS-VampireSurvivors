using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))] // LocalToWorld hesaplanmadan önce yaz
public partial struct ProjectileMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        var job = new ProjectileMovementJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            ECB = ecb
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}
