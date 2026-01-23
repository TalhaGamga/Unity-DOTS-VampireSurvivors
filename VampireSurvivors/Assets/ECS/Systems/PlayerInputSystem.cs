using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PlayerInputSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        foreach (var input in
                 SystemAPI.Query<RefRW<PlayerInput>>()
                          .WithAll<PlayerTag>())
        {
            input.ValueRW.Move = new float2(x, y);
        }
    }
}
