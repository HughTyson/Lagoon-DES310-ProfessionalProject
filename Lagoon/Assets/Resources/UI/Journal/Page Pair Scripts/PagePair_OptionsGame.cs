using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_OptionsGame : BasePagePair
{
    [SerializeField] UnselectableButton goBackButton;
    [SerializeField] UnselectableButton controlsButton;
    [SerializeField] UnselectableButton audioButton;

    [SerializeField] Checkbox_ vibrationCheckbox;
    [SerializeField] SelectableButton_TextButton back_SButton;


    [SerializeField] PagePair_OptionsControl controlOptionPair;
    [SerializeField] PagePair_OptionsAudio audioOptionPair;

    [SerializeField] BasePagePair goBackPair;
    void Awake()
    {

        back_SButton.Event_Selected += request_GoBack;

        controlsButton.Event_Selected += requestGoTo_ControlOptions;
        audioButton.Event_Selected += requestGoTo_AudioOptions;

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        controlsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });
        audioButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });

        TypeRef<bool> grouper = new TypeRef<bool>();
        goBackButton.AssignToGroup(grouper);
        controlsButton.AssignToGroup(grouper);
        audioButton.AssignToGroup(grouper);

        vibrationCheckbox.Event_ToggleChanged += vibrationToggled;
    }


    void vibrationToggled(Checkbox_.EventArgs_ValueChanged args)
    {
        GM_.Instance.input.VibrationsEnabled = args.newValue;
    }

    public override void BegunEnteringPage()
    {
        vibrationCheckbox.SetToggle(GM_.Instance.input.VibrationsEnabled);


        goBackButton.Event_Selected += request_GoBack;

        goBackButton.Show();
        controlsButton.Show();
        audioButton.Show();
        vibrationCheckbox.Show();
        back_SButton.Show();
    }



    public override void FinishedEnteringPage()
    {
        vibrationCheckbox.HoverOver();
    }


    public override void BegunExitingPage()
    {
        goBackButton.Event_Selected -= request_GoBack;

        vibrationCheckbox.SafeUnHoverOver();
        back_SButton.SafeUnHoverOver();
    }


    void requestGoTo_ControlOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(controlOptionPair));
    }
    void requestGoTo_AudioOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(audioOptionPair));
    }


    void request_GoBack()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(goBackPair));
    }
}
