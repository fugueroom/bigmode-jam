using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct FollowerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var leader = SystemAPI.GetSingletonEntity<Leader>();
        var leaderTransform = SystemAPI.GetComponentRO<LocalTransform>(leader);

        FollowerJob followerJob = new FollowerJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            leaderTransform = leaderTransform.ValueRO,
        };

        followerJob.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct FollowerJob : IJobEntity
{
    public float deltaTime;
    public LocalTransform leaderTransform;

    // ref acts as a RW reference, in acts as RO reference
    public void Execute(ref LocalTransform localTransform, in Follower follower, ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirection = leaderTransform.Position - localTransform.Position;
        float reachedTargetPosition = 5f;

        if (math.lengthsq(moveDirection) < reachedTargetPosition)
        {
            // Reached target position
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;
        }

        moveDirection = math.normalize(moveDirection);
        localTransform.Rotation =
            math.slerp(localTransform.Rotation,
            quaternion.LookRotation(moveDirection, math.up()),
            deltaTime * follower.rotationSpeed);

        physicsVelocity.Linear = moveDirection * follower.moveSpeed;
        physicsVelocity.Angular = float3.zero;
    }
}