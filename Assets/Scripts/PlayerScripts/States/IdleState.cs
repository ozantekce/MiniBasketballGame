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



        if (playerController.OwnerOfBall())
        {
            playerController.ChangeCurrentState(playerController.IdleWithBallState);
        }
        else if (playerController.JumpInputDown())
        {
            playerController.ChangeCurrentState(playerController.JumpState);

        }
        else if (playerController.MovementInput())
        {
            playerController.ChangeCurrentState(playerController.RunState);
        }


    }

    public override void Exit(StateMachine stateMachine)
    {
        
    }



}
