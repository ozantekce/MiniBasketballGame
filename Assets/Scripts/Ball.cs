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
            MakeNonKinematic();
            if (dribblingTweener!=null &&dribblingTweener.active)
            {
                dribblingTweener.Kill(false);
            }
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        else if(ballStatus == BallStatus.dribblingByPlayer)
        {
            ownerHand = owner.transform.Find("Body/Arms/RightShoulder/Hand");
            dribblingCompleted = true;
            MakeKinematic();
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        }
        else if(ballStatus == BallStatus.holdingByPlayer)
        {
            MakeKinematic();
            ownerMidOfHands = owner.transform.Find("Body/Arms/RightShoulder/MidOfHands");
            if (dribblingTweener != null && dribblingTweener.active)
            {
                dribblingTweener.Kill(false);
            }
        }
        else if(ballStatus == BallStatus.throwByPlayer)
        {
            MakeNonKinematic();
            if (dribblingTweener != null && dribblingTweener.active)
            {
                dribblingTweener.Kill(false);
            }
        }



    }

    public void OnOwnerJumped()
    {
        ballStatus = BallStatus.holdingByPlayer;
        ballStatusChanged = true;
        MakeKinematic();
        ownerMidOfHands = owner.transform.Find("Body/Arms/RightShoulder/MidOfHands");
        transform.position = ownerMidOfHands.position;
        if (dribblingTweener != null && dribblingTweener.active)
        {
            dribblingTweener.Kill(false);
        }
    }

    public void OnOwnerStartShot()
    {
        ballStatus = BallStatus.holdingByPlayer;
        ballStatusChanged = true;
        MakeKinematic();
        ownerMidOfHands = owner.transform.Find("Body/Arms/RightShoulder/MidOfHands");
        transform.position = ownerMidOfHands.position;
        if (dribblingTweener != null && dribblingTweener.active)
        {
            dribblingTweener.Kill(false);
        }
    }

    public void OnOwnerEndShot(Vector3 velocity)
    {
                
        ballStatus = BallStatus.throwByPlayer;
        ballStatusChanged = true;
        throwVector = velocity;

    }


    private void ExecuteFreeStatus()
    {


    }

    private bool dribblingCompleted =true;
    private Tweener dribblingTweener;
    private Transform ownerHand;

    private void ExecuteDribblingStatus()
    {
        float groundY = 0.7f;
        float handY = ownerHand.position.y;

        Vector3 trimming = owner.transform.forward * 0.7f;
        trimming = Quaternion.Euler(0, 23.66f, 0) * trimming;

        transform.position = new Vector3(
            owner.transform.position.x+trimming.x
            , transform.position.y
            , owner.transform.position.z +trimming.z
            );

        if (dribblingCompleted)
        {
            dribblingCompleted=false;
            float sendHandTime = 0.7f * CONSTANTS.Linear(Mathf.Abs(handY-transform.position.y),0,1.6f);

            dribblingTweener = transform.DOMoveY(handY, sendHandTime).OnComplete(delegate {
                dribblingTweener = transform.DOMoveY(groundY, 0.7f).OnComplete(delegate
                {
                    dribblingCompleted = true;
                });
            });
        }



    }


    private Transform ownerMidOfHands;
    private void ExecuteHoldingStatus()
    {
        transform.position = ownerMidOfHands.position;
    }

    private Vector3 throwVector;
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
        throwingRoutineRunning = true;
        MakeKinematic();
        yield return new WaitForFixedUpdate();
        MakeNonKinematic();
        owner = null;
        rigidbody.velocity = vector;
        // farkli bir seyler eklenebilir
        yield return new WaitForSeconds(1f);
        owner = null;
        ballStatus = BallStatus.free;
        ballStatusChanged = true;
        throwingRoutineRunning = false;
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



    private void MakeKinematic()
    {

        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rigidbody.isKinematic=true;
    }
    private void MakeNonKinematic()
    {

        rigidbody.isKinematic = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

    }


    #region GetterSetter
    public static Ball Instance { get => instance; set => instance = value; }
    public BallStatus BallStatus { get => ballStatus; set => ballStatus = value; }
    public GameObject Owner { get => owner; }

    #endregion





}
public enum BallStatus
{
    free, dribblingByPlayer, holdingByPlayer, throwByPlayer
}
