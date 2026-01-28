using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ProjectileDebugSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        int reqCount = SystemAPI.QueryBuilder().WithAll<ProjectileSpawnRequest>().Build().CalculateEntityCount();
        int activeCount = SystemAPI.QueryBuilder().WithAll<Projectile, ProjectileActive>().Build().CalculateEntityCount();

        //Debug.Log($"Requests: {reqCount} | ActiveProjectiles: {activeCount}");
    }
}