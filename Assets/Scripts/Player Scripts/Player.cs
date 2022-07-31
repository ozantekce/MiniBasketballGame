using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    private Vision vision;
    private Rigidbody rigidbody;

    [SerializeField]
    private float movementSpeed, rotationSpeed, jumpHeight;

    private void Start()
    {
        
        vision = GetComponent<Vision>();
        rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {

        if (vision.visibleTargets.Count > 0)
        {
            Ball.Instance.ChangeOwnerRequest(gameObject);
        }

        if (Ball.Instance.Owner == gameObject && Ball.Instance.BallStatus == BallStatus.holdingByPlayer)
        {
            Rotate();
        }
        else
        {
            Movement();
            Rotate();
            Jump();
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
