using Unity.Entities;
using Unity.Mathematics;

public struct FormationState : IComponentData
{
    public float3 Velocity; // spring velocity
}
