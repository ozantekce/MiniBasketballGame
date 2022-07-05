using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpdateType
{
    normalUpdate,fixedUpdate
}
public class StateMachine : MonoBehaviour
{

    [SerializeField]
    private UpdateType _updateType;

    public State initialState;

    private State currentState;

    // Update is called once per frame
    public void Update()
    {
        if (UpdateType.fixedUpdate == _updateType)
            return;

        if(currentState == null)
        {
            currentState = initialState;
        }
        else
        {
            currentState.Execute(this);
        }

    }

    public void FixedUpdate()
    {
        if (UpdateType.normalUpdate == _updateType)
            return;

        if (currentState == null)
        {
            currentState = initialState;
        }
        else
        {
            currentState.Execute(this);
        }

    }



    public void ChangeCurrentState(State state)
    {
        currentState.Exit(this);
        currentState = state;
        currentState.Enter(this);
    }




}
