using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * EXAMPLE OF USE:
 * 
 *   
        stateMachine = new FSMAuto(
            (int)STATES.DEFAULT, false,
            new FSMAuto.State(
                DefaultUpdate, 
                DefaultOnEnter,
                DefaultOnExit,
                new FSMAuto.Transition(
                    (int)STATES.FIRE,
                    new Conditioners.Float(
                        testerFloat,
                        0.0f,
                        0.1f                
                        ),
                    new Conditioners.Bool(
                        testerBool,
                        false
                        )
                    )
                ),
            new FSMAuto.State(
                FireUpdate,
                null,
                null
                )
            );
 */
public class FSMAuto
{

    State[] states;
    State currentState = null;


    public FSMAuto(int initStateID, bool triggerInitStateEnter, params State[] states_)
    { 
        states = states_;
        currentState = states[initStateID];
        if (triggerInitStateEnter)
            currentState.OnEnter?.Invoke();

#if UNITY_EDITOR
        for (int i = 0; i < states.Length; i++)
        {
            for (int t = 0; t < states[i].transitions.Length; t++)
                if (states[i].transitions[t].toStateID > states.Length - 1)
                {
                    Debug.LogError("Error! Transition's toStateID is out of bounds in State("+i.ToString()+")"+".Transition("+t.ToString()+")");
                }
        }
#endif
    }


    public void Update()
    {
        for (int i = 0; i < currentState.transitions.Length; i++)
        {
            if (currentState.transitions[i].IsConditionMet())
            {
                currentState.OnExit?.Invoke();
                currentState.transitions[i].TransitioningTo?.Invoke();
                currentState = states[currentState.transitions[i].toStateID];
                currentState.OnEnter?.Invoke();
                break;
            }
        }

        currentState.Update?.Invoke();
    }

    public class State
    {

        public System.Action Update;
        public System.Action OnEnter;
        public System.Action OnExit;

        public Transition[] transitions = null;

        public State(System.Action update, System.Action on_enter, System.Action on_exit, params Transition[] transitions_)
        {
            Update = update;
            OnEnter = on_enter;
            OnExit = on_exit;
             transitions = transitions_;
        }

    }

    public class Transition
    {
        public System.Action TransitioningTo;

        public int toStateID;
        Conditioners.BaseC[] conditions;
        public Transition(int toStateID_, System.Action TransitiongTo_, params Conditioners.BaseC[] conditions_)
        {
            toStateID = toStateID_;
            conditions = conditions_;
            TransitioningTo = TransitiongTo_;
        }
     
        public bool IsConditionMet()
        {
            for (int i = 0; i < conditions.Length; i++)
            {
               if (conditions[i].IsConditionMet())
                {
                    return true;
                }
            }
            return false;
        }
    }

}
