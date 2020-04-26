using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableAndUnhoverableButton : SelectableAndUnhoverable_
{
    private void Awake()
    {
        Init();
    }

    AudioManager.SFXArgs sfxArgs_select;


    protected sealed override void ThisInit_Layer2()
    {

        sfxArgs_select = new AudioManager.SFXArgs
        (
            GM_.Instance.audio.GetSFX("UI_ButtonSelect"),
           null,
           IsMenuSound_: true
        );

        InternalEvent_BeginListeningForSelectionAction += beginCheckForButtons;
        InternalEvent_UpdateListeningForSelectionAction += internal_updateListeningForSelection_checkButtons;
        InternalEvent_BeginSelected += beginSelected;
        InternalEvent_EndSelected += endSelected;


        InternalEventQuery_BlockOtherItemInGroupRequest += blockingRequest;


        ThisInit_Layer3();
    }
    protected virtual void ThisInit_Layer3() { }

    float current_optionswap_timer;

    InputManager.BUTTON[] buttonsToCheck = new InputManager.BUTTON[0];

    bool isSelected = false;
    bool isListening = false;
    
    void blockingRequest(BlockRequestArgs args)
    {
        if (args.cmdType.GetType() == typeof(CMD_Select))
        {
            if (isSelected)
                args.Block();
        }

    }




    void beginSelected()
    {
        isSelected = true;
        isListening = false;
        GM_.Instance.audio.PlaySFX(sfxArgs_select);
        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
        _Transitioner.RequestContinue(beginSelected);
    }
    void endSelected()
    {
        isSelected = false;
        _Transitioner.RequestContinue(endSelected);
    }

    public void SetButtonsToCheckForPress(InputManager.BUTTON[] buttons)
    {
        buttonsToCheck = buttons;
    }


    protected override void InteruptedShow(InteruptArgs args, InteruptReturn returns)
    {
        if (args.interuptedBy == typeof(CMD_ListeningForSelection))
        {
            returns.interuptResolution = InteruptReturn.INTERUPT_RESOLUTION.QUEUE;
        }
    }
    void beginCheckForButtons()
    {
       // current_optionswap_timer = 0.3f;
        isListening = true;
        isSelected = false;
        _Transitioner.RequestContinue(beginCheckForButtons);

    }
    void internal_updateListeningForSelection_checkButtons()
    {
   //     current_optionswap_timer -= Time.deltaTime;

    //    if (current_optionswap_timer <= 0)
    //    {
            for (int i = 0; i < buttonsToCheck.Length; i++)
            {
                if (GM_.Instance.input.GetButtonDown(buttonsToCheck[i]))
                {
                    Select();
                    return;
                }
            }
    //    }
    }

}
