using UnityEngine;
using UnityEngine.AI;

public class Mobber : MonoBehaviour
{
    private UnitTarget target;
    NavMeshAgent agent;
    NavMeshPath path;
    Vector3 currentDestination;
    private Vector2 randomOffset;

    public GameObject Projectile;
    [HideInInspector] public int NumMobbers;

    private float offsetTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        target = FindFirstObjectByType<UnitTarget>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Start()
    {
        randomOffset = Random.insideUnitCircle * Mathf.Sqrt(NumMobbers);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }

        if (!target.Moving)
            return;

        offsetTimer += Time.deltaTime;

        if (offsetTimer > 10f)
        {
            // set new offset
            randomOffset = Random.insideUnitCircle * Mathf.Sqrt(NumMobbers);
            offsetTimer = 0f;
        }

        currentDestination = new Vector3(target.transform.position.x + randomOffset.x, 
            0f, 
            target.transform.position.z + randomOffset.y);

        if (NavMesh.CalculatePath(transform.position, currentDestination, NavMesh.AllAreas, path))
        {
            // always add a lil noise
            agent.SetDestination(currentDestination);
        }
        else
        {   
            /*
            if (NavMesh.SamplePosition(target.transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            */
        }
    }

    void FireProjectile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            var dist = (hitInfo.point - transform.position).magnitude;

            // max throwing distance
            if (dist < 25f)
            {
                var projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
                var rb = projectile.GetComponent<Rigidbody>();

                var launchAngle = (Vector3.Normalize(hitInfo.point - projectile.transform.position) + Vector3.up) * 0.5f;
                var fireDir = Vector3.Normalize(launchAngle);

                // sqrt (g * dist / sin (2 * angle))
                var v0 = Mathf.Sqrt((9.81f * dist) / Mathf.Sin(Mathf.Deg2Rad * 90f));
                rb.AddForce(fireDir * v0, ForceMode.Impulse);
                rb.AddTorque(projectile.transform.TransformDirection(projectile.transform.forward) * Random.Range(3f, 27f));
            }
        }
    }
}
