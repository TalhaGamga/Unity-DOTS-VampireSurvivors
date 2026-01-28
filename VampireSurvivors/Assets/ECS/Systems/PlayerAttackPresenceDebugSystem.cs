using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerAttackPresenceDebugSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        int playerCount = SystemAPI.QueryBuilder()
            .WithAll<PlayerAttack, LocalTransform>()
            .Build()
            .CalculateEntityCount();

        //Debug.Log($"Player entities with PlayerAttack+LocalTransform: {playerCount}");
    }
}
