using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : State
{
    MovementController movementController;
    public GroundedState(MovementController movementController,
                            StateMachine playerStateMachine) :
                            base(playerStateMachine)
    {
        this.movementController = movementController;
    }

    public override void Enter()
    {
        movementController.RbToMove.drag = movementController.groundedDrag;
        movementController.Input.SetJumpActionActive(true);
    }

    public override void Exit()
    {
		movementController.Input.SetJumpActionActive(false);
	}

    public override void LogicUpdate()
    {
    }
    public override void LateLogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
        movementController.GroundMovement();
        CheckForStateChange();
    }
    void CheckForStateChange()
    {
        if (!movementController.IsGrounded)
        {
            movementController.GroundedStateMachine.ChangeState(movementController.AerialState);
        }
    }
}
