using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CastleCamera : MonoBehaviour
{
    public MovingKeep[] Keeps;

    private CinemachineFollow _camera;
    private bool panning;

    public bool BeginningOfTheEnd { get; private set; }

    private void Start()
    {
        _camera = FindFirstObjectByType<CinemachineFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Mobber>() && !panning)
        {
            StartCoroutine(PanCamera());
        }
    }

    IEnumerator PanCamera()
    {
        panning = true;
        float camY = _camera.FollowOffset.y;

        while (camY < 42f)
        {
            camY += Time.deltaTime * 2f;
            _camera.FollowOffset = new Vector3(_camera.FollowOffset.x, camY, _camera.FollowOffset.z);
            yield return null;
        }

        foreach (var keep in Keeps)
        {
            keep.enabled = true;
        }

        BeginningOfTheEnd = true;
    }    
}
