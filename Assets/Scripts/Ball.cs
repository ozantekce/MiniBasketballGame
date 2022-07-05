using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Singleton
    private static Ball instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public static Ball Instance
    {
        get
        {
            return instance;
        }
    }

    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }

    private Rigidbody _rigidbody;



    private GameObject _owner;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _owner = collision.gameObject;
        }
    }



}
