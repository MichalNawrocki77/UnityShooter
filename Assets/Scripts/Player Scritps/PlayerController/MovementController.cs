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

	[field: SerializeField] public float maxSpeed { get; private set; }

    [field: SerializeField] public float jumpForce { get; private set; }

    public Vector3 CurrentXZVelocity { get; private set; }


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
        Input.OnJumpPressed += Jump;

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
		CurrentXZVelocity = new Vector3(RbToMove.velocity.x,0f,RbToMove.velocity.z);
        if(CurrentXZVelocity.magnitude > maxSpeed)
        {
			CurrentXZVelocity = CurrentXZVelocity.normalized * maxSpeed;
            RbToMove.velocity = new Vector3(CurrentXZVelocity.x, RbToMove.velocity.y, CurrentXZVelocity.z);
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
