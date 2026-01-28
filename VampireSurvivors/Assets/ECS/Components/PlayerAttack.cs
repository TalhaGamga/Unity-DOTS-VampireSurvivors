using Unity.Entities;

public struct PlayerAttack : IComponentData
{
    public float Cooldown;
    public float CooldownTimer;

    public float Range;
    public float Damage;

    public float ProjectileSpeed;
    public float ProjectileRange;
}
