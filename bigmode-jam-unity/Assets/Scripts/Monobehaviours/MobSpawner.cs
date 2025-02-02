using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MobSpawner : MonoBehaviour
{
    public GameObject TorchPrefab;
    public Mobber MobPrefab;
    public int NumMobbers;
    public int CurrentMobCount { get; private set; }

    public bool MainMenu = false;
    [HideInInspector] public bool GameOver { get; private set; }

    public GameObject GameOverUI;
    private List<Mobber> mobbers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mobbers = new List<Mobber>();

        for (int i = 0; i < NumMobbers; i++)
        {
            var randomPos = Random.insideUnitCircle * Mathf.Sqrt(NumMobbers);
            SpawnAdditionalMobbers(new Vector3(transform.position.x + randomPos.x, transform.position.y, transform.position.z + randomPos.y));
        }
    }

    public void SpawnAdditionalMobbers(Vector3 spawnPosition)
    {
        var mobber = Instantiate(MobPrefab, new Vector3(spawnPosition.x, transform.position.y, spawnPosition.z), Quaternion.identity);

        if (Random.value > 0.666f)
        {
            Vector3 torchOffset = new Vector3(0.4f, 1.8f, 0.2f);
            var torch = Instantiate(TorchPrefab, mobber.transform.position + torchOffset, Quaternion.identity, mobber.transform);
            torch.GetComponent<Rigidbody>().isKinematic = true;
            torch.GetComponent<Collider>().enabled = false;
            torch.transform.GetChild(0).gameObject.AddComponent<Billboard>();
        }

        mobbers.Add(mobber);
    }
    
    private void Update()
    {
        if (GameOver || MainMenu)
            return;

        CurrentMobCount = 0;
        transform.position = Vector3.zero;

        for (int i = 0; i < mobbers.Count; i++)
        {
            if (mobbers[i].enabled)
            {
                transform.position += mobbers[i].transform.position;
                CurrentMobCount++;
            }
        }

        if (CurrentMobCount > 0)
        {
            transform.position /= CurrentMobCount;
        }
        else
        {
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        GameOver = true;
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1.5f);
        GameOverUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }
    
}
