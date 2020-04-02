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
    [SerializeField] PlayerSleepState stateSleep;

    public enum STATE
    { 
        EXPLORING,
        FISHING,
        CONVERSATION,
        REPAIR,
        SLEEP
    };


    void Start()
    {
        currentState.enabled = true;

        stateFishing.SetStateManagerPointer(this);
        stateExplore.SetStateManagerPointer(this);
        stateConverstation.SetStateManagerPointer(this);
        stateRepair.SetStateManagerPointer(this);
        stateSleep.SetStateManagerPointer(this);

        stateFishing.enabled = false;
        stateExplore.enabled = false;
        stateConverstation.enabled = false;
        stateRepair.enabled = false;
        stateSleep.enabled = false;

        stateFishing.OnDisable();
        stateConverstation.OnDisable();
        stateRepair.OnDisable();
        stateSleep.OnDisable();

        currentState.enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GM_.Instance.pause.GetPausedState() == PauseManager.PAUSED_STATE.PAUSED)
            return;

        currentState.StateUpdate();
    }

    private void FixedUpdate()
    {
        if (GM_.Instance.pause.GetPausedState() == PauseManager.PAUSED_STATE.PAUSED)
            return;

        currentState.StateFixedUpdate();
    }

    private void LateUpdate()
    {
        if (GM_.Instance.pause.GetPausedState() == PauseManager.PAUSED_STATE.PAUSED)
            return;

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
            case STATE.SLEEP:
                {
                    currentState = stateSleep;
                    break;
                }
        }

        //Debug.Log(newState);
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



