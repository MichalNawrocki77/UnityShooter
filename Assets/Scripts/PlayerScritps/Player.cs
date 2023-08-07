using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    public  Rigidbody Rb { get; private set; }
    public Collider Collider { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public PlayerInputActions PlayerInputActions { get; private set; }

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

    public float MaxSpeed { get { return maxSpeed; } }
    [SerializeField] float maxSpeed;

    public float SpeedMultiplier { get { return speedMultiplier; } }
    [SerializeField] float speedMultiplier;

    public float MouseSensitivity { get { return mouseSensitivity; } }
    [SerializeField] float mouseSensitivity;

    #endregion

    #region drag

    //IsGrounded is a property so that it doesn't show in the inspector
    public bool IsGrounded { get; private set; }

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

    #region Animations

    Animator animator;
    string velocityHorizontalString = "VelocityHorizontal";
    string velocityVerticalString = "VelocityVertical";

    #endregion



    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        InputHandler = new InputHandler(this);
        PlayerInputActions = new PlayerInputActions();

        stateMachine = new PlayerStateMachine();
        walkState = new PlayerWalkState(this, stateMachine);
        aerialState = new PlayerInAirState(this, stateMachine);
    }
    private void Start()
    {
        PlayerInputActions.PlayerMap.Enable();
        PlayerInputActions.PlayerMap.MovementAction.performed += InputHandler.MovementAction_performed;
        PlayerInputActions.PlayerMap.CameraMovementAction.performed += InputHandler.CameraMovementAction_performed;
        PlayerInputActions.PlayerMap.JumpAction.performed += InputHandler.JumpAction_performed;

        stateMachine.Initialize(walkState);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Debug.LogError("Implement animations control inside Player script i.e. modify Animator's VelocityVertical and VelocityHorizontal via code, by normalizing velocity.x and velocity.z and pass them to Animator.SetFloat()");
    }
    private void FixedUpdate()
    {
        CheckForGround();
        stateMachine.currentState.PhysicsUpdate();
        SpeedControl();
    }
    public void CheckForGround()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, Collider.bounds.extents.y + 0.5f, GroundLayerMask);
    }

    void SpeedControl()
    {
        if (Rb.velocity.magnitude > MaxSpeed)
        {
            Vector3 newVelocity = Rb.velocity.normalized * MaxSpeed;
            Rb.velocity = new Vector3(newVelocity.x, Rb.velocity.y, newVelocity.z);
        }
    }
    public void UpdateAnimatorMovementFields()
    {
        Vector3 localVelocity =  transform.InverseTransformDirection(Rb.velocity);
        float velocityVerticalValue = LinearNormalization(localVelocity.z,0,maxSpeed,0,1);
        float velocityHorizontalValue = LinearNormalization(localVelocity.x,0,maxSpeed,0,1);

        animator.SetFloat(velocityVerticalString, velocityVerticalValue);
        animator.SetFloat(velocityHorizontalString, velocityHorizontalValue);
    }
    private float LinearNormalization(float num, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (num-oldMin)/(oldMax-oldMin) * (newMax-newMin) + newMin;
    }
    
}