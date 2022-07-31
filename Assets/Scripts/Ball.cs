using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{

    private static Ball instance;

    private void Awake()
    {
        instance = this;
    }


    [SerializeField]
    private GameObject owner;

    private Rigidbody rigidbody;
    [SerializeField]
    private BallStatus ballStatus;

    private void Start()
    {
        ballStatus = BallStatus.free;
        rigidbody = GetComponent<Rigidbody>();

    }


    private bool ballStatusChanged = false;

    private void OnValidate()
    {
        ballStatusChanged = true;
    }

    private void Update()
    {
        elapsedTimeChangeOwner+=Time.deltaTime;
        if (ballStatusChanged)
        {
            OnStatusChanged();
            ballStatusChanged = false;
        }

        if (ballStatus == BallStatus.free)
        {
            ExecuteFreeStatus();
        }
        else if (ballStatus == BallStatus.dribblingByPlayer)
        {
            ExecuteDribblingStatus();
        }
        else if (ballStatus == BallStatus.holdingByPlayer)
        {
            ExecuteHoldingStatus();
        }
        else if (ballStatus == BallStatus.throwByPlayer)
        {
            ExecuteThrowStatus();
        }

    }

    private void OnStatusChanged()
    {
        if(ballStatus == BallStatus.free)
        {
            rigidbody.isKinematic = false;
            rigidbody.constraints = RigidbodyConstraints.None;
            if (dribblingTweener!=null &&dribblingTweener.active)
            {
                dribblingTweener.Kill();
            }
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        else if(ballStatus == BallStatus.dribblingByPlayer)
        {
            dribblingComplate = true;
            rigidbody.isKinematic=true;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        else if(ballStatus == BallStatus.holdingByPlayer)
        {
            rigidbody.isKinematic=true;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            ownerMidOfHands = owner.transform.Find("Body/Arms/RightShoulder/MidOfHands");
            if (dribblingTweener != null && dribblingTweener.active)
            {
                dribblingTweener.Kill();
            }
        }
        else if(ballStatus == BallStatus.throwByPlayer)
        {
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            if (dribblingTweener != null && dribblingTweener.active)
            {
                dribblingTweener.Kill();
            }
        }



    }



    private void ExecuteFreeStatus()
    {


    }

    private bool dribblingComplate =true;
    private Tweener dribblingTweener;
    private void ExecuteDribblingStatus()
    {
        float groundY = 0.7f;
        float handY = 1.5f;

        Vector3 trimming = owner.transform.forward * 0.7f;
        trimming = Quaternion.Euler(0, 23.66f, 0) * trimming;

        transform.position = new Vector3(
            owner.transform.position.x+trimming.x
            , transform.position.y
            , owner.transform.position.z +trimming.z
            );

        if (dribblingComplate)
        {
            dribblingComplate=false;
            dribblingTweener = transform.DOMoveY(groundY, 0.7f).OnComplete(delegate {
                transform.DOMoveY(handY, 0.7f).OnComplete(delegate
                {
                    dribblingComplate = true;
                });
            });
        }



    }


    private Transform ownerMidOfHands;
    private void ExecuteHoldingStatus()
    {
        transform.position = ownerMidOfHands.position;
    }

    public Vector3 throwVector;
    private bool throwingRoutineRunning;
    private void ExecuteThrowStatus()
    {

        if (!throwingRoutineRunning)
        {
            StartCoroutine(ThrowRoutine(throwVector));
        }

    }

    private IEnumerator ThrowRoutine(Vector3 vector)
    {
        rigidbody.isKinematic = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        yield return new WaitForFixedUpdate();
        rigidbody.velocity = vector;
        // farkli bir seyler eklenebilir
        yield return new WaitForSeconds(1f);
        
        ballStatus = BallStatus.free;
        ballStatusChanged = true;

    }


    private float minTimeToChangeOwner = 2f;
    private float elapsedTimeChangeOwner;
    public void ChangeOwnerRequest(GameObject source)
    {

        if (source == owner)
            return;
        else if(minTimeToChangeOwner<=elapsedTimeChangeOwner)
        {
            elapsedTimeChangeOwner = 0;
            owner = source;
            ballStatus = BallStatus.dribblingByPlayer;
            ballStatusChanged = true;
            if (dribblingTweener!=null && dribblingTweener.active)
            {
                dribblingTweener.Kill();
            }
        }
    }



    #region GetterSetter
    public static Ball Instance { get => instance; set => instance = value; }
    public BallStatus BallStatus { get => ballStatus; set => ballStatus = value; }

    #endregion





}
public enum BallStatus
{
    free, dribblingByPlayer, holdingByPlayer, throwByPlayer
}
