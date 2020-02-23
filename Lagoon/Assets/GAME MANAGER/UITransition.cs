using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UITransition : MonoBehaviour
{
    [SerializeField] Image imgFade;

    public enum FADE_PRESET
    { 
        DEFAULT
    }



    enum STATE
    { 
        SETTLED,
        FADE_IN,
        FADE_OUT,
        FADE_INOUT
    }

    STATE state = STATE.SETTLED; 

    float timeIn = 0;
    float timeWait = 0;
    float timeOut = 0;

    float timeInCurrent = 0;
    float timeWaitCurrent = 0;
    float timeOutCurrent = 0;

    public void ManagerUpdate()
    {
        float new_alpha = 0;
        switch (state)
        {
            case STATE.FADE_IN:
                {
                    timeInCurrent -= Time.deltaTime;
                    new_alpha = 1.0f - (timeInCurrent / timeIn);
                    if (timeInCurrent <= 0)
                    {
                        state = STATE.SETTLED;    
                    }
                    break;
                }
            case STATE.FADE_OUT:
                {
                    timeOutCurrent -= Time.deltaTime;
                    new_alpha = (timeOutCurrent / timeOut);
                    if (timeOutCurrent <= 0)
                    {
                        state = STATE.SETTLED;
                    }
                    break;
                }
            case STATE.FADE_INOUT:
                {
                   
                    if (timeInCurrent >= 0)
                    {
                        timeInCurrent -= Time.deltaTime;
                        new_alpha = 1.0f -  (timeInCurrent / timeIn);
                    }
                    else
                    {
                        if (timeWaitCurrent >= 0)
                        {
                            timeWaitCurrent -= Time.deltaTime;
                            new_alpha = imgFade.color.a;
                        }
                        else
                        {
                            if (timeOutCurrent >= 0)
                            {
                                timeOutCurrent -= Time.deltaTime;
                                new_alpha = (timeOutCurrent / timeOut);

                            }
                            else
                            {
                                state = STATE.SETTLED;
                                new_alpha = imgFade.color.a;
                            }
                        }
                    }
                    break;
                }
            case STATE.SETTLED:
                {
                    new_alpha = imgFade.color.a;
                    break;
                }
        }
        imgFade.color = new Color( 0, 0, 0, Mathf.Clamp01(new_alpha));

    }

    public bool IsTransitioning()
    {
        return (state != STATE.SETTLED);
    }

    public bool IsInWaitingTransition()
    {
        if (state == STATE.FADE_INOUT)
        {
            if (timeWaitCurrent >= 0 && timeInCurrent <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public void FadePreset(FADE_PRESET preset)
    {
        switch (preset)
        {
            case FADE_PRESET.DEFAULT:
                {
                    FadeInOut(1.0f, 0.1f, 1.0f);
                    break;
                }
        }
    }

    public void FadeIn(float transition_time)
    {
        timeIn = transition_time;
        timeInCurrent = timeIn;
        state = STATE.FADE_IN;
    }

    public void FadeOut(float transition_time)
    {
        timeOut = transition_time;
        timeOutCurrent = timeOut;
        state = STATE.FADE_OUT;
    }

    public void FadeInOut(float transition_in_time, float transition_wait_time, float transition_out_time)
    {
        timeIn = transition_in_time;
        timeInCurrent = timeIn;

        timeWaitCurrent = transition_wait_time;
        timeWait = timeWaitCurrent;

        timeOut = transition_out_time;
        timeOutCurrent = timeOut;

        state = STATE.FADE_INOUT;
    }
}
