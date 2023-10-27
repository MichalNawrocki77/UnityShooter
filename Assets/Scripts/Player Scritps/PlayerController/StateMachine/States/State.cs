using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public StateMachine playerStateMachine { get; set; }
    public State(StateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
    public virtual void LateLogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
