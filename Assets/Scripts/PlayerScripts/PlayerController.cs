using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : StateMachine
{


    private Player player;
    public KeyCode jumpKey, shotKey, blockKey;
    public Vector3 movementInput;
    public bool jumpInput, shotInputDown, shotInputUp, blockInput;


    public static State IdleState;


    void Start()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        ReadInputs();        
        base.Update();
    }


    private void ReadInputs()
    {
        movementInput = Vector3.zero;
        movementInput = Vector3.forward * Input.GetAxisRaw("Horizontal");
        movementInput += Vector3.left * Input.GetAxisRaw("Vertical");
        movementInput.Normalize();


        blockInput = Input.GetKey(blockKey);
        shotInputDown = Input.GetKeyDown(shotKey);
        shotInputUp = Input.GetKeyUp(shotKey);
        jumpInput = Input.GetKey(jumpKey);
    }


    public bool MovementInput()
    {
        return movementInput != Vector3.zero;
    }




}


