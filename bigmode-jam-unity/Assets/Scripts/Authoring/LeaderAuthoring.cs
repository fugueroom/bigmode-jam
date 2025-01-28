using Unity.Entities;
using UnityEngine;

public class LeaderAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float RotationSpeed;

    class Baker : Baker<LeaderAuthoring>
    {
        public override void Bake(LeaderAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            AddComponent(entity, new Leader
            {
                moveSpeed = authoring.MoveSpeed,
                rotationSpeed = authoring.RotationSpeed,
            });
        }
    }
}

public struct Leader : IComponentData
{
    public float moveSpeed;
    public float rotationSpeed;
}
