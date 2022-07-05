using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{


    [SerializeField]
    private float _movSpeed,_rotateSpeed, _throwPower, _jumpPower;


    public Transform middleOfHands;
    public GameObject hand;
    public GameObject ground;

    private Rigidbody rigidbody;





    protected void Start()
    {

        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        middleOfHands = transform.Find("Body/Arms/RightShoulder/MidOfHands");

        hand = transform.Find("Body/Arms/RightShoulder/Hand").gameObject;
        ground = GameObject.Find("Ground");

    }

    protected void Update()
    {
        
    }


    public void Move(MovementType type,Vector3 vector)
    {
        if(type == MovementType.moveToDirection)
        {
            MoveToDirection(vector);
        }
        else if(type == MovementType.moveToPosition)
        {
            MoveToPosition(vector);
        }

    } 

    private void MoveToPosition(Vector3 vector)
    {
        MovementAndRotation.MovePosition(rigidbody, vector, _movSpeed);

    }
    private void MoveToDirection(Vector3 vector)
    {
       
        MovementAndRotation.RotateAndMoveForward(rigidbody, _movSpeed, _rotateSpeed, vector);
    }


    public void Jump()
    {
        if (!jumping)
        {
            StartCoroutine(JumpRoutine());
        }
    }


    public void HoldTheBall()
    {
        Ball ball = Ball.Instance;

        Rigidbody rb = ball.Rigidbody;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        ball.transform.position = middleOfHands.position;

    }


    private float targetY;
    public void Dribbling()
    {
        Ball ball = Ball.Instance;
        Vector3 ballPos = ball.transform.position;

        ballPos.x = hand.transform.position.x;
        ballPos.z = hand.transform.position.z;

        float deltaY = (targetY - ballPos.y) > 0 ? +1 : -1;
        ballPos.y += deltaY * Time.deltaTime;

        if (ballPos.y == hand.transform.position.y)
        {
            targetY = ground.transform.position.y;
        }
        else if (ballPos.y <= ground.transform.position.y + 1.2f)
        {
            targetY = hand.transform.position.y;
        }

        ball.transform.position = ballPos;

    }

    public void ThrowBall(Vector3 target)
    {
        if (!throwing)
        {
            StartCoroutine(ThrowRoutine(target));
        }
    }


    public void Block()
    {


    }




    public bool jumpOver;
    private bool jumping;
    private IEnumerator JumpRoutine()
    {
        jumping = true;

        rigidbody.useGravity = false;

        float firstY = transform.position.y;
        float targetY = transform.position.y + _jumpPower;
        float elapsedTime = 0;
        float duration = 0.66f;
        while (duration > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(firstY, targetY, elapsedTime / duration);
            //Debug.Log("y : " + pos.y);
            transform.position = pos;
            yield return new WaitForEndOfFrame();
        }

        firstY = transform.position.y;
        targetY = transform.position.y - _jumpPower;
        elapsedTime = 0;
        duration = 0.66f;
        while (duration > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(firstY, targetY, elapsedTime / duration);
            //Debug.Log("y : " + pos.y);
            transform.position = pos;
            yield return new WaitForEndOfFrame();
        }
        rigidbody.useGravity = true;
        jumping = false;
        jumpOver = true;
    }


    public bool throwOver;
    private bool throwing;

    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }

    private IEnumerator ThrowRoutine(Vector3 targetPosition)
    {
        throwing = true;
        Ball ball = Ball.Instance;

        Rigidbody rigidbody = ball.Rigidbody;

        float distance = Vector3.Distance(targetPosition, ball.transform.position);

        float t = 3f * CONSTANTS.Linear(distance, 0, 7.5f);//  Time
        float g = 9.8f;
        float Dx, Dy, Dz;   //  Distance
        float Vx, Vy, Vz;   //  required velocity

        Dx = targetPosition.x - ball.transform.position.x;
        Dy = targetPosition.y - ball.transform.position.y;
        Dz = targetPosition.z - ball.transform.position.z;


        Vx = Dx / t;
        Vy = (Dy / t) + ((g * t) / 2); // V(0) = (d/t) + ((g*t)/2)
        Vz = Dz / t;


        rigidbody.velocity = new Vector3(Vx, Vy, Vz);
        float elapsedTime = 0;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            float dis = Vector3.Distance(transform.position, targetPosition);
            if (dis < 2f || elapsedTime > t)
            {
                break;
            }
            yield return new WaitForEndOfFrame();

        }

        throwOver = true;
        throwing = false;

    }



}

public enum MovementType
{
    moveToPosition,moveToDirection
}
