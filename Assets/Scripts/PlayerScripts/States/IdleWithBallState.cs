using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleWithBallState : State
{
    public override void Enter(StateMachine stateMachine)
    {

    }

    public override void Execute(StateMachine stateMachine)
    {

        PlayerController playerController = (PlayerController)stateMachine;

        if (!playerController.OwnerOfBall())
        {
            playerController.ChangeCurrentState(playerController.IdleState);
        }
        else if (playerController.ShotInputDown())
        {
            playerController.ChangeCurrentState(playerController.ShotState);
        }
        else if (playerController.JumpInputDown())
        {
            playerController.ChangeCurrentState(playerController.JumpState);
        }
        else if (playerController.MovementInput())
        {
            playerController.ChangeCurrentState(playerController.DribblingState);
        }


        playerController.Player.Dribbling();
    }

    public override void Exit(StateMachine stateMachine)
    {


    }


}
