using UnityEngine;

public class MobberHut : MonoBehaviour
{
    public Transform SpawnPosition;
    public int NumAdditionalMobbers;

    private MobSpawner spawner;
    private bool spawned = false;

    private void Start()
    {
        spawner = FindFirstObjectByType<MobSpawner>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Mobber>() && !spawned)
        {
            spawned = true;

            // spawn x number of mobbers
            for (int i = 0; i < NumAdditionalMobbers; ++i)
            {
                spawner.SpawnAdditionalMobbers(SpawnPosition.position);
            }
        }
    }
}
