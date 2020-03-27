using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTimer
{
    class ActionWrapper
    {
        public System.Action action;
        public float time; 
    }

    float current_time;

    List<ActionWrapper> totalActions = new List<ActionWrapper>();

    List<ActionWrapper> uncalledActions = new List<ActionWrapper>();

    public void AddAction(System.Action action_, float time_of_call)
    {
        totalActions.Add(new ActionWrapper() { action = action_, time = time_of_call });
    }
    public void Start()
    {
        uncalledActions.Clear();
        uncalledActions.AddRange(totalActions);
        current_time = 0;
    }

    public void Update()
    {
        current_time += Time.unscaledDeltaTime;

       uncalledActions.RemoveAll(y =>
       {
            if (current_time >= y.time)
           {
               y.action?.Invoke();
               return true;
           }
           return false;
       }
        );
    }


    // Call the remaining actions and clear them
    public void CallAndFlush()
    {
        for (int i = 0; i < uncalledActions.Count; i++)
        {
            uncalledActions[i].action?.Invoke();
        }
        uncalledActions.Clear();
    }
   
}
