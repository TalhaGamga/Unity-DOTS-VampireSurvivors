using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PlayerMoveSystem))]
public partial struct PlayerPositionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.EntityManager.CreateSingleton<PlayerPosition>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float3 playerPos = float3.zero;

        foreach (var transform in
                 SystemAPI.Query<RefRO<LocalTransform>>()
                          .WithAll<PlayerTag>())
        {
            playerPos = transform.ValueRO.Position;
            break; 
        }

        SystemAPI.SetSingleton(new PlayerPosition
        {
            Value = playerPos
        });
    }
}
