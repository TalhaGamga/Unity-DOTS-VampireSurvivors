using Unity.Entities;

public struct ProjectilePool : IComponentData
{
    public int NextIndex;
    public int Capacity;
}
