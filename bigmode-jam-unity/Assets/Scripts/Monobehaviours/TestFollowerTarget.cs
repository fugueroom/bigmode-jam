using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class TestFollowerTarget : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Follower>().Build(entityManager);
            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Follower> followerArray = entityQuery.ToComponentDataArray<Follower>(Allocator.Temp);

            for (int i = 0; i < followerArray.Length; i++)
            {
                Follower follower = followerArray[i];
                follower.randomUnitCircle = Random.insideUnitCircle;
                followerArray[i] = follower;
            }

            entityQuery.CopyFromComponentDataArray(followerArray);
        }
    }
}
