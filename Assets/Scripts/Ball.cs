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

        readyForNewOwner = new CooldownManualReset(1000f);
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


    private CooldownManualReset readyForNewOwner;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(readyForNewOwner.TimeOver())
                _owner = collision.gameObject;
        }
    }



    public bool IsOwner(GameObject gameObject)
    {
        return _owner == gameObject;
    }

    public void ResetOwner()
    {
        _owner = null;
        readyForNewOwner.ResetTimer();
    }

}
