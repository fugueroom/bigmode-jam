using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct FlockingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*
        var leader = SystemAPI.GetSingletonEntity<Leader>();
        var leaderTransform = SystemAPI.GetComponentRO<LocalTransform>(leader);

        // Move towards average position of the group
        // * iterate over all followers and get average position
        // * then move towards that position
        float3 targetInput = float3.zero;
        int counter = 0;

        // First get average position of flock
        foreach (RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Follower>())
        {
            targetInput += localTransform.ValueRO.Position;
            counter++;
        }

        targetInput /= counter;

        // average of leader position and flock average
        targetInput = leaderTransform.ValueRO.Position;

        //Debug.Log(targetInput);
        float distribution = counter * 0.1f;

        foreach ((RefRW<LocalTransform> localTransform,
            RefRO<Follower> follower,
            RefRW<PhysicsVelocity> physicsVelocity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<Follower>, RefRW<PhysicsVelocity>>())
        {
            float3 moveDirection = targetInput - localTransform.ValueRO.Position;
            float3 target = new float3(moveDirection.x + follower.ValueRO.randomUnitCircle.x * distribution, 
                moveDirection.y, 
                moveDirection.z + follower.ValueRO.randomUnitCircle.y * distribution);

            if (math.lengthsq(target) < 1f)
            {
                // Reached target position
                physicsVelocity.ValueRW.Linear = float3.zero;
                physicsVelocity.ValueRW.Angular = float3.zero;
                continue;
            }

            float rotationSpeed = 3f + follower.ValueRO.randomUnitCircle.x;
            float movementSpeed = 5f;

            target = math.normalize(target);
            localTransform.ValueRW.Rotation =
                math.slerp(localTransform.ValueRW.Rotation,
                quaternion.LookRotation(target, math.up()),
            SystemAPI.Time.DeltaTime * rotationSpeed);

            physicsVelocity.ValueRW.Linear = target * movementSpeed;
            physicsVelocity.ValueRW.Angular = float3.zero;
        }
        
        // Align with average heading of the group
        // Avoid crowding other group members
        */
    }
}
