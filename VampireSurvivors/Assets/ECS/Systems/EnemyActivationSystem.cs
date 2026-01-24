using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct EnemyActivationSystem : ISystem
{
    private float _timer;

    public void OnUpdate(ref SystemState state)
    {
        Debug.Log("EnemyActivationSyustem");
        _timer += SystemAPI.Time.DeltaTime;

        // Activate 50 enemies per second (tweakable)
        if (_timer < 0.02f)
            return;

        _timer = 0f;

        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        int activated = 0;
        const int batchSize = 10;

        foreach (var (_, entity) in
                 SystemAPI.Query<RefRO<EnemyTag>>()
                          .WithAll<Inactive>()
                          .WithEntityAccess())
        {
            ecb.RemoveComponent<Disabled>(entity);
            ecb.RemoveComponent<Inactive>(entity);
            activated++;

            if (activated >= batchSize)
                break;
        }

        ecb.Playback(state.EntityManager);
        Debug.Log("EnemyActivationSyustem2");

    }
}
