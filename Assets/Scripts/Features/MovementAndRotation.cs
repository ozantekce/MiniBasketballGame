using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAndRotation : MonoBehaviour
{


    public static void MoveForward(Rigidbody rigidbody,float movementSpeed)
    {

        Vector3 directionVector
            = new Vector3(rigidbody.gameObject.transform.forward.x, 0, rigidbody.gameObject.transform.forward.z).normalized;

        directionVector *= movementSpeed;

        directionVector.y = rigidbody.velocity.y;

        rigidbody.velocity = directionVector;       

    }


    public static void Move(Rigidbody rigidbody,float movementSpeed, Vector3 direction)
    {

        Vector3 directionVector
            = direction.normalized;

        directionVector *= movementSpeed;
        
        directionVector.y = rigidbody.velocity.y;

        rigidbody.velocity = directionVector;

    }

    public static void MovePosition(Rigidbody rigidbody, Vector3 targetPosition, float speed_)
    {

        float distanceWithTargetPosition = Vector3.Distance(targetPosition, rigidbody.gameObject.transform.position);

        Vector3 directionVector = (targetPosition - rigidbody.gameObject.transform.position).normalized;

        float mag = speed_ * CONSTANTS.Linear(distanceWithTargetPosition,0,50f);

        directionVector *= mag;

        directionVector.y = rigidbody.velocity.y;

        rigidbody.velocity = directionVector;

    }



    public static void RotateAndMoveForward(Rigidbody rigidbody,float movementSpeed,float rotateSpeed, Vector3 direction)
    {

        RotateInYAxis(rigidbody.gameObject, direction, rotateSpeed);
        if (direction.x != 0 || direction.z != 0)
            MoveForward(rigidbody,movementSpeed);

    }


    public static void RotateToObject(GameObject gameObject, GameObject target,float rotateSpeed)
    {
        Vector3 directionVector = VectorCalculater.CalculateDirectionVectorWithoutYAxis(
        gameObject.transform.position, target.transform.position
        );

        RotateInYAxis(gameObject, directionVector,rotateSpeed);

    }


    private static void RotateInYAxis(GameObject gameObject,Vector3 targetForward,float rotateSpeed)
    {

        Vector2 curretForward = VectorCalculater.ThreeDForwardToTwoDForward(gameObject.transform.forward);
        Vector2 target = VectorCalculater.ThreeDForwardToTwoDForward(targetForward);

        float angleBetweenVectors = Vector2.SignedAngle(curretForward, target);

        if (Mathf.Abs(angleBetweenVectors) < 5f)
        {
            return;
        }

        float deltaAngle = rotateSpeed * Time.deltaTime * ((angleBetweenVectors < 0) ? +1 : -1);

        if(Mathf.Abs(deltaAngle) >= Mathf.Abs(angleBetweenVectors))
        {
            deltaAngle = angleBetweenVectors*Time.deltaTime;
        }



        Vector3 eulerAng = gameObject.transform.eulerAngles;
        eulerAng.y += deltaAngle;
        gameObject.transform.eulerAngles = eulerAng;

    }










}
