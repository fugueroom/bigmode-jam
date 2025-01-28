using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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

        foreach ((RefRW<LocalTransform> localTransform, RefRO<Leader> leader) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Leader>>())
        {
            var targetInput = inputActions.PlayerMap.PlayerMovement.ReadValue<Vector2>();

            if (targetInput.sqrMagnitude > 0f)
            {
                var moveDirection = new float3(targetInput.x, 0f, targetInput.y);
                localTransform.ValueRW.Position += moveDirection * leader.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
                localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRW.Rotation, quaternion.LookRotation(moveDirection, math.up()), leader.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);
            }
        }
    }
}
