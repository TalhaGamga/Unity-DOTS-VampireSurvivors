using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerMoveJob : IJobEntity
{
    public float DeltaTime;

    public void Execute(ref LocalTransform transform, in PlayerInput input, in MoveSpeed speed)
    {
        float3 direction = new float3(input.Move.x, input.Move.y, 0f);

        if (math.lengthsq(direction) > 0f)
        {
            direction = math.normalize(direction);
        }

        transform.Position += direction * speed.Value * DeltaTime;
    }
}
