using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public GameObject Cannonball;
    public GameObject TargetMarker;
    public MobSpawner MobSpawner;

    public float range = 35f;
    private float shotTimer = 0f;
    private float shotThreshold = 6f;

    private GameObject spawnedCannonball;

    private void Start()
    {
        spawnedCannonball = Instantiate(Cannonball, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;
        var dist = (transform.position - MobSpawner.transform.position).magnitude;

        if (dist < range && shotTimer > shotThreshold )
        {
            //var markerPos = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.01f, hitInfo.point.z);
            //Instantiate(TargetMarker, markerPos, Quaternion.Euler(90f, 0f, 0f));
            spawnedCannonball.transform.position = transform.position;
            spawnedCannonball.SetActive(true);
            var rb = spawnedCannonball.GetComponent<Rigidbody>();

            var launchAngle = (Vector3.Normalize(MobSpawner.transform.position - spawnedCannonball.transform.position) + Vector3.up) * 0.5f;
            var fireDir = Vector3.Normalize(launchAngle);

            // sqrt (g * dist / sin (2 * angle))
            var v0 = Mathf.Sqrt((9.81f * dist) / Mathf.Sin(Mathf.Deg2Rad * 90f));
            rb.AddForce(fireDir * v0, ForceMode.Impulse);

            shotTimer = 0f;

            StartCoroutine(CannonballTimer());
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {

            }
        }
        */
    }

    IEnumerator CannonballTimer()
    {
        yield return new WaitForSeconds(5f);
        spawnedCannonball.SetActive(false);

        var rb = spawnedCannonball.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
