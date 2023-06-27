using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    PlayerInputActions playerInputActions;
    [SerializeField] Transform cameraHolder;
    float cameraVerticalRotation;

    [SerializeField] float speed;

    Vector2 playerMovementVector2;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerMap.Enable();
        playerInputActions.PlayerMap.JumpAction.performed += JumpAction_performed;
        playerInputActions.PlayerMap.MovementAction.performed += MovementAction_performed;
        playerInputActions.PlayerMap.CameraMovementAction.performed += CameraMovementAction_performed;

    }
    private void FixedUpdate()
    {
        UpdatePlayerMovement(playerMovementVector2);
    }
    
    #region CameraMovement

    private void CameraMovementAction_performed(InputAction.CallbackContext obj)
    {
        float sensitivity = 0.25f;
        //caching the vertical rotation so that I can clamp it later
        cameraVerticalRotation = cameraHolder.eulerAngles.x;

        Vector2 inputValueAsVector2 = obj.ReadValue<Vector2>()*sensitivity;

        //rotate the whole object along Y axis (left and right)
        //
        transform.Rotate(new Vector3(0f, inputValueAsVector2.x, 0f));

        cameraVerticalRotation += inputValueAsVector2.y;
        Mathf.Clamp(cameraVerticalRotation,-90f, 90f);

        cameraHolder.localEulerAngles = new Vector3(cameraVerticalRotation, 0f, 0f);
        
    } 
    #endregion

    #region PlayerMovement

    private void MovementAction_performed(InputAction.CallbackContext obj)
    {
        playerMovementVector2 = obj.ReadValue<Vector2>();
    }
    private void UpdatePlayerMovement(Vector2 movementVector2)
    {
        rb.AddForce(transform.forward * movementVector2.y,ForceMode.VelocityChange);
        rb.AddForce(transform.right*movementVector2.x,ForceMode.VelocityChange);
    }
    #endregion

    #region Jump
    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("JumpAction_performed() called");
        rb.AddForce(Vector3.up * 4, ForceMode.Impulse);
    }
    #endregion
}