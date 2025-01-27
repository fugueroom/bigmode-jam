using UnityEngine;

public class UnitTarget : MonoBehaviour
{
    public float targetSpeed;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var targetInput = inputActions.PlayerMap.PlayerMovement.ReadValue<Vector2>();

        transform.position += new Vector3(targetInput.x, 0f, targetInput.y) * targetSpeed;
    }
}
