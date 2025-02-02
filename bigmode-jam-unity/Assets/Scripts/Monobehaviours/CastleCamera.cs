using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CastleCamera : MonoBehaviour
{
    private CinemachineFollow _camera;
    private bool panning;

    private void Start()
    {
        _camera = FindFirstObjectByType<CinemachineFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Castle camera!");
        if (other.GetComponent<Mobber>() && !panning)
        {
            StartCoroutine(PanCamera());
        }
    }

    IEnumerator PanCamera()
    {
        panning = true;
        float camY = _camera.FollowOffset.y;

        while (camY < 37f)
        {
            camY += Time.deltaTime;
            _camera.FollowOffset = new Vector3(_camera.FollowOffset.x, camY, _camera.FollowOffset.z);
            yield return null;
        }
    }    
}
