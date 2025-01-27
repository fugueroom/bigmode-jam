using UnityEngine;
using UnityEngine.InputSystem;


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
    
    void FixedUpdate()
    {
        var targetInput = inputActions.PlayerMap.PlayerMovement.ReadValue<Vector2>();
        transform.position += new Vector3(targetInput.x, 0f, targetInput.y) * targetSpeed;
    }
    
}
