using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BaseState : MonoBehaviour
{
    public void SetStateManagerPointer(PlayerScriptManager manager)
    {
        StateManager = manager;
    }


    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }

    public virtual void StateLateUpdate() { }


    public virtual void StateOnTriggerEnter(Collider other) { }
    public virtual void StateOnTriggerStay(Collider other) { }
    public virtual void StateOnTriggerExit(Collider other) { }

    protected PlayerScriptManager StateManager;
}

public class PlayerScriptManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] BaseState currentState;

    [SerializeField] PlayerFishingState stateFishing;
    [SerializeField] PlayerExploreState stateExplore;
    [SerializeField] PlayerConversationState stateConverstation;
    [SerializeField] PlayerRepairState stateRepair;

    public enum STATE
    { 
        EXPLORING,
        FISHING,
        CONVERSATION,
        REPAIR
    };


    void Start()
    {
        currentState.enabled = true;

        stateFishing.SetStateManagerPointer(this);
        stateExplore.SetStateManagerPointer(this);
        stateConverstation.SetStateManagerPointer(this);
        stateRepair.SetStateManagerPointer(this);

        stateFishing.enabled = false;
        stateExplore.enabled = false;
        stateConverstation.enabled = false;
        stateRepair.enabled = false;

        stateFishing.OnDisable();
        stateConverstation.OnDisable();
        stateRepair.OnDisable();

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
                    currentState = stateExplore;
                    break;
                }
            case STATE.FISHING:
                {
                    currentState = stateFishing;
                    break;
                }
            case STATE.CONVERSATION:
                {
                    currentState = stateConverstation;
                    break;
                }
            case STATE.REPAIR:
                {
                    currentState = stateRepair;
                    break;
                }
        }

        Debug.Log(newState);
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



