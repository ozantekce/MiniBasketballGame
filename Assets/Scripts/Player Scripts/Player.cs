using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    private VisionArea vision;
    private Rigidbody rigidbody;

    [SerializeField]
    private float movementSpeed, rotationSpeed, jumpHeight;

    private void Start()
    {
        
        vision = GetComponent<VisionArea>();
        rigidbody = GetComponent<Rigidbody>();



    }
    
    private void Update()
    {

        if (vision.visibleTargets.Count > 0 && Ball.Instance.BallStatus != BallStatus.throwByPlayer)
        {
            Ball.Instance.ChangeOwnerRequest(gameObject);
        }

        if (Ball.Instance.Owner == gameObject && Ball.Instance.BallStatus == BallStatus.holdingByPlayer)
        {
            Rotate();
            Throw();
        }
        else
        {
            Movement();
            Rotate();
            Jump();
            if(Ball.Instance.Owner == gameObject)
                Throw();
        }
            
        
        
        
    }

    private void Movement()
    {
        Vector3 directionVector = GetMovementInput();
        Vector3 velocityVector = directionVector;
        velocityVector *= movementSpeed;
        velocityVector.y = rigidbody.velocity.y;

        rigidbody.velocity = velocityVector;



    }

    private void Rotate()
    {
        Vector3 directionVector = GetMovementInput();
        if (directionVector != Vector3.zero)
        {
            transform.rotation
            = Quaternion.Slerp(transform.rotation
            , Quaternion.LookRotation(directionVector, Vector3.up)
            , Time.deltaTime * rotationSpeed);
        }
    }

    private bool jumpCompleted = true;
    private void Jump()
    {

        if (jumpCompleted&&JumpInput())
        {
            if(Ball.Instance.Owner == gameObject)
            {
                Ball.Instance.OnOwnerJumped();
            }
            
            jumpCompleted = false;
            rigidbody.useGravity = false;
            transform.DOMoveY(jumpHeight, 0.7f).OnComplete(delegate {
                transform.DOMoveY(1.45f, 0.7f).OnComplete(delegate
                {
                    rigidbody.useGravity = true;
                    jumpCompleted = true;
                }).SetEase(Ease.InSine); ;
            }).SetEase(Ease.OutSine);
        }

    }


    private bool shotCompleted = true;
    private void Throw()
    {
        
        if (shotCompleted && ThrowInputDown())
        {
            StartCoroutine(ThrowRoutine());
        }

    }

    private IEnumerator ThrowRoutine()
    {

        shotCompleted = false;
        OpenShotScrollBar();
        Ball.Instance.OnOwnerStartShot();
        //yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => { return ThrowInputUp();});
        CloseShotScrollBar();


        Ball ball = Ball.Instance;

        Vector3 targetPosition = ShotTargetPosition();

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

        ball.OnOwnerEndShot(new Vector3(Vx, Vy, Vz));
        yield return new WaitForSeconds(1f);
        shotCompleted = true;


    }



    [SerializeField]
    private Transform rivalHoop, throwTarget;
    [SerializeField]
    private ShotScrollBar shotScrollBar;
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

        Vector3 pos = throwTarget.position;

        pos = Deformation.Deform(pos, val * 100, val * 100);

        return pos;

    }

    private bool ThrowInputDown()
    {
        return Input.GetKeyDown(KeyCode.J);
    }
    private bool ThrowInputUp()
    {
        return Input.GetKeyUp(KeyCode.J);
    }
    private bool JumpInput()
    {
        return Input.GetKeyUp(KeyCode.Space);
    }

    private Vector3 GetMovementInput()
    {
        Vector3 movementInput;
        movementInput = Vector3.forward * Input.GetAxisRaw("Horizontal");
        movementInput += Vector3.left * Input.GetAxisRaw("Vertical");
        movementInput.Normalize();
        return movementInput;
    }


}
