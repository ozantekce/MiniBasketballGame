using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{




    public override void Enter(StateMachine stateMachine)
    {
        
    }

    public override void Execute(StateMachine stateMachine)
    {
        PlayerController playerController = (PlayerController)stateMachine;



        if (playerController.MovementInput())
        {
            //playerController.ChangeCurrentState(RunState);
        }

    }

    public override void Exit(StateMachine stateMachine)
    {
        
    }



}
