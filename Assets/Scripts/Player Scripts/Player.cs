using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    private Vision vision;
    private Rigidbody rigidbody;

    [SerializeField]
    private float movementSpeed, rotationSpeed;

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

        Movement();
        
    }





    private void Movement()
    {
        Vector3 directionVector = GetMovementInput();
        directionVector *= movementSpeed;
        directionVector.y = rigidbody.velocity.y;

        rigidbody.velocity = directionVector;
        if (directionVector != Vector3.zero)
        {
            transform.rotation
            = Quaternion.Slerp(transform.rotation
            , Quaternion.LookRotation(directionVector, Vector3.up)
            , Time.deltaTime * rotationSpeed);
        }


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
