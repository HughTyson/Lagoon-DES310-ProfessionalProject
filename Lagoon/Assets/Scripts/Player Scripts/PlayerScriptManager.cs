using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BaseState : MonoBehaviour
{
    
    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }

    public virtual void StateLateUpdate() { }

    public virtual void StateOnTriggerEnter(Collider other) { }
    public virtual void StateOnTriggerStay(Collider other) { }
    public virtual void StateOnTriggerExit(Collider other) { }

}

public class PlayerScriptManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] BaseState currentState;

    [SerializeField] PlayerFishing stateFishing;

    public enum STATE
    { 
        EXPLORING,
        FISHING    
    };


    void Start()
    {
        currentState.enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        currentState.StateUpdate();
    }

    private void FixedUpdate()
    {
        currentState.StateFixedUpdate();
    }

    private void LateUpdate()
    {
        currentState.StateLateUpdate();
    }

    public void ChangeState(STATE newState)
    {
        currentState.enabled = false;

        switch (newState)
        {
            case STATE.EXPLORING:
                {

                    break;
                }
            case STATE.FISHING:
                {
                    currentState = stateFishing;
                    break;
                }
        
        }

        currentState.enabled = true;
    }

    public void StateOnTriggerEnter(Collider other)
    {
        currentState.StateOnTriggerEnter(other);
    }
    public void StateOnTriggerStay(Collider other)
    {
        currentState.StateOnTriggerStay(other);


    }
    public void StateOnTriggerExit( Collider other)
    {
        currentState.StateOnTriggerExit( other);
    }

}



