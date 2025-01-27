using Unity.Entities;
using UnityEngine;

public class PlayableUnitAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayableUnitAuthoring>
    {
        public override void Bake(PlayableUnitAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayableUnit());
        }
    }
}

public struct PlayableUnit : IComponentData
{

}
