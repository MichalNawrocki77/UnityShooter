using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    InputHandler inputHandler;
    PlayerInputActions playerInputActions;

    #region Mouse and Movement vars
    public Transform cameraHolder;
    public float speed;
    public float speedMultiplier;
    public float mouseSensitivity;
    #endregion

    #region drag vars

    [DoNotSerialize] public bool isGrounded;

    [SerializeField] float groundDrag;    
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayerMask;

    #endregion

    #region jump vars

    public float inAirSpeedMultiplier;
    public float jumpForce;

    #endregion



    private void Awake()
    {
        inputHandler = new InputHandler(this);
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
        CheckIfGrounded();
        SpeedControl();
        Debug.Log(isGrounded + " " + rb.velocity.magnitude);
    }
    void CheckIfGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f+0.2f, groundLayerMask);
        UpdateDrag();
    }
    void UpdateDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    void SpeedControl()
    {
        if(rb.velocity.magnitude > speed)
        {
            Vector3 newVelocity = rb.velocity.normalized * speed;
            rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
        }
    }
}