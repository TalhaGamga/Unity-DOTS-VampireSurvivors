using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(EnemyActivationSystem))]
public partial struct EnemyCountSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // Create exactly ONE EnemyCount entity
        state.EntityManager.CreateSingleton<EnemyCount>();
    }

    public void OnUpdate(ref SystemState state)
    {
        int count = 0;

        // Count only ACTIVE enemies
        foreach (var _ in
                 SystemAPI.Query<RefRO<EnemyTag>>()
                          .WithNone<Inactive, Disabled>())
        {
            count++;
        }

        SystemAPI.SetSingleton(new EnemyCount
        {
            Value = count
        });
    }
}
