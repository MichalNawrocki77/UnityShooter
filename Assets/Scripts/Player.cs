using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    InputHandler inputHandler;
    PlayerInputActions playerInputActions;

    #region Input Variables
    public Transform cameraHolder;
    public float speed;
    public float mouseSensitivity;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = new InputHandler(this, rb);
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerMap.Enable();
        playerInputActions.PlayerMap.JumpAction.performed += inputHandler.JumpAction_performed;
        playerInputActions.PlayerMap.MovementAction.performed += inputHandler.MovementAction_performed;
        playerInputActions.PlayerMap.CameraMovementAction.performed += inputHandler.CameraMovementAction_performed;        
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }
    private void FixedUpdate()
    {
        inputHandler.PlayerMovement();
        inputHandler.CameraRotation();
    }
}