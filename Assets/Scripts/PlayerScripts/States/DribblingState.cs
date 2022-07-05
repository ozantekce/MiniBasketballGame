using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DribblingState : State
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
        else if (!playerController.MovementInput())
        {
            playerController.ChangeCurrentState(playerController.IdleWithBallState);
        }


        playerController.Player.Dribbling();
        playerController.Player.Move(MovementType.moveToDirection, playerController.movementInput);


    }

    public override void Exit(StateMachine stateMachine)
    {


    }
}
