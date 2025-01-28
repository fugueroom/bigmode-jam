using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class FollowerAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float RotationSpeed;

    class Baker : Baker<FollowerAuthoring>
    {
        public override void Bake(FollowerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Follower
            {
                moveSpeed = authoring.MoveSpeed,
                rotationSpeed = authoring.RotationSpeed,
            });
        }
    }
}

public struct Follower : IComponentData
{
    public float moveSpeed;
    public float rotationSpeed;
    public float3 targetPosition;
}
