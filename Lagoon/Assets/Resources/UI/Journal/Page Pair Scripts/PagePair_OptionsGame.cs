using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_OptionsGame : BasePagePair
{
    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableAndUnhoverableButton controlsButton;
    

    [SerializeField] Checkbox_ vibrationCheckbox;
    


    [SerializeField] PagePair_OptionsControl controlOptionPair;
    [SerializeField] PagePair_OptionsAudio audioOptionPair;

    [SerializeField] BasePagePair goBackPair;
    void Awake()
    {

        

        controlsButton.Event_Selected += requestGoTo_ControlOptions;
        

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        controlsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });
        

        TypeRef<bool> grouper = new TypeRef<bool>();
        goBackButton.GroupWith(controlsButton);
       

        vibrationCheckbox.Event_ToggleChanged += vibrationToggled;
    }


    void vibrationToggled(Checkbox_.EventArgs_ValueChanged args)
    {
        GM_.Instance.input.VibrationsEnabled = args.newValue;
    }

    public override void BegunEnteringPage()
    {
        vibrationCheckbox.SetToggle(GM_.Instance.input.VibrationsEnabled);


  

        goBackButton.Show();
        controlsButton.Show();
        
        vibrationCheckbox.Show();
        
    }



    public override void FinishedEnteringPage()
    {
        goBackButton.Event_Selected += request_GoBack;

        goBackButton.ListenForSelection();
        controlsButton.ListenForSelection();
       
        vibrationCheckbox.HoverOver();
    }


    public override void BegunExitingPage()
    {
        goBackButton.Event_Selected -= request_GoBack;

        vibrationCheckbox.SafeUnHoverOver();
        
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
