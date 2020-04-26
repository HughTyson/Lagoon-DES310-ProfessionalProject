using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableButton_ : Selectable_
{
    private void Awake()
    {
        Init();
    }



    protected sealed override void ThisInit_Layer2()
    {


        InternalEvent_BeginHoverOver += internalHoveredOverBegin;
        InternalEvent_UpdateHoverOver += internalHoveredOverUpdate;
        ThisInit_Layer3();
    }
    protected virtual void ThisInit_Layer3(){}




    float current_optionswap_timer;

    void internalHoveredOverBegin()
    {
        current_optionswap_timer = OPTION_SWAP_COOLDOWN;
        _Transitioner.RequestContinue(internalHoveredOverBegin);
    }
    void internalHoveredOverUpdate()
    {

        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);

            UnHoverOver();
            Select();
            return;
        }

        float horizontal = GM_.Instance.input.GetAxis(InputManager.AXIS.LH);
        float vertical = GM_.Instance.input.GetAxis(InputManager.AXIS.LV);


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
                        UnHoverOver();
                        siblingUp.HoverOver();
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

                        UnHoverOver();
                        siblingDown.HoverOver();
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

                        UnHoverOver();
                        siblingRight.HoverOver();
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

                        UnHoverOver();
                        siblingLeft.HoverOver();
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
