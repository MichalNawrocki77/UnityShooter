using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        player.Rb.drag = player.GroundDrag;
        player.PlayerInputActions.PlayerMap.JumpAction.Enable();
    }

    public override void Exit()
    {
        player.PlayerInputActions.PlayerMap.JumpAction.Disable();
    }

    public override void LogicUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        CheckForStateChange();
        player.InputHandler.PlayerMovementOnGround();
        player.InputHandler.CameraRotation();
        player.UpdateAnimatorMovementFields();
    }
    void CheckForStateChange()
    {
        if (player.IsGrounded)
        {
            player.StateMachine.ChangeState(player.AerialState);
        }
    }
}
