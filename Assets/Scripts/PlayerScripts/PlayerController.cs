using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : StateMachine
{


    public Transform rivalHoop;

    private Player _player;
    public KeyCode jumpKey, shotKey, blockKey;
    public Vector3 movementInput;
    public bool jumpInput, shotInputDown, shotInputUp, blockInput;


    private IdleState _idleState;
    private RunState _runState;
    private DribblingState _dribblingState;
    private IdleWithBallState _idleWithBallState;
    private ShotState _shotState;
    private JumpState _jumpState;

    public IdleState IdleState { get => _idleState; set => _idleState = value; }
    public RunState RunState { get => _runState; set => _runState = value; }
    public DribblingState DribblingState { get => _dribblingState; set => _dribblingState = value; }
    public IdleWithBallState IdleWithBallState { get => _idleWithBallState; set => _idleWithBallState = value; }
    public ShotState ShotState { get => _shotState; set => _shotState = value; }
    public JumpState JumpState { get => _jumpState; set => _jumpState = value; }
    public Player Player { get => _player; set => _player = value; }

    void Start()
    {

        _idleState = new IdleState();
        _runState = new RunState();
        _dribblingState = new DribblingState();
        _idleWithBallState = new IdleWithBallState();
        _shotState = new ShotState();
        _jumpState = new JumpState();

        _player = GetComponent<Player>();
        initialState = _idleState;


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
        //jumpInput = Input.GetKey(jumpKey);
    }




    #region Conditions
    public bool MovementInput()
    {
        return movementInput != Vector3.zero;
    }

    public bool OwnerOfBall()
    {
        return Ball.Instance.IsOwner(this.gameObject);
    }

    public bool ShotInputDown()
    {
        return shotInputDown;
    }

    public bool ShotInputUp()
    {
        return shotInputUp;
    }

    public bool JumpInputDown()
    {
        return jumpInput;
    }



    #endregion



    public ShotScrollBar shotScrollBar;

    public void OpenShotScrollBar()
    {
        shotScrollBar.mul = 10 / Vector3.Distance(transform.position, rivalHoop.position);
        shotScrollBar.gameObject.SetActive(true);

    }

    public void CloseShotScrollBar()
    {
        shotScrollBar.gameObject.SetActive(false);
    }

    public Vector3 ShotTargetPosition()
    {

        float val = Mathf.Abs(shotScrollBar.value - 0.5f);

        Vector3 pos = rivalHoop.position;

        pos = Deformation.Deform(pos, val * 100, val * 100);

        return pos;

    }


    public void SendBallToMidOfHands()
    {
        Ball.Instance.transform.position = Player.middleOfHands.position;
    }


}


