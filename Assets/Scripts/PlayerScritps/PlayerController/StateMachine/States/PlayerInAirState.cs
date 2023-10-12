using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

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
    public override void LateLogicUpdate()
    {
        player.InputHandler.CameraRotation();
    }
    public override void PhysicsUpdate()
    {
        CheckForStateChange();
        player.InputHandler.PlayerMovementInAir();             
    }
    public void CheckForStateChange()
    {
        if(player.IsGrounded)
        {
            player.StateMachine.ChangeState(player.WalkState);
        }        
    }
}
