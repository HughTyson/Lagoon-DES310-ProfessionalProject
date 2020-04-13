using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_OptionsControl : BasePagePair
{
    [SerializeField] UnselectableButton goBackButton;
    [SerializeField] UnselectableButton controlsButton;
    [SerializeField] UnselectableButton audioButton;

    [SerializeField] Slider_Default xSenseSlider;
    [SerializeField] Slider_Default ySenseSlider;
    [SerializeField] Checkbox_ xInvSlider;
    [SerializeField] Checkbox_ yInvSlider;
    [SerializeField] SelectableButton_TextButton back_SButton;


    [SerializeField] PagePair_OptionsGame gameOptionPair;
    [SerializeField] PagePair_OptionsAudio audioOptionPair;

    [SerializeField] BasePagePair goBackPair;

    void Awake()
    {

        back_SButton.Event_Selected += request_GoBack;

        controlsButton.Event_Selected += requestGoTo_GameOptions;
        audioButton.Event_Selected += requestGoTo_AudioOptions;

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        controlsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });
        audioButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });

        TypeRef<bool> grouper = new TypeRef<bool>();
        goBackButton.AssignToGroup(grouper);
        controlsButton.AssignToGroup(grouper);
        audioButton.AssignToGroup(grouper);


        xSenseSlider.Event_Selected += sliderSelected;
        ySenseSlider.Event_Selected += sliderSelected;

        xSenseSlider.Event_UnSelected += sliderUnSelected;
        ySenseSlider.Event_UnSelected += sliderUnSelected;



        xSenseSlider.Event_ValueChanged += changeXSense;
        ySenseSlider.Event_ValueChanged += changeYSense;
        xInvSlider.Event_ToggleChanged += toggledXInvert;
        yInvSlider.Event_ToggleChanged += toggledYInvert;

        xSenseSlider.ChangeSliderRange(PlayerSettings.MINMAX_X_SENSITIVITY);
        ySenseSlider.ChangeSliderRange(PlayerSettings.MINMAX_Y_SENSITIVITY);
    }

    public override void BegunEnteringPage()
    {


        goBackButton.Event_Selected += request_GoBack;

        xSenseSlider.SetValue(GM_.Instance.settings.XSensitivity);
        ySenseSlider.SetValue(GM_.Instance.settings.YSensitivity);
        xInvSlider.SetToggle(GM_.Instance.settings.IsXInverted);
        yInvSlider.SetToggle(GM_.Instance.settings.IsYInverted);


        goBackButton.Show();
        controlsButton.Show();
        audioButton.Show();
        xSenseSlider.Show();
        ySenseSlider.Show();
        xInvSlider.Show();
        yInvSlider.Show();
        back_SButton.Show();
    }
    public override void PassingBy()
    {
        goBackButton.Show();
        controlsButton.Show();
        audioButton.Show();
        xSenseSlider.Show();
        ySenseSlider.Show();
        xInvSlider.Show();
        yInvSlider.Show();
        back_SButton.Show();
    }


    void changeXSense(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.XSensitivity = args.newValue;
    }
    void changeYSense(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.YSensitivity = args.newValue;
    }

    void toggledXInvert(Checkbox_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.IsXInverted = args.newValue;
    }
    void toggledYInvert(Checkbox_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.IsYInverted = args.newValue;
    }


    public override void FinishedEnteringPage()
    {
        xSenseSlider.HoverOver();
    }


    void sliderSelected()
    {
        goBackButton.Event_Selected -= request_GoBack;
    }
    void sliderUnSelected()
    {
        goBackButton.Event_Selected += request_GoBack;
    }

    public override void BegunExitingPage()
    {
        goBackButton.Event_Selected -= request_GoBack;

        xSenseSlider.SafeUnHoverOver();
        ySenseSlider.SafeUnHoverOver();
        xInvSlider.SafeUnHoverOver();
        yInvSlider.SafeUnHoverOver();
        back_SButton.SafeUnHoverOver();
    }

    void requestGoTo_GameOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(gameOptionPair));
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
