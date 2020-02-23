using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XInputDotNetPure; // Required in C#

public class PauseManager
{
    /*
    NOTES:
        PauseManager can be used to set a pause on or off and allow all checking objects to act accordingly depending on what they need to 
        do such as manually pause an Animator State Machine or allow non-time dependant objects to ignore inputs or updates
        
    */


    public enum PAUSED_EVENTS
    { 
        NONE,
        PAUSED_ON_FRAME,
        UNPAUSED_ON_FRAME
    }
    public enum PAUSED_STATE
    {
        UNPAUSED,
        PAUSED
    }


    PAUSED_EVENTS paused_event = PAUSED_EVENTS.NONE;
    PAUSED_STATE paused_state = PAUSED_STATE.UNPAUSED;


    bool dirty_data = false; // a change in data has happened but don't let calling objects know about it until the late update to keep synchronised objects
    PAUSED_EVENTS dirty_paused_event = PAUSED_EVENTS.NONE;
    PAUSED_STATE dirty_paused_state = PAUSED_STATE.UNPAUSED;


    // NOTE ON PAUSING
    // if a time sensitive object should keep playing while paused, use UnscaledDelta / FixedUnscaledDelta 
    public void Pause() 
    {
            dirty_data = true;
            dirty_paused_event = PAUSED_EVENTS.PAUSED_ON_FRAME;
            dirty_paused_state = PAUSED_STATE.PAUSED;
    }

    public void UnPause()
    {
            dirty_data = true;
            dirty_paused_event = PAUSED_EVENTS.UNPAUSED_ON_FRAME;
            dirty_paused_state = PAUSED_STATE.UNPAUSED;
    }


    public PAUSED_EVENTS GetPausedEvent()
    {
        return paused_event;
    }

    public PAUSED_STATE GetPausedState()
    {
        return paused_state;
    }


    public void Update() 
    {
        paused_event = PAUSED_EVENTS.NONE;

        if (dirty_data)
        {
            dirty_data = false;

            if (dirty_paused_state != paused_state)
            {
                paused_state = dirty_paused_state;
                paused_event = dirty_paused_event;
            }

            switch(paused_event)
            {
                case PAUSED_EVENTS.PAUSED_ON_FRAME:
                    {
                        Time.timeScale = 0.0f;
                        break;
                    }
                case PAUSED_EVENTS.UNPAUSED_ON_FRAME:
                    {
                        Time.timeScale = 1.0f;
                        break;
                    }
            }
        }


    }
   

}
