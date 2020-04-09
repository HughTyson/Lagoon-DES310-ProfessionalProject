using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class Slider_ : Selectable_
{
    public enum DIRECTION
    {
        VERTICAL,
        HORIZONTAL
    }

    [SerializeField]
    DIRECTION direction;

    [SerializeField]
    float startValue;
    [SerializeField]
    float endValue;
    [SerializeField]
    float sliderInvervals = 0.1f;
    [SerializeField]
    float SLIDER_VALUE_CHANGE_COOLDOWN = 0.1f;


    float sliderWeighting = 0;
    public float Value => Mathf.Lerp(startValue,endValue, sliderWeighting);
    public float SliderWeighting => sliderWeighting;
    public class EventArgs_ValueChanged
    {
        public readonly float newValue;
        public EventArgs_ValueChanged(float newValue_)
        {
            newValue = newValue_;
        }
    }

    public event System.Action<EventArgs_ValueChanged> Event_ValueChanged;
    protected event System.Action<EventArgs_ValueChanged> InternalEvent_ValueChanged;

    public event System.Action<EventArgs_ValueChanged> Event_ValueSet;
    protected event System.Action<EventArgs_ValueChanged> InternalEvent_ValueSet;


    private void Awake()
    {
        Init();
    }


    protected sealed override void ThisInit_Layer2()
    {
        InternalEvent_BeginHoverOver += internalHoveredOverBegin;
        InternalEvent_BeginUnSelect += internalHoveredOverBegin;
        InternalEvent_UpdateHoverOver += internalHoveredOverUpdate;
        InternalEvent_UpdateUnSelect += internalHoveredOverUpdate;
        InternalEvent_BeginSelected += internalSelectedBegin;
        InternalEvent_UpdateSelected += internalSelectedUpdate;

        ThisInit_Layer3();
    }
    protected virtual void ThisInit_Layer3() { }



    float current_optionswap_timer;


    public void ChangeValue(float new_value)
    {
        float realNewValue = Mathf.Clamp(new_value, startValue, endValue);
        sliderWeighting = (realNewValue - startValue) / (endValue - startValue);

        EventArgs_ValueChanged args = new EventArgs_ValueChanged(realNewValue);

        NoArgsActionWrapper<EventArgs_ValueChanged> EventWrapper_ValueChanged = new NoArgsActionWrapper<EventArgs_ValueChanged>(Event_ValueChanged, args);
        NoArgsActionWrapper<EventArgs_ValueChanged> InternalEventWrapper_ValueChanged = new NoArgsActionWrapper<EventArgs_ValueChanged>(InternalEvent_ValueChanged, args);

        _Transitioner.RequestNonTransitionCall(InternalEventWrapper_ValueChanged.wrappedAction, EventWrapper_ValueChanged.wrappedAction);
    }
    public void SetValue(float new_value)
    {
        float realNewValue = Mathf.Clamp(new_value, startValue, endValue);
        sliderWeighting = (realNewValue - startValue) / (endValue - startValue);

        EventArgs_ValueChanged args = new EventArgs_ValueChanged(realNewValue);

        NoArgsActionWrapper<EventArgs_ValueChanged> EventWrapper_ValueChanged = new NoArgsActionWrapper<EventArgs_ValueChanged>(Event_ValueSet, args);
        NoArgsActionWrapper<EventArgs_ValueChanged> InternalEventWrapper_ValueChanged = new NoArgsActionWrapper<EventArgs_ValueChanged>(InternalEvent_ValueSet, args);

        _Transitioner.RequestNonTransitionCall(InternalEventWrapper_ValueChanged.wrappedAction, EventWrapper_ValueChanged.wrappedAction);
    }

    public void ChangeSliderRange(Vector2 start_end)
    {
        startValue = start_end.x;
        endValue = start_end.y;
        SetValue(Value);
    }
    void internalHoveredOverBegin()
    {
        current_optionswap_timer = OPTION_SWAP_COOLDOWN;
        _Transitioner.RequestContinue(internalHoveredOverBegin);
    }
    void internalHoveredOverUpdate()
    {
        float horizontal = GM_.Instance.input.GetAxis(InputManager.AXIS.LH);
        float vertical = GM_.Instance.input.GetAxis(InputManager.AXIS.LV);


        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
            Select();
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
        current_optionswap_timer = SLIDER_VALUE_CHANGE_COOLDOWN;
        _Transitioner.RequestContinue(internalSelectedBegin);
    }
    void internalSelectedUpdate()
    {
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
        {
            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
            UnSelect();
            return;
        }


        switch (direction)
        {
            case DIRECTION.HORIZONTAL:
                {
                    float horizontal = GM_.Instance.input.GetAxis(InputManager.AXIS.LH);

                    if (horizontal > OPTION_SWAP_DEADZONE)
                    {
                        current_optionswap_timer -= Time.unscaledDeltaTime;

                        if (current_optionswap_timer < 0)
                        {
                            current_optionswap_timer = SLIDER_VALUE_CHANGE_COOLDOWN;
                 //           GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);

                            sliderWeighting = Mathf.Clamp01(sliderWeighting + sliderInvervals* horizontal);
                            ChangeValue(Mathf.Lerp(startValue, endValue, sliderWeighting));
                        }
                    }
                    else if (horizontal < -OPTION_SWAP_DEADZONE)
                    {
                        current_optionswap_timer -= Time.unscaledDeltaTime;

                        if (current_optionswap_timer < 0)
                        {
                            current_optionswap_timer = SLIDER_VALUE_CHANGE_COOLDOWN;
           //                 GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);

                            sliderWeighting = Mathf.Clamp01(sliderWeighting + sliderInvervals* horizontal);
                            ChangeValue(Mathf.Lerp(startValue, endValue, sliderWeighting));
                        }
                    }
                    else
                    {
                        current_optionswap_timer = -0.001f;
                    }
                    break;
                }
            case DIRECTION.VERTICAL:
                {
                    float vertical = GM_.Instance.input.GetAxis(InputManager.AXIS.LV);
                    if (vertical > OPTION_SWAP_DEADZONE)
                    {
                        current_optionswap_timer -= Time.unscaledDeltaTime;

                        if (current_optionswap_timer < 0)
                        {
                            current_optionswap_timer = SLIDER_VALUE_CHANGE_COOLDOWN;
           //                 GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);

                            sliderWeighting = Mathf.Clamp01(sliderWeighting + sliderInvervals);
                            ChangeValue(Mathf.Lerp(startValue, endValue, sliderWeighting));
                        }

                    }
                    else if (vertical < -OPTION_SWAP_DEADZONE)
                    {
                        current_optionswap_timer -= Time.unscaledDeltaTime;

                        if (current_optionswap_timer < 0)
                        {
                            current_optionswap_timer = SLIDER_VALUE_CHANGE_COOLDOWN;
          //                  GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);

                            sliderWeighting = Mathf.Clamp01(sliderWeighting - sliderInvervals);
                            ChangeValue(Mathf.Lerp(startValue, endValue, sliderWeighting));
                        }
                    }
                    else
                    {
                        current_optionswap_timer = -0.001f;
                    }
                    break;
                }
        }
    }

}
