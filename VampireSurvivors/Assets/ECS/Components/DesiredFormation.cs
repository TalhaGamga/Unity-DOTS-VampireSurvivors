using Unity.Entities;

public struct DesiredFormation : IComponentData
{
    public float Angle;      // slot angle
    public float Radius;     // rest distance
}
