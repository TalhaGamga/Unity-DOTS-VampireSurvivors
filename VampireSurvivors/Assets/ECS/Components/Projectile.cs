using Unity.Entities;
using Unity.Mathematics;

public struct Projectile : IComponentData
{
    public float Speed;
    public float Damage;
    public float Range;
    public float HitRadius;

    public float3 Direction;
    public float TraveledDistance;
}
