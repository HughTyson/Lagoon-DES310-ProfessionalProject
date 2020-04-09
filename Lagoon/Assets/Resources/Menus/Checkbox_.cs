using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkbox_ : Selectable_
{



    bool toggled_On;
    public bool ToggledOn => toggled_On;

    public class EventArgs_ValueChanged
    {
        public readonly bool newValue;
        public EventArgs_ValueChanged(bool newValue_)
        {
            newValue = newValue_;
        }
    }


    public event System.Action<EventArgs_ValueChanged> Event_ToggleChanged;
    public event System.Action<EventArgs_ValueChanged> Event_ToggleSet;

    protected event System.Action<EventArgs_ValueChanged> InternalEvent_ToggleChanged;
    protected event System.Action<EventArgs_ValueChanged> InternalEvent_ToggleSet;


    // Further internal events to allow different animations for toggled on and off
    protected event System.Action InternalEvent_ShowBegin_ToggledOn;
    protected event System.Action InternalEvent_ShowBegin_ToggledOff;

    protected event System.Action InternalEvent_HideBegin_ToggledOn;
    protected event System.Action InternalEvent_HideBegin_ToggledOff;

    protected event System.Action InternalEvent_SelectedBegin_ToggledOn;
    protected event System.Action InternalEvent_SelectedBegin_ToggledOff;

    protected event System.Action InternalEvent_HoverBegin_ToggledOn;
    protected event System.Action InternalEvent_HoverBegin_ToggledOff;

    protected event System.Action InternalEvent_UnHoverBegin_ToggledOn;
    protected event System.Action InternalEvent_UnHoverBegin_ToggledOff;
    private void Awake()
    {
        Init();
    }

    public void Toggle()
    {
        toggled_On = !toggled_On;

        EventArgs_ValueChanged args = new EventArgs_ValueChanged(toggled_On);
        NoArgsActionWrapper<EventArgs_ValueChanged> internalActionWrapper = new NoArgsActionWrapper<EventArgs_ValueChanged>(InternalEvent_ToggleChanged, args);
        NoArgsActionWrapper<EventArgs_ValueChanged> externalActionWrapper = new NoArgsActionWrapper<EventArgs_ValueChanged>(Event_ToggleChanged, args);
        _Transitioner.RequestNonTransitionCall(internalActionWrapper.wrappedAction, externalActionWrapper.wrappedAction);

        Select();
    }
    public void SetToggle(bool value)
    {
        toggled_On = value;

        EventArgs_ValueChanged args = new EventArgs_ValueChanged(toggled_On);
        NoArgsActionWrapper<EventArgs_ValueChanged> internalActionWrapper = new NoArgsActionWrapper<EventArgs_ValueChanged>(InternalEvent_ToggleSet, args);
        NoArgsActionWrapper<EventArgs_ValueChanged> externalActionWrapper = new NoArgsActionWrapper<EventArgs_ValueChanged>(Event_ToggleSet, args);
        _Transitioner.RequestNonTransitionCall(internalActionWrapper.wrappedAction, externalActionWrapper.wrappedAction);
    }

    protected sealed override void ThisInit_Layer2()
    {
        InternalEvent_BeginHoverOver += internalHoveredOverBegin;
        InternalEvent_UpdateHoverOver += internalHoveredOverUpdate;
        InternalEvent_BeginSelected += internalSelectedBegin;
        InternalEvent_UpdateSelected += internalSelectedUpdate;


        InternalEvent_BeginShow += showBegin;
        InternalEvent_BeginHide += hideBegin;
        InternalEvent_BeginSelected += selectBegin;
        InternalEvent_BeginHoverOver += hoverBegin;
        InternalEvent_BeginUnHoverOver += unhoverBegin;

        InternalEvent_ShowBegin_ToggledOn += internalBeginToggleOn;
        InternalEvent_ShowBegin_ToggledOff += internalBeginToggleOff;

        InternalEvent_HideBegin_ToggledOn += internalBeginHideToggleOn;
        InternalEvent_HideBegin_ToggledOff += internalBeginHideToggleOff;

        InternalEvent_SelectedBegin_ToggledOn += internalBeginSelectToggleOn;
        InternalEvent_SelectedBegin_ToggledOff += internalBeginSelectToggleOff;

        InternalEvent_HoverBegin_ToggledOn += internalBeginHoverToggleOn;
        InternalEvent_HoverBegin_ToggledOff += internalBeginHoverToggleOff;

        InternalEvent_UnHoverBegin_ToggledOn += internalBeginUnHoverToggleOn;
        InternalEvent_UnHoverBegin_ToggledOff += internalBeginUnHoverToggleOff;

        ThisInit_Layer3();
    }
    protected virtual void ThisInit_Layer3() { }





    TweenAnimator.Animation animationShowToggleOn;
    TweenAnimator.Animation.PlayArgs animationShowToggleOnArgs;

    TweenAnimator.Animation animationShowToggleOff;
    TweenAnimator.Animation.PlayArgs animationShowToggleOffArgs;
    void showBegin()
    {
        if (toggled_On)
        {
            InternalEvent_ShowBegin_ToggledOn?.Invoke();
        }
        else
        {
            InternalEvent_ShowBegin_ToggledOff?.Invoke();
        }
    }
    void internalBeginToggleOn()
    {
        if (animationShowToggleOn != null)
        {
            animationShowToggleOn.PlayAnimation(animationShowToggleOnArgs, unique_animationCompleteDelegate_: completedShow);
        }
        else
        {
            _Transitioner.RequestContinue(showBegin);
        }
    }

    void completedShow()
    {
        _Transitioner.RequestContinue(showBegin);
    }
    void internalBeginToggleOff()
    {
        if (animationShowToggleOff != null)
        {
            animationShowToggleOff.PlayAnimation(animationShowToggleOffArgs, unique_animationCompleteDelegate_: completedShow);
        }
        else
        {
            _Transitioner.RequestContinue(showBegin);
        }
    }




    TweenAnimator.Animation animationHideToggleOn;
    TweenAnimator.Animation.PlayArgs animationHideToggleOnArgs;

    TweenAnimator.Animation animationHideToggleOff;
    TweenAnimator.Animation.PlayArgs animationHideToggleOffArgs;
    void hideBegin()
    {
        if (toggled_On)
        {
            InternalEvent_HideBegin_ToggledOn?.Invoke();
        }
        else
        {
            InternalEvent_HideBegin_ToggledOff?.Invoke();
        }
    }
    void internalBeginHideToggleOn()
    {
        if (animationHideToggleOn != null)
        {
            animationHideToggleOn.PlayAnimation(animationHideToggleOnArgs, unique_animationCompleteDelegate_: completedHide);
        }
        else
        {
            _Transitioner.RequestContinue(hideBegin);
        }
    }
    void internalBeginHideToggleOff()
    {
        if (animationHideToggleOff != null)
        {
            animationHideToggleOff.PlayAnimation(animationHideToggleOffArgs, unique_animationCompleteDelegate_: completedHide);
        }
        else
        {
            _Transitioner.RequestContinue(hideBegin);
        }
    }

    void completedHide()
    {
        _Transitioner.RequestContinue(hideBegin);
    }




    TweenAnimator.Animation animationSelectToggleOn;
    TweenAnimator.Animation.PlayArgs animationSelectToggleOnArgs;
    bool animationSelectToggleOn_Delay;

    TweenAnimator.Animation animationSelectToggleOff;
    TweenAnimator.Animation.PlayArgs animationSelectToggleOffArgs;
    bool animationSelectToggleOff_Delay;
    void selectBegin()
    {
        if (toggled_On)
        {
            InternalEvent_SelectedBegin_ToggledOn?.Invoke();
        }
        else
        {
            InternalEvent_SelectedBegin_ToggledOff?.Invoke();
        }
    }
    void internalBeginSelectToggleOn()
    {
        if (animationSelectToggleOn != null)
        {
            if (animationSelectToggleOn_Delay)
            {
                animationSelectToggleOn.PlayAnimation(animationSelectToggleOnArgs, unique_animationCompleteDelegate_: oncompleteSelectToggleOn);
            }
            else
            {
                animationSelectToggleOn.PlayAnimation(animationSelectToggleOnArgs);
                _Transitioner.RequestContinue(selectBegin);
            }

        }
        else
        {
            _Transitioner.RequestContinue(selectBegin);
        }
    }
    void oncompleteSelectToggleOn()
    {
        _Transitioner.RequestContinue(selectBegin);
    }

    void internalBeginSelectToggleOff()
    {
        if (animationSelectToggleOff != null)
        {
            if (animationSelectToggleOff_Delay)
            {
                animationSelectToggleOff.PlayAnimation(animationSelectToggleOnArgs, unique_animationCompleteDelegate_: oncompleteSelectToggleOff);
            }
            else
            {
                animationSelectToggleOff.PlayAnimation(animationSelectToggleOffArgs);
                _Transitioner.RequestContinue(selectBegin);
            }

        }
        else
        {
            _Transitioner.RequestContinue(selectBegin);
        }
    }
    void oncompleteSelectToggleOff()
    {
        _Transitioner.RequestContinue(selectBegin);
    }






    TweenAnimator.Animation animationHoverToggleOn;
    TweenAnimator.Animation.PlayArgs animationHoverToggleOnArgs;

    TweenAnimator.Animation animationHoverToggleOff;
    TweenAnimator.Animation.PlayArgs animationHoverToggleOffArgs;
    void hoverBegin()
    {
        if (toggled_On)
        {
            InternalEvent_HoverBegin_ToggledOn?.Invoke();
        }
        else
        {
            InternalEvent_HoverBegin_ToggledOff?.Invoke();
        }
    }
    void internalBeginHoverToggleOn()
    {
        if (animationHoverToggleOn != null)
        {
            animationHoverToggleOn.PlayAnimation(animationHoverToggleOnArgs);
            _Transitioner.RequestContinue(hoverBegin);
        }
        else
        {
            _Transitioner.RequestContinue(hoverBegin);
        }
    }
    void internalBeginHoverToggleOff()
    {
        if (animationHoverToggleOff != null)
        {
            animationHoverToggleOff.PlayAnimation(animationHoverToggleOffArgs);
            _Transitioner.RequestContinue(hoverBegin);
        }
        else
        {
            _Transitioner.RequestContinue(hoverBegin);
        }
    }


    TweenAnimator.Animation animationUnHoverToggleOn;
    TweenAnimator.Animation.PlayArgs animationUnHoverToggleOnArgs;

    TweenAnimator.Animation animationUnHoverToggleOff;
    TweenAnimator.Animation.PlayArgs animationUnHoverToggleOffArgs;
    void unhoverBegin()
    {
        if (toggled_On)
        {
            InternalEvent_UnHoverBegin_ToggledOn?.Invoke();
        }
        else
        {
            InternalEvent_UnHoverBegin_ToggledOff?.Invoke();
        }
    }
    void internalBeginUnHoverToggleOn()
    {
        if (animationUnHoverToggleOn != null)
        {
            animationUnHoverToggleOn.PlayAnimation(animationUnHoverToggleOnArgs);
            _Transitioner.RequestContinue(unhoverBegin);
        }
        else
        {
            _Transitioner.RequestContinue(unhoverBegin);
        }
    }
    void internalBeginUnHoverToggleOff()
    {
        if (animationUnHoverToggleOff != null)
        {
            animationUnHoverToggleOff.PlayAnimation(animationUnHoverToggleOffArgs);
            _Transitioner.RequestContinue(unhoverBegin);
        }
        else
        {
            _Transitioner.RequestContinue(unhoverBegin);
        }
    }


    public void OverrideBeginShow_ToggledOn(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationShowToggleOn = animation;
        animationShowToggleOnArgs = args;
    }
    public void OverrideBeginShow_ToggledOff(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationShowToggleOff = animation;
        animationShowToggleOffArgs = args;
    }
    public void OverrideBeginHide_ToggledOn(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationHideToggleOn = animation;
        animationHideToggleOnArgs = args;
    }
    public void OverrideBeginHide_ToggledOff(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationHideToggleOff = animation;
        animationHideToggleOffArgs = args;
    }
    public void OverrideBeginSelect_ToggledOn(TweenAnimator.Animation animation, bool delayUntilComplete, TweenAnimator.Animation.PlayArgs args)
    {
        animationSelectToggleOn = animation;
        animationSelectToggleOnArgs = args;
        animationSelectToggleOn_Delay = delayUntilComplete;
    }
    public void OverrideBeginSelect_ToggledOff(TweenAnimator.Animation animation, bool delayUntilComplete, TweenAnimator.Animation.PlayArgs args)
    {
        animationSelectToggleOff = animation;
        animationSelectToggleOffArgs = args;
        animationSelectToggleOff_Delay = delayUntilComplete;
    }
    public void OverrideBeginHover_ToggledOn(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationHoverToggleOn = animation;
        animationHoverToggleOnArgs = args;
    }
    public void OverrideBeginHover_ToggledOff(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationHoverToggleOff = animation;
        animationHoverToggleOffArgs = args;
    }
    public void OverrideBeginUnHover_ToggledOn(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationUnHoverToggleOn = animation;
        animationUnHoverToggleOnArgs = args;
    }
    public void OverrideBeginUnHover_ToggledOff(TweenAnimator.Animation animation, TweenAnimator.Animation.PlayArgs args)
    {
        animationUnHoverToggleOff = animation;
        animationUnHoverToggleOffArgs = args;
    }


    float current_optionswap_timer;
    bool changedThisFrame = false;
    void internalHoveredOverBegin()
    {
        current_optionswap_timer = OPTION_SWAP_COOLDOWN;
        changedThisFrame = true;
        _Transitioner.RequestContinue(internalHoveredOverBegin);
    }
    void internalHoveredOverUpdate()
    {
        if (changedThisFrame) // prevents infinite loop due to Input not being updated, causing infinite toggle of button down
        {
            changedThisFrame = false;
            return;
        }

        float horizontal = GM_.Instance.input.GetAxis(InputManager.AXIS.LH);
        float vertical = GM_.Instance.input.GetAxis(InputManager.AXIS.LV);


        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
            Toggle();
            return;
        }
        // always go for the axis with the the highest magnitude pushed on it, icase the player is pushing over the deadzone in both, the horizontal and vertical
        if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            if (vertical > OPTION_SWAP_DEADZONE)
            {
                current_optionswap_timer -= Time.unscaledDeltaTime;

                if (siblingUp != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        siblingUp.HoverOver();
                        UnHoverOver();
                    }
                }

            }
            else if (vertical < -OPTION_SWAP_DEADZONE)
            {
                current_optionswap_timer -= Time.unscaledDeltaTime;

                if (siblingDown != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        siblingDown.HoverOver();
                        UnHoverOver();
                    }
                }
            }
            else
            {
                current_optionswap_timer = -0.001f;
            }
        }
        else
        {
            if (horizontal > OPTION_SWAP_DEADZONE)
            {
                current_optionswap_timer -= Time.unscaledDeltaTime;

                if (siblingRight != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        siblingRight.HoverOver();
                        UnHoverOver();
                    }
                }

            }
            else if (horizontal < -OPTION_SWAP_DEADZONE)
            {
                current_optionswap_timer -= Time.unscaledDeltaTime;

                if (siblingLeft != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        siblingLeft.HoverOver();
                        UnHoverOver();
                    }
                }
            }
            else
            {
                current_optionswap_timer = -0.001f;
            }
        }
    }



    void internalSelectedBegin()
    {
        current_optionswap_timer = OPTION_SWAP_COOLDOWN;
        changedThisFrame = true;
        _Transitioner.RequestContinue(internalSelectedBegin);
    }

    void internalSelectedUpdate()
    {

        if (changedThisFrame) // prevents infinite loop due to Input not being updated, causing infinite toggle of button down
        {
            changedThisFrame = false;
            return;
        }

        float horizontal = GM_.Instance.input.GetAxis(InputManager.AXIS.LH);
            float vertical = GM_.Instance.input.GetAxis(InputManager.AXIS.LV);


            if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
            {
                GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                Toggle();
                return;
            }
            // always go for the axis with the the highest magnitude pushed on it, icase the player is pushing over the deadzone in both, the horizontal and vertical
            if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
            {
                if (vertical > OPTION_SWAP_DEADZONE)
                {
                    current_optionswap_timer -= Time.unscaledDeltaTime;

                    if (siblingUp != null)
                    {
                        if (current_optionswap_timer < 0)
                        {
                            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                            siblingUp.HoverOver();
                            UnHoverOver();
                        }
                    }

                }
                else if (vertical < -OPTION_SWAP_DEADZONE)
                {
                    current_optionswap_timer -= Time.unscaledDeltaTime;

                    if (siblingDown != null)
                    {
                        if (current_optionswap_timer < 0)
                        {
                            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                            siblingDown.HoverOver();
                            UnHoverOver();
                        }
                    }
                }
                else
                {
                    current_optionswap_timer = -0.001f;
                }
            }
            else
            {
                if (horizontal > OPTION_SWAP_DEADZONE)
                {
                    current_optionswap_timer -= Time.unscaledDeltaTime;

                    if (siblingRight != null)
                    {
                        if (current_optionswap_timer < 0)
                        {
                            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                            siblingRight.HoverOver();
                            UnHoverOver();
                        }
                    }

                }
                else if (horizontal < -OPTION_SWAP_DEADZONE)
                {
                current_optionswap_timer -= Time.unscaledDeltaTime;
                    if (siblingLeft != null)
                        {

                        if (current_optionswap_timer < 0)
                        {
                            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                            siblingLeft.HoverOver();
                            UnHoverOver();
                        }
                    }
                }
                else
                {
                    current_optionswap_timer = -0.001f;
                }
            }
    }
}
