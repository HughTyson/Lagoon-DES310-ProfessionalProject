using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManual
{


    State[] states;
    State currentState = null;


    public FSMManual(int initStateID, bool triggerStateOnEnter, params State[] states_)
    {
        states = states_;
        currentState = states[initStateID];
        if (triggerStateOnEnter)
            currentState.OnEnter?.Invoke();
    }


    public void Update()
    {
        currentState.Update?.Invoke();
    }
    public void ChangeState(int stateID)
    {
        currentState.OnExit?.Invoke();
        currentState.Transitions[stateID]?.Invoke();
        currentState = states[stateID];
        currentState.OnEnter?.Invoke();
    }


    public class State
    {
        public Dictionary<int, System.Action> Transitions;

        public System.Action Update;
        public System.Action OnEnter;
        public System.Action OnExit;

        public State(System.Action update, System.Action on_enter, System.Action on_exit, params Transition[] transitions_)
        {
            Update = update;
            OnEnter = on_enter;
            OnExit = on_exit;
            
            for (int i = 0; i < transitions_.Length; i++)
            {
                Transitions.Add(transitions_[i].toStateID, transitions_[i].TransitioningTo);
            }
        }

    }

    public struct Transition
    {
        public System.Action TransitioningTo;
        public int toStateID;
        public Transition(int toStateID_, System.Action TransitiongTo_)
        {
            toStateID = toStateID_;
            TransitioningTo = TransitiongTo_;
        }
    }
}
