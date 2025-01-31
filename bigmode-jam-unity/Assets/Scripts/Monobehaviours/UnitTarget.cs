using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;


public class UnitTarget : MonoBehaviour
{
    public float chargeSpeed;
    public float normalSpeed;
    public float targetRotationSpeed;

    PlayerInputActions inputActions;

    float currentSpeed;

    public bool Moving { get; private set; }

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
        currentSpeed = normalSpeed;
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }
    
    void Update()
    {
        var targetInput = inputActions.PlayerMap.PlayerMovement.ReadValue<Vector2>();
        Moving = targetInput.sqrMagnitude > 0f;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSpeed = chargeSpeed;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentSpeed = normalSpeed;
        }

        if (Moving)
        {
            var moveDirection = new Vector3(targetInput.x, 0f, targetInput.y);
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
            transform.rotation = math.slerp(transform.rotation, quaternion.LookRotation(moveDirection, math.up()), targetRotationSpeed * Time.deltaTime);
        }
    }
    
}
