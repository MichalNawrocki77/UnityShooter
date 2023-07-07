using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    public Rigidbody Rb { get; private set; }

    public InputHandler InputHandler { get; private set; }

    PlayerInputActions playerInputActions;

    #region StateMachine
    public PlayerStateMachine StateMachine { get { return stateMachine; } }
    PlayerStateMachine stateMachine;
    public PlayerWalkState WalkState { get { return walkState; } }
    PlayerWalkState walkState;
    public PlayerInAirState AerialState { get { return aerialState; } }
    PlayerInAirState aerialState;

    #endregion

    #region Mouse and Movement
    public Transform CameraHolder { get { return cameraHolder; } }
    [SerializeField] Transform cameraHolder;

    public float Speed { get { return speed; } }
    [SerializeField] float speed;

    public float SpeedMultiplier { get { return speedMultiplier; } }
    [SerializeField] float speedMultiplier;

    public float MouseSensitivity { get { return mouseSensitivity; } }
    [SerializeField] float mouseSensitivity;

    #endregion

    #region drag

    //IsGrounded is a property so that it doesn't show in the inspector
    public bool IsGrounded { get; set; }

    public float GroundDrag { get { return groundDrag; } }
    [SerializeField] float groundDrag;

    public float PlayerHeight { get { return playerHeight; } }
    [SerializeField] float playerHeight;

    public LayerMask GroundLayerMask { get { return groundLayerMask; } }
    [SerializeField] LayerMask groundLayerMask;

    #endregion

    #region jump

    public float InAirSpeedMultiplier { get { return inAirSpeedMultiplier; } }
    [SerializeField] float inAirSpeedMultiplier;

    public float JumpForce { get { return jumpForce; } }
    [SerializeField] float jumpForce;

    #endregion



    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();

        stateMachine = new PlayerStateMachine();
        walkState = new PlayerWalkState(this, stateMachine);
        aerialState = new PlayerInAirState(this, stateMachine);
        stateMachine.Initialize(walkState);

        InputHandler = new InputHandler(this);
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerMap.Enable();
        playerInputActions.PlayerMap.JumpAction.performed += InputHandler.JumpAction_performed;
        playerInputActions.PlayerMap.MovementAction.performed += InputHandler.MovementAction_performed;
        playerInputActions.PlayerMap.CameraMovementAction.performed += InputHandler.CameraMovementAction_performed;

        
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
        SpeedControl();
    }
    

    void SpeedControl()
    {
        if(Rb.velocity.magnitude > Speed)
        {
            Vector3 newVelocity = Rb.velocity.normalized * Speed;
            Rb.velocity = new Vector3(newVelocity.x, Rb.velocity.y, newVelocity.z);
        }
    }
}