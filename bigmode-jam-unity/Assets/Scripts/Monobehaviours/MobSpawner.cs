using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public Mobber MobPrefab;
    public int NumMobbers;


    private Mobber[] mobbers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mobbers = new Mobber[NumMobbers];

        for (int i = 0; i < NumMobbers; i++)
        {
            var randomPos = Random.insideUnitCircle * Mathf.Sqrt(NumMobbers);
            var mobber = Instantiate(MobPrefab, new Vector3(randomPos.x, 0f, randomPos.y), Quaternion.identity);

            mobbers[i] = mobber;
            mobber.NumMobbers = NumMobbers;
            mobber.MobberIndex = i;
        }
    }
    
    private void Update()
    {
        int currentMobCount = 0;
        for (int i = 0; i < mobbers.Length; i++)
        {
            if (mobbers[i].enabled)
            {
                transform.position += mobbers[i].transform.position;
                currentMobCount++;
            }
        }

        transform.position /= currentMobCount;
    }
    
}
