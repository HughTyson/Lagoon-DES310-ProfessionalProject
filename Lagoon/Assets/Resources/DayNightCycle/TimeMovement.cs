using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMovement : MonoBehaviour
{

    public enum Solar
    {
        DAY,
        NIGHT
    }

    Solar solar;
    
    float time_multiplyer = 1.0f;
    float base_time = 1.0f; //base time is the time which has been set by the story - so if the game is waiting on the radio base time will be set to 0 as we are waiting on the player ot have the conversation.

    public void SetBaseTime(float base_time_)
    {
        base_time = base_time_;
    }

    public void SetTime()   //if there is no parameters then the it will be set to the base_time that is defined by the break in the story
    {
        time_multiplyer = base_time;
    }

    public void SetTime(float time_multi)
    {
        time_multiplyer = time_multi;
    }

    public float GetTime()
    {
        return time_multiplyer;
    }

    public void SetSolar(Solar s)
    {
        solar = s;
    }

    public Solar GetSolar()
    {
        return solar;
    }

}
