using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerMovementState
{
    // Sets the state machine for this state, ensuring it is the same as the base state
    public PlayerJumpState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }


    // Called once at the start of the state, after ending the previous state
    public override void Enter()
    { 

    }

    // Called continously throughout the state (update)
    public override void Tick()
    {
       /* if (checkGrounded())
        {
            stateMachine.switchToMoveState();
        }*/
    }

    // Called once at the end of the state, before starting the next state
    public override void Exit()
    {

    }
}
