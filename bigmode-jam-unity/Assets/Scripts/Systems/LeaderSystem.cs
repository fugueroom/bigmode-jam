using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class LeaderSystem : SystemBase
{
    private PlayerInputActions inputActions;

    protected override void OnUpdate()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
            inputActions.Enable();
        }

        foreach ((RefRW<LocalTransform> localTransform, 
            RefRO<Leader> leader,
            RefRW<PhysicsVelocity> physicsVelocity) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<Leader>, RefRW<PhysicsVelocity>>())
        {
            var targetInput = inputActions.PlayerMap.PlayerMovement.ReadValue<Vector2>();

            if (math.lengthsq(targetInput) == 0f)
            {
                // Reached target position
                physicsVelocity.ValueRW.Linear = float3.zero;
                physicsVelocity.ValueRW.Angular = float3.zero;
                return;
            }

            var moveDirection = new float3(targetInput.x, 0f, targetInput.y);
            localTransform.ValueRW.Rotation =
                math.slerp(localTransform.ValueRW.Rotation,
                quaternion.LookRotation(moveDirection, math.up()),
            SystemAPI.Time.DeltaTime * 5f);

            physicsVelocity.ValueRW.Linear = moveDirection * 5f;
            physicsVelocity.ValueRW.Angular = float3.zero;

            //localTransform.ValueRW.Position += moveDirection * leader.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
            //localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRW.Rotation, quaternion.LookRotation(moveDirection, math.up()), leader.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);
        }
    }
}
