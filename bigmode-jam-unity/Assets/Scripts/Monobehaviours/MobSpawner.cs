using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public Mobber MobPrefab;
    public int NumMobbers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < NumMobbers; i++)
        {
            var randomPos = Random.insideUnitCircle * Mathf.Sqrt(NumMobbers);
            var mobber = Instantiate(MobPrefab, new Vector3(randomPos.x, 0f, randomPos.y), Quaternion.identity);
            mobber.NumMobbers = NumMobbers;
        }
    }
}
