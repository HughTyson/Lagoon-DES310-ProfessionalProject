using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventHandler
{
    // event : Event Started
    // event request: Block Completed
    // event : Event Completed


    // steps
    // Subscribe to Event Started, Event Completed Request and Event Completed
    // Event Started: Called when an event has been triggered (e.i Supply Crate Event has been triggered)
    // EventRequest Block Completed: Gives opportunity for subscibers to block the completed event from triggering
    // Event Completed: Called when an event has been completed

    // Call "EventCompletedRequest" When 

    public bool EventCompletedRequest()
    {
      //  Event_Completed?.Invoke();
        return true;
    }

}
