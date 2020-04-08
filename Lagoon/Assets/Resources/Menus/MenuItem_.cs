using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem_ : MonoBehaviour
{

    public abstract class CMD_Base { };
    public class CMD_Show : CMD_Base { };
    public class CMD_Hide : CMD_Base { };


    static CMD_Show cmdShow = new CMD_Show();
    static CMD_Hide cmdHide = new CMD_Hide();

    protected class InteruptArgs
    {
        public InteruptArgs(CMD_Base interuptedBy_, Transitioner.STATE currentState_)
        {
            interuptedBy = interuptedBy_.GetType();
            currentState = currentState_;
        }
        public readonly System.Type interuptedBy;
        public readonly Transitioner.STATE currentState;
  
    }
    protected class InteruptReturn
    {
        public enum INTERUPT_RESOLUTION
        { 
            END_CURRENT__START_INTERUPTED_BY,
            IGNORE,
            
            
        }
        public INTERUPT_RESOLUTION interuptResolution = INTERUPT_RESOLUTION.END_CURRENT__START_INTERUPTED_BY;

    }





    protected class Transitioner
    {
        System.Action<InteruptArgs, InteruptReturn> interuptionCallBack;

        System.Action internalBegin;
        System.Action internalUpdate;
        System.Action internalEnd;

        System.Action externalComplete;

        System.Action ForceCompleteAllAnimations;
        public Transitioner(System.Action forceCompleteAllAnimations)
        {
            ForceCompleteAllAnimations = forceCompleteAllAnimations;
        }
        public enum STATE
        { 
            NO_STATE,
            BEGIN,
            UPDATE,
            END       
        }
        STATE state = STATE.NO_STATE;

        public STATE State => state;

        public void RequestBegin(CMD_Base myCMD, System.Action<InteruptArgs, InteruptReturn> interuptionCallBack_, System.Action internalBegin_, System.Action internalUpdate_, System.Action internalEnd_, System.Action externalComplete_)
        {
            if (state == STATE.NO_STATE)
            {
                state = STATE.BEGIN;
                internalBegin = internalBegin_;
                interuptionCallBack = interuptionCallBack_;
                internalUpdate = internalUpdate_;
                internalEnd = internalEnd_;
                externalComplete = externalComplete_;


                if (internalBegin == null)
                {
                    RequestContinue(null);
                }
                else
                {
                    internalBegin?.Invoke();
                }
            }
            else
            {
                InteruptReturn interuptReturn = new InteruptReturn();
                interuptionCallBack?.Invoke(new InteruptArgs(myCMD, state), interuptReturn);

                switch (interuptReturn.interuptResolution)
                {
                    case InteruptReturn.INTERUPT_RESOLUTION.END_CURRENT__START_INTERUPTED_BY:
                        {
                            GM_.Instance.update_events.UpdateEvent -= internalUpdate;
                            internalBegin = null;
                            internalUpdate = null;
                            internalEnd = null;
                            ForceCompleteAllAnimations?.Invoke();
                            state = STATE.NO_STATE;
                            RequestBegin(myCMD, interuptionCallBack_, internalBegin_, internalUpdate_, internalEnd_, externalComplete_);
                            break;
                        }
                    case InteruptReturn.INTERUPT_RESOLUTION.IGNORE:
                        {
                            break;
                        }
                }
            }
        }

       public void RequestContinue(System.Action unsubscribeFunction)
        {
            switch (state)
            {
                case STATE.BEGIN:
                    {
                        internalBegin -= unsubscribeFunction;

                        if (internalBegin == null)
                        {
                            state = STATE.UPDATE;
                            GM_.Instance.update_events.UpdateEvent += internalUpdate;

                            if (internalUpdate == null)
                            {
                                RequestContinue(null);
                            }
                            else
                            {
                                internalUpdate?.Invoke();
                            }

                        }
                        break;
                    }
                case STATE.UPDATE:
                    {
                        internalUpdate -= unsubscribeFunction;
                        GM_.Instance.update_events.UpdateEvent -= unsubscribeFunction;

                        if (internalUpdate == null)
                        {
                            state = STATE.END;

                            if (internalEnd == null)
                            {
                                RequestContinue(null);
                            }
                            else
                            {
                                internalEnd?.Invoke();
                            }

                        }
                        break;
                    }
                case STATE.END:
                    {
                        internalEnd -= unsubscribeFunction;

                        if (internalEnd == null)
                        {
                            state = STATE.NO_STATE;
                            externalComplete?.Invoke();
                        }
                        break;
                    }
                case STATE.NO_STATE:
                    {
                        Debug.LogError("Error! AttemptEnd called when nothing was transitioning!");
                        Debug.Break();
                        break;
                    }
            }

        }
      


        //void End()
        //{
        //    isTransitioning = false;
        //    GM_.Instance.update_events.UpdateEvent -= Update;
        //    internalEnd?.Invoke();
        //    externalComplete?.Invoke();
        //}
    }


    Transitioner transitioner;
    protected Transitioner _Transitioner => transitioner;


    bool isShowing;
    public bool IsShowing => isShowing;

    public event System.Action Event_CompletedHide;
    public event System.Action Event_CompletedShow;


    private void Awake()
    {
        Init();
    }


    void internalBeginHide()
    {
        if (animationHide != null)
        {
            animationHide.PlayAnimation(animationHideArgs, unique_animationCompleteDelegate_: hideAnimationCompleted);
        }
        else
        {
            transitioner.RequestContinue(internalBeginHide);
        }
    }
    void hideAnimationCompleted()
    {
        transitioner.RequestContinue(internalBeginHide);
    }


    void internalEndHide()
    {
        isShowing = false;
        gameObject.SetActive(false);
        transitioner.RequestContinue(internalEndHide);
    }

    void internalBeginShow()
    {
        gameObject.SetActive(true);

        if (animationShow != null)
        {
            animationShow.PlayAnimation(animationShowArgs, unique_animationCompleteDelegate_: showAnimationCompleted);
        }
        else
        {
            transitioner.RequestContinue(internalBeginShow);
        }
    }
    void showAnimationCompleted()
    {
        transitioner.RequestContinue(internalBeginShow);
    }


    void internalEndShow()
    {
        isShowing = true;
        transitioner.RequestContinue(internalEndShow);
    }

    protected void Init()
    {
        transitioner = new Transitioner(ForceCompleteAllAnimations);
        InternalEvent_BeginHide += internalBeginHide;
        InternalEvent_EndHide += internalEndHide;
        InternalEvent_EndShow += internalEndShow;
        InternalEvent_BeginShow += internalBeginShow;

        isShowing = gameObject.activeSelf;

        ThisInit_Layer1(); // go all the way down the inheritance tree to the object, calling each layers initalization
        ApplyDefaults(); // apply defaults once the object is initialized

    }

    virtual protected void ThisInit_Layer1() { }
    virtual protected void ApplyDefaults() { }

    public void Hide()
    {
        transitioner.RequestBegin(cmdHide, InteruptedHide, InternalEvent_BeginHide, InternalEvent_UpdateHide, InternalEvent_EndHide, Event_CompletedHide);
    }
    public void Show()
    {
        transitioner.RequestBegin(cmdShow, InteruptedShow, InternalEvent_BeginShow, InternalEvent_UpdateShow, InternalEvent_EndShow, Event_CompletedShow);
    }

    public void OverrideHideAnimation(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationHide = animation;
        animationHideArgs = args;
    }
    public void OverrideShowAnimation(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationShow = animation;
        animationShowArgs = args;
    }

    TweenAnimator.Animation animationHide;
    TweenAnimator.Animation.PlayArgs animationHideArgs;

    TweenAnimator.Animation animationShow;
    TweenAnimator.Animation.PlayArgs animationShowArgs;

    protected event System.Action InternalEvent_BeginHide;
    protected event System.Action InternalEvent_UpdateHide;
    protected event System.Action InternalEvent_EndHide;

    protected event System.Action InternalEvent_BeginShow;
    protected event System.Action InternalEvent_UpdateShow;
    protected event System.Action InternalEvent_EndShow;


    protected void ForceCompleteAllAnimations()
    {
        if (animationHide != null)
            animationHide.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
        if (animationShow != null)
            animationShow.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
        ForceCompleteAnimations_Layer1();
    }
    protected virtual void ForceCompleteAnimations_Layer1()
    { }

    protected virtual void InteruptedHide(InteruptArgs args, InteruptReturn returns)
    {

    }
    protected virtual void InteruptedShow(InteruptArgs args, InteruptReturn returns)
    {

    }



}
