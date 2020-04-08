using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable_ : MenuItem_
{
    [SerializeField] protected Selectable_ siblingUp;
    [SerializeField] protected Selectable_ siblingDown;
    [SerializeField] protected Selectable_ siblingLeft;
    [SerializeField] protected Selectable_ siblingRight;

    public event System.Action Event_Selected;
    public event System.Action Event_HoveredOver;
    public event System.Action Event_UnHoveredOver;

    public class CMD_Select : CMD_Base { };
    public class CMD_HoverOver : CMD_Base { };
    public class CMD_UnHoverOver : CMD_Base { };

    static CMD_Select cmdSelect = new CMD_Select();
    static CMD_HoverOver cmdHoverOver = new CMD_HoverOver();
    static CMD_UnHoverOver cmdUnHoverOver = new CMD_UnHoverOver();


    protected static readonly float OPTION_SWAP_COOLDOWN = 0.3f;
    protected static readonly float OPTION_SWAP_DEADZONE = 0.3f;

    protected void SetupNavigation(Selectable_ up = null, Selectable_ down = null, Selectable_ left = null, Selectable_ right = null)
    {
        siblingUp = up;
        siblingDown = down;
        siblingLeft = left;
        siblingRight = right;
    }

    public void Select()
    {
         _Transitioner.RequestBegin(cmdSelect, InteruptedSelect, InternalEvent_BeginSelected, InternalEvent_UpdateSelected, InternalEvent_EndSelected, Event_Selected);
    }
    public void HoverOver()
    {
         _Transitioner.RequestBegin(cmdHoverOver, InteruptedHoverOver, InternalEvent_BeginHoverOver, InternalEvent_UpdateHoverOver, InternalEvent_EndHoverOver, Event_HoveredOver);
    }
    public void UnHoverOver()
    {
        _Transitioner.RequestBegin(cmdUnHoverOver, InteruptedUnHoverOver, InternalEvent_BeginUnHoverOver, InternalEvent_UpdateUnHoverOver, InternalEvent_EndUnHoverOver, Event_UnHoveredOver);
    }


    protected event System.Action InternalEvent_BeginHoverOver;
    protected event System.Action InternalEvent_UpdateHoverOver;
    protected event System.Action InternalEvent_EndHoverOver;


    protected event System.Action InternalEvent_BeginSelected;
    protected event System.Action InternalEvent_UpdateSelected;
    protected event System.Action InternalEvent_EndSelected;


    protected event System.Action InternalEvent_BeginUnHoverOver;
    protected event System.Action InternalEvent_UpdateUnHoverOver;
    protected event System.Action InternalEvent_EndUnHoverOver;

    protected virtual void InteruptedSelect(InteruptArgs args, InteruptReturn returns)
    {

    }
    protected virtual void InteruptedHoverOver(InteruptArgs args, InteruptReturn returns)
    {

    }
    protected virtual void InteruptedUnHoverOver(InteruptArgs args, InteruptReturn returns)
    {

    }

    public enum SELECTABLE_STATE 
        {
        SELECTED,
        HOVERED_OVER,
        UNHOVERED_OVER
        }
    SELECTABLE_STATE selectable_state;
    public SELECTABLE_STATE SelectableState => selectable_state;


    void internalBeginSelect()
    {
        selectable_state = SELECTABLE_STATE.SELECTED;
        if (animationSelect != null)
        {
            animationSelect.PlayAnimation(animationSelectArgs, unique_animationCompleteDelegate_: _completedBeginSelectAnimation);
        }
        else
        {
            _Transitioner.RequestContinue(internalBeginSelect);
        }
    }
    void _completedBeginSelectAnimation()
    {
        _Transitioner.RequestContinue(internalBeginSelect);
    }

    void internalBeginHoverOver()
    {
        selectable_state = SELECTABLE_STATE.HOVERED_OVER;
        if (animationHoverOver != null)
        {
            if (animationHoverOver_DelayEvent)
            {
                animationHoverOver.PlayAnimation(animationHoverOverArgs, unique_animationCompleteDelegate_: _completedBeginHoverOverAnimation);
            }
            else
            {
                animationHoverOver.PlayAnimation(animationHoverOverArgs);
                _Transitioner.RequestContinue(internalBeginHoverOver);
            }
        }
        else
        {
            _Transitioner.RequestContinue(internalBeginHoverOver);
        }
    }
    void _completedBeginHoverOverAnimation()
    {
        _Transitioner.RequestContinue(internalBeginHoverOver);
    }


    void internalBeginUnHoverOver()
    {
        selectable_state = SELECTABLE_STATE.UNHOVERED_OVER;
        if (animationUnHoverOver != null)
        {
            animationUnHoverOver.PlayAnimation(animationUnHoverOverArgs, unique_animationCompleteDelegate_: _completedBeginUnHoverOverAnimation);
        }
        else
        {
            _Transitioner.RequestContinue(internalBeginUnHoverOver);
        }
    }
    void _completedBeginUnHoverOverAnimation()
    {
        _Transitioner.RequestContinue(internalBeginUnHoverOver);
    }


    TweenAnimator.Animation animationUnHoverOver;
    TweenAnimator.Animation.PlayArgs animationUnHoverOverArgs;

    TweenAnimator.Animation animationHoverOver;
    TweenAnimator.Animation.PlayArgs animationHoverOverArgs;
    bool animationHoverOver_DelayEvent;

    TweenAnimator.Animation animationSelect;
    TweenAnimator.Animation.PlayArgs animationSelectArgs;

    public void OverrideBeginHoverOverAnimation(TweenAnimator.Animation animation, bool doesAnimationDelayEvent, TweenAnimator.Animation.PlayArgs args)
    {
        animationHoverOver = animation;
        animationHoverOverArgs = args;
        animationHoverOver_DelayEvent = doesAnimationDelayEvent;
    }
    public void OverrideBeginUnHoverOverAnimation(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationUnHoverOver = animation;
        animationUnHoverOverArgs = args;
    }
    public void OverrideBeginSelectAnimation(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationSelect = animation;
        animationSelectArgs = args;
    }


    private void Awake()
    {
        Init();
    }

    protected sealed override void ThisInit_Layer1()
    {
        InternalEvent_BeginHoverOver += internalBeginHoverOver;
        InternalEvent_BeginUnHoverOver += internalBeginUnHoverOver;
        InternalEvent_BeginSelected += internalBeginSelect;
        selectable_state = SELECTABLE_STATE.UNHOVERED_OVER;
        ThisInit_Layer2();
    }


    protected virtual void ThisInit_Layer2(){}


    protected sealed override void ForceCompleteAnimations_Layer1()
    {
        if (animationHoverOver != null)
            animationHoverOver.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
        if (animationUnHoverOver != null)
            animationUnHoverOver.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
        if (animationSelect != null)
            animationSelect.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);

    }
}
