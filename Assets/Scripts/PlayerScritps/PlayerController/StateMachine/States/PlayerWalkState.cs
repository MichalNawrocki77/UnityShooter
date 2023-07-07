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
    }

    public override void Exit()
    {
        
    }

    public override void LogicUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        player.InputHandler.PlayerMovementOnGround();
        player.InputHandler.CameraRotation();
        CheckIfGrounded();
        CheckForStateChange();
    }
    public void CheckIfGrounded()
    {
        player.IsGrounded = Physics.Raycast(player.transform.position, Vector3.down, player.PlayerHeight * 0.5f + 0.2f, player.GroundLayerMask);
    }
    void CheckForStateChange()
    {
        if (!player.IsGrounded)
        {
            player.StateMachine.ChangeState(player.AerialState);
        }
    }
    public void JumpAction_performed(InputAction.CallbackContext obj)
    {
        player.Rb.AddForce(Vector3.up * player.JumpForce, ForceMode.Impulse);
    }
}
