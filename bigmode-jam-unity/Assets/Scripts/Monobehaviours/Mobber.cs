using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mobber : MonoBehaviour
{
    private UnitTarget target;
    private MobSpawner spawner;

    NavMeshAgent agent;
    NavMeshPath path;
    Vector3 currentDestination;
    private Vector2 randomOffset;

    public GameObject Projectile;
    public GameObject Mesh;
    public GameObject Bloodstain;

    public float NormalSpeed;
    public float ChargeSpeed;
    public float TorchReloadTime;

    private float offsetTimer = 0f;
    private GameObject torch;
    private bool canThrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        target = FindFirstObjectByType<UnitTarget>();
        spawner = FindFirstObjectByType<MobSpawner>();

        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Start()
    {
        randomOffset = Random.insideUnitCircle * Mathf.Sqrt(spawner.CurrentMobCount);
        torch = Instantiate(Projectile, transform.position, Quaternion.identity);
        torch.SetActive(false);
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        if (Input.GetMouseButtonDown(0) && canThrow)
        {
            FireProjectile();
        }

        if (!target.Moving)
            return;

        var currentDistance = 0f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // charge, in a line?
            currentDistance = (transform.position - target.transform.position).magnitude;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            // set offset while space is held
            agent.speed = ChargeSpeed;
            var targetPos = target.transform.position - target.transform.forward * currentDistance * 2f;
            currentDestination = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
        else
        {
            currentDestination = new Vector3(target.transform.position.x + randomOffset.x,
            transform.position.y,
            target.transform.position.z + randomOffset.y);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // stop charge
            randomOffset = Random.insideUnitCircle * Mathf.Sqrt(spawner.CurrentMobCount);
            agent.speed = NormalSpeed;
        }
        /*
        offsetTimer += Time.deltaTime;

        if (offsetTimer > 10f)
        {
            // set new offset
            randomOffset = Random.insideUnitCircle * Mathf.Sqrt(NumMobbers);
            offsetTimer = 0f;
        }
        */

        if (NavMesh.CalculatePath(transform.position, currentDestination, NavMesh.AllAreas, path))
        {
            // always add a lil noise
            agent.SetDestination(currentDestination);
        }
        else
        {   
            
            if (NavMesh.SamplePosition(target.transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            
        }
    }

    void FireProjectile()
    {
        var viewportPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        Ray ray = Camera.main.ViewportPointToRay(viewportPos);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            var dist = (hitInfo.point - transform.position).magnitude;

            // max throwing distance
            if (dist < 25f)
            {
                //var projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
                torch.transform.position = transform.position;
                torch.SetActive(true);

                var rb = torch.GetComponent<Rigidbody>();

                var launchAngle = (Vector3.Normalize(hitInfo.point - torch.transform.position) + Vector3.up) * 0.5f;
                var fireDir = Vector3.Normalize(launchAngle);

                // sqrt (g * dist / sin (2 * angle))
                var v0 = Mathf.Sqrt((9.81f * dist) / Mathf.Sin(Mathf.Deg2Rad * 90f));
                rb.AddForce(fireDir * v0, ForceMode.Impulse);
                rb.AddTorque(torch.transform.TransformDirection(torch.transform.forward) * Random.Range(3f, 27f));

                StartCoroutine(TorchReload());
            }
        }
    }

    IEnumerator TorchReload()
    {
        canThrow = false;
        yield return new WaitForSeconds(TorchReloadTime);

        var rb = torch.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        torch.transform.rotation = Quaternion.identity;
        torch.SetActive(false);
        canThrow = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannonball"))
        {
            Mesh.SetActive(false);
            Bloodstain.SetActive(true);

            agent.enabled = false;
            enabled = false;

            // this is bad
            var torchChild = transform.GetComponentInChildren<Rigidbody>();
            if (torchChild != null)
            {
                Destroy(torchChild.gameObject);
            }
        }
    }
}
