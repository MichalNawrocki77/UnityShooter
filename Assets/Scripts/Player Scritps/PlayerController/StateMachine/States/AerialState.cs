using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : State
{
    MovementController movementController;
    public AerialState(MovementController movementController,
                            StateMachine playerStateMachine) : 
                            base(playerStateMachine)
    {
        this.movementController = movementController;
    }

    public override void Enter()
    {
        movementController.RbToMove.drag = movementController.AerialDrag;
    }

    public override void Exit()
    {
        
    }

    public override void LogicUpdate()
    {
        
    }
    public override void PhysicsUpdate()
    {        
        movementController.AerialMovement();
        CheckForStateChange();
    }
    public void CheckForStateChange()
    {
        if (movementController.IsGrounded)
        {
            movementController.GroundedStateMachine.ChangeState(movementController.GroundedState);
        }
    }
}
