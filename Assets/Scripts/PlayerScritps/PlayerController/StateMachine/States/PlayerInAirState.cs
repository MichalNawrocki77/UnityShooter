using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        player.Rb.drag = 0;
    }

    public override void Exit()
    {
        
    }

    public override void LogicUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        player.InputHandler.PlayerMovementInAir();
        player.InputHandler.CameraRotation();
        CheckIfGrounded();
        CheckForStateChange();
    }
    public void CheckIfGrounded()
    {
        player.IsGrounded = Physics.Raycast(player.transform.position, Vector3.down, player.PlayerHeight * 0.5f + 0.2f, player.GroundLayerMask);
    }
    public void CheckForStateChange()
    {
        if(player.IsGrounded)
        {
            player.StateMachine.ChangeState(player.WalkState);
        }        
    }
}
