using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileSpawnRequest : IComponentData
{
    public float3 Position;
    public float3 Direction;

    public float Damage;
    public float Range;
    public float Speed;
}