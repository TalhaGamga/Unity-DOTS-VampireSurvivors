using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct EnemyActivationSystem : ISystem
{
    private float _timer;

    public void OnUpdate(ref SystemState state)
    {
        _timer += SystemAPI.Time.DeltaTime;

        if (_timer < 1f)
            return;

        _timer = 0f;

        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (_, entity) in
                 SystemAPI.Query<RefRO<EnemyTag>>()
                          .WithAll<Inactive>()
                          .WithEntityAccess())
        {
            ecb.RemoveComponent<Inactive>(entity);
            break; // activate ONE enemy per second
        }

        ecb.Playback(state.EntityManager);
    }
}