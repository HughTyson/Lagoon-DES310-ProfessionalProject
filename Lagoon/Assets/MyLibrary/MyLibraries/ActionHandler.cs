using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActionHandler<T> where T : System.Enum
{
    System.Action[] actions;


    public ActionHandler()
    {
        actions = new System.Action[System.Enum.GetValues(typeof(T)).Length];
    }
    public void AddAction(T id, System.Action action)
    {
        actions[(int)(object)(id)] += action;
    }
    public void RemoveAction(T id, System.Action action)
    {
        actions[(int)(object)id] -= action;
    }


    bool isInvoking = false;
    Queue<T> invoke_queue = new Queue<T>();
    
    /*
     *  recursion used to prevent invokes which happen inside an invoked action happening before all first invoke actions get called first 
     *          e.g - action A is called
     *              - inside action A, action B is called
     *              - don't call action B, add it to a queue instead
     *              - action A finished and has returned
     *              - the queue isn't empty, so call the next action; action B           
     */
    public void InvokeActions(T id) 
    {
        if (!isInvoking)
        {
            isInvoking = true;
            actions[(int)(object)id]?.Invoke();
            isInvoking = false;
            if (invoke_queue.Count == 1)
                InvokeActions(invoke_queue.Dequeue());
            else if (invoke_queue.Count > 1)
            {
                Debug.LogError("Error in ActionHandler. Ambiguous Invokations. Multiple invokations occurred in the same action invoke");
                Debug.Break();
            }

        }
        else
        {
            invoke_queue.Enqueue(id);
        }
    }


}
