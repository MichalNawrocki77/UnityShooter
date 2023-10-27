using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MovementController : MonoBehaviour
{
    public IInputProvider Input { get; private set; }

    public StateMachine GroundedStateMachine { get; private set; }
    public State GroundedState { get; private set; }
    public State AerialState { get; private set; }

    public Rigidbody RbToMove { get; private set; }

    public bool IsGrounded { get; private set; }
    [SerializeField] LayerMask GroundLayerMask;


    [SerializeField] private float AerialAcceleration;
    [SerializeField] private float groundedAcceleration;

    [field: SerializeField] public float groundedDrag { get; private set; }
    [field: SerializeField] public float AerialDrag { get; private set; }

    [SerializeField] private float maxSpeed;

    [field: SerializeField] public float jumpForce { get; private set; }

    private void Awake()
    {
        GroundedStateMachine = new StateMachine();
        GroundedState = new GroundedState(this, GroundedStateMachine);
        AerialState = new AerialState(this, GroundedStateMachine);        
    }
    void Start()
    {
        Input = GetComponent<IInputProvider>();
        if (Input is null)
        {
            Debug.Log("CameraController Component Could not find IInputProvider. Please attach a component that inherits from IInputProvider interface :)");
        }

        RbToMove = GetComponentInParent<Rigidbody>();
        if (RbToMove is null)
        {
            Debug.Log("CameraController could not find any rigidbody to move, please attach Rigidbody component on this GameObject first");
        }
        GroundedStateMachine.Initialize(GroundedState);
    }

    void FixedUpdate()
    {
        GroundedStateMachine.currentState.PhysicsUpdate();
        SpeedControl();
        GroundCheck();
    }
    public void GroundMovement()
    {
        RbToMove.AddForce(transform.forward * Input.MovementInput.y * groundedAcceleration
        +
                          transform.right * Input.MovementInput.x * groundedAcceleration,
                          ForceMode.Force);
    }
    public void AerialMovement()
    {
        RbToMove.AddForce(transform.forward * Input.MovementInput.y * AerialAcceleration
        +
                          transform.right * Input.MovementInput.x * AerialAcceleration,
                          ForceMode.Force);
    }
    void SpeedControl()
    {
        Vector3 forwardSideVelocity = new Vector3(RbToMove.velocity.x,0f,RbToMove.velocity.z);
        if(forwardSideVelocity.magnitude > maxSpeed)
        {
            forwardSideVelocity = forwardSideVelocity.normalized * maxSpeed;
            RbToMove.velocity = new Vector3(forwardSideVelocity.x, RbToMove.velocity.y, forwardSideVelocity.z);
        }
    }
    void GroundCheck()
    {
        IsGrounded = Physics.Raycast(transform.position -
                                     new Vector3(0, transform.localPosition.y-0.5f, 0),
                                     Vector3.down, 2f, GroundLayerMask);
    }
    public void Jump()
    {
        RbToMove.AddForce(Vector3.up * jumpForce);
    }
}
