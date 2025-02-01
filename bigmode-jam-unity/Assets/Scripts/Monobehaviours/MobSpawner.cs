using UnityEngine;
using System.Collections.Generic;

public class MobSpawner : MonoBehaviour
{
    public Mobber MobPrefab;
    public int NumMobbers;
    public int CurrentMobCount { get; private set; }

    private List<Mobber> mobbers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mobbers = new List<Mobber>();

        for (int i = 0; i < NumMobbers; i++)
        {
            var randomPos = Random.insideUnitCircle * Mathf.Sqrt(NumMobbers);
            var mobber = Instantiate(MobPrefab, new Vector3(randomPos.x, transform.position.y, randomPos.y), Quaternion.identity);

            mobbers.Add(mobber);
        }
    }

    public void SpawnAdditionalMobbers(Vector3 spawnPosition)
    {
        var mobber = Instantiate(MobPrefab, new Vector3(spawnPosition.x, transform.position.y, spawnPosition.z), Quaternion.identity);
        mobbers.Add(mobber);
    }
    
    private void Update()
    {
        CurrentMobCount = 0;

        for (int i = 0; i < mobbers.Count; i++)
        {
            if (mobbers[i].enabled)
            {
                transform.position += mobbers[i].transform.position;
                CurrentMobCount++;
            }
        }

        transform.position /= CurrentMobCount;
    }
    
}
