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
                randomUnitCircle = UnityEngine.Random.insideUnitCircle
            });

            //SetComponentEnabled<Follower>(entity, false);
        }
    }
}

public struct Follower : IComponentData, IEnableableComponent
{
    public float2 randomUnitCircle;
}
