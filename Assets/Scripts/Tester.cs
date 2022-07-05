using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{


    private Rigidbody rigidbody;
    void Start()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //rigidbody.useGravity = false;


    }

    void Update()
    {

        float up_down;
        float left_right;
        float forward_backward;

        up_down = Input.GetAxis("Jump");
        left_right = Input.GetAxis("Horizontal");
        forward_backward = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(left_right,up_down,forward_backward);

        MovementAndRotation.RotateAndMoveForward(rigidbody, 2f,500f, direction);


    }



}
