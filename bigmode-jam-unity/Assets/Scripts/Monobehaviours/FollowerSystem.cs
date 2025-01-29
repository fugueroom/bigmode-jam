using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct FollowerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // Get the leader
        //var leader = SystemAPI.GetSingletonEntity<Leader>();
        //var leaderTransform = SystemAPI.GetComponentRO<LocalTransform>(leader);
        /*
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Query all disabled followers
        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform>().WithDisabled<Follower>().Build(entityManager);
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
        NativeArray<LocalTransform> followerTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

        // Iterate through disabled followers and enable if close enough proximity
        for (int i = 0; i < followerTransformArray.Length; i++)
        {
            LocalTransform followerTransform = followerTransformArray[i];

            if (math.lengthsq(followerTransform.Position - leaderTransform.ValueRO.Position) < 10f)
            {
                SystemAPI.SetComponentEnabled<Follower>(entityArray[i], true);
            }
        }
        */
        /*
        FollowerJob followerJob = new FollowerJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            leaderTransform = leaderTransform.ValueRO,
        };

        followerJob.ScheduleParallel();
        */
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
        float3 noisyTarget = new float3(leaderTransform.Position.x + follower.randomUnitCircle.x, 0f, leaderTransform.Position.z + follower.randomUnitCircle.y);
        float3 moveDirection = noisyTarget - localTransform.Position;
        float targetProximity = 20f;

        if (math.lengthsq(moveDirection) < targetProximity)
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
            deltaTime * 5f);

        physicsVelocity.Linear = moveDirection * 5f;
        physicsVelocity.Angular = float3.zero;
    }
}