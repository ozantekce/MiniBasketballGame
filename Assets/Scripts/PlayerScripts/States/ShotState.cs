using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotState : State
{

    private enum SubStates
    {
        readying,shotting,wait,exit
    }

    private SubStates subState;
    public override void Enter(StateMachine stateMachine)
    {
        subState = SubStates.readying;

        PlayerController playerController = (PlayerController)stateMachine;

        playerController.OpenShotScrollBar();

        waitTime = 1f;

    }

    private float waitTime;

    public override void Execute(StateMachine stateMachine)
    {

        PlayerController playerController = (PlayerController)stateMachine;

        /*
        if (!playerController.OwnerOfBall())
        {
            playerController.ChangeCurrentState(playerController.IdleState);
        }
        else if (!playerController.MovementInput())
        {
            playerController.ChangeCurrentState(playerController.IdleWithBallState);
        }*/


        if (playerController.ShotInputUp())
        {
            subState = SubStates.shotting;
        }
        
        if (subState == SubStates.readying)
        {

            playerController.SendBallToMidOfHands();
            if (playerController.ShotInputUp())
            {
                subState = SubStates.shotting;
            }
        }
        else if(subState == SubStates.shotting)
        {
            playerController.CloseShotScrollBar();
            Ball.Instance.ResetOwner();
            playerController.Player.ThrowBall(playerController.ShotTargetPosition());
            subState = SubStates.wait;    
        }
        else if(subState == SubStates.wait)
        {
            waitTime -= Time.deltaTime;
            if(waitTime <= 0f)
            {
                subState = SubStates.exit;
            }
        }
        else if(subState == SubStates.exit)
        {
            playerController.ChangeCurrentState(playerController.IdleState);
        }

    }

    public override void Exit(StateMachine stateMachine)
    {
        Debug.Log("exit");
    }







}
