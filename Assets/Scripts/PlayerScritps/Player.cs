using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    public Rigidbody Rb { get; private set; }
    public Collider Collider { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public PlayerInputActions PlayerInputActions { get; private set; }

    #region StateMachine

    //These properties don't have setters so that I can't change them in other scripts, only here (where I only initialize them)
    //The public getter is required so that I can change player's current state from state's script (every state has "CheckForStateChangeFucntion()" where I change the player's state) using instances of states initialized here

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerInAirState AerialState { get; private set; }

    #endregion

    #region Mouse and Movement
    //[field: SerializeField] allows me to see and set the property in the inspector whilst keeping the functionality of a property (which in this instance, I use to make the setter private)
    [field: SerializeField] public Transform CameraHolder { get; private set; }

    [field: SerializeField] public float MaxSpeed { get; private set; }

    [field: SerializeField] public float Speed { get; private set; }

    [field: SerializeField] public float MouseSensitivity { get; private set; }

    #endregion

    #region drag

    //IsGrounded is a property so that it doesn't show in the inspector
    public bool IsGrounded { get; private set; }

    //[field: SerializeField] allows me to see and set the property in the inspector whilst keeping the functionality of a property (which in this instance, I use to make the setter private)
    [field: SerializeField] public float GroundDrag { get; private set; }

    [field: SerializeField] public float PlayerHeight { get; private set; }

    [field: SerializeField] public LayerMask GroundLayerMask { get; private set; }

    #endregion

    #region jump

    [field: SerializeField] public float InAirSpeed { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }

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

        StateMachine = new PlayerStateMachine();
        WalkState = new PlayerWalkState(this, StateMachine);
        AerialState = new PlayerInAirState(this, StateMachine);
    }
    private void Start()
    {
        PlayerInputActions.PlayerMap.Enable();
        PlayerInputActions.PlayerMap.MovementAction.performed += InputHandler.MovementAction_performed;
        PlayerInputActions.PlayerMap.CameraMovementAction.performed += InputHandler.CameraMovementAction_performed;
        PlayerInputActions.PlayerMap.JumpAction.performed += InputHandler.JumpAction_performed;

        StateMachine.Initialize(WalkState);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Debug.LogError("Implement animations control inside Player script i.e. modify Animator's VelocityVertical and VelocityHorizontal via code, by normalizing velocity.x and velocity.z and pass them to Animator.SetFloat()");
    }
    private void FixedUpdate()
    {
        CheckForGround();
        StateMachine.currentState.PhysicsUpdate();
        SpeedControl();
        Debug.Log(Rb.velocity.magnitude);
    }
    private void LateUpdate()
    {
        StateMachine.currentState.LateLogicUpdate();
    }
    public void CheckForGround()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, GroundLayerMask);
    }

    void SpeedControl()
    {
        //velocity without Y, because I only want to controll the speed of walking, not falling. Not excluding Y value, would interfere in gravity cause unnatural and unwanted behaviour.
        Vector3 velocityXZ = new Vector3(Rb.velocity.x,0,Rb.velocity.z);
        if (velocityXZ.magnitude > MaxSpeed)
        {
            Vector3 newVelocity = velocityXZ.normalized * MaxSpeed;
            Rb.velocity = new Vector3(newVelocity.x, Rb.velocity.y, newVelocity.z);
        }
    }
    public void UpdateAnimatorMovementFields()
    {
        Vector3 localVelocity =  transform.InverseTransformDirection(Rb.velocity);
        //if(localVelocity.magnitude>4) 
        //{
        //}
        //Fix a bug in RemapValue that makes it so it takes values outside the initial range, and still remaps them. Also there's a bug where negative values are mapped incorrectly in this instance (try to debug when the character is moving back and see the results compared to when the characters walks forward
        float velocityVerticalValue = RemapValue(localVelocity.z, 0, MaxSpeed, 0, 1);
        float velocityHorizontalValue = RemapValue(localVelocity.x, 0, MaxSpeed, 0, 1);

        animator.SetFloat(velocityVerticalString, velocityVerticalValue);
        animator.SetFloat(velocityHorizontalString, velocityHorizontalValue);
    }
    /// <summary>
    /// Remaps a value form range (oldMin, oldMax) to it's corresponding value in a new range (newMin,newMax). Basically works like mathf.InvLerp(), but instead of remaping to a range (0,1) you can specify the range you want your number to be remaped to.
    /// </summary>    ///
    /// <param name="num">The value you wish to remap</param>
    /// <param name="oldMin">Minimum value of num's previous range</param>
    /// <param name="oldMax">Maximum value of num's previous range</param>
    /// <param name="newMin"></param>
    /// <param name="newMax"></param>
    /// <returns></returns>
    private float RemapValue(float num, float oldMin, float oldMax, float newMin, float newMax)
    {
        //I created this function to be able to remap player's velocity to a
        return (num-oldMin)/(oldMax-oldMin) * (newMax-newMin) + newMin;
    }
    
}