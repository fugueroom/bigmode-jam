using UnityEngine;

public class Tank : MonoBehaviour
{
    public GameObject Cannonball;
    public GameObject TargetMarker;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                var markerPos = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.01f , hitInfo.point.z);
                Instantiate(TargetMarker, markerPos, Quaternion.Euler(90f, 0f, 0f));
                var cannonball = Instantiate(Cannonball, transform.position, Quaternion.identity);
                var rb = cannonball.GetComponent<Rigidbody>();

                var dist = (hitInfo.point - cannonball.transform.position).magnitude;
                var launchAngle = (Vector3.Normalize(hitInfo.point - cannonball.transform.position) + Vector3.up) * 0.5f;
                var fireDir = Vector3.Normalize(launchAngle);

                // sqrt (g * dist / sin (2 * angle))
                var v0 = Mathf.Sqrt((9.81f * dist) / Mathf.Sin(Mathf.Deg2Rad * 90f));
                rb.AddForce(fireDir * v0, ForceMode.Impulse);
            }
        }
    }
}
