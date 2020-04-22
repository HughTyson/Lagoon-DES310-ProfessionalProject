using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_OptionsControl : BasePagePair
{
    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableAndUnhoverableButton controlsButton;


    [Header("Control Options")]
    [SerializeField] Slider_Default xSenseSlider;
    [SerializeField] Slider_Default ySenseSlider;
    [SerializeField] Checkbox_ xInvSlider;
    [SerializeField] Checkbox_ yInvSlider;

    [Header("Audio Options")]
    [SerializeField] Slider_Default masterSlider;
    [SerializeField] Slider_Default musicSlider;
    [SerializeField] Slider_Default sfxSlider;

    [Header("page Pairs")]
    [SerializeField] PagePair_OptionsGame gameOptionPair;

    [SerializeField] BasePagePair goBackPair;

    void Awake()
    {

      

        controlsButton.Event_Selected += requestGoTo_GameOptions;
      

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        controlsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });
       

        TypeRef<bool> grouper = new TypeRef<bool>();
        goBackButton.GroupWith(controlsButton);
        

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


        //Audio Options

        masterSlider.Event_ValueChanged += setMasterVolume;
        musicSlider.Event_ValueChanged += setMusicVolume;
        sfxSlider.Event_ValueChanged += setSFXVolume;

        masterSlider.Event_Selected += sliderSelected;
        musicSlider.Event_Selected += sliderSelected;
        sfxSlider.Event_Selected += sliderSelected;

        masterSlider.Event_UnSelected += sliderUnSelected;
        musicSlider.Event_UnSelected += sliderUnSelected;
        sfxSlider.Event_UnSelected += sliderUnSelected;
    }

    public override void BegunEnteringPage()
    {


        goBackButton.Event_Selected += request_GoBack;

        xSenseSlider.SetValue(GM_.Instance.settings.XSensitivity);
        ySenseSlider.SetValue(GM_.Instance.settings.YSensitivity);
        xInvSlider.SetToggle(GM_.Instance.settings.IsXInverted);
        yInvSlider.SetToggle(GM_.Instance.settings.IsYInverted);

        masterSlider.SetValue(GM_.Instance.audio.MasterVolume);
        musicSlider.SetValue(GM_.Instance.audio.MusicVolume);
        sfxSlider.SetValue(GM_.Instance.audio.SFXVolume);


        goBackButton.Show();
        controlsButton.Show();
       



        xSenseSlider.Show();
        ySenseSlider.Show();
        xInvSlider.Show();
        yInvSlider.Show();
        masterSlider.Show();
        musicSlider.Show();
        sfxSlider.Show();

    }
  
    public override void PassingBy()
    {
        goBackButton.Show();
        controlsButton.Show();
        
        xSenseSlider.Show();
        ySenseSlider.Show();
        xInvSlider.Show();
        yInvSlider.Show();

        masterSlider.SetValue(GM_.Instance.audio.MasterVolume);
        musicSlider.SetValue(GM_.Instance.audio.MusicVolume);
        sfxSlider.SetValue(GM_.Instance.audio.SFXVolume);

        masterSlider.Show();
        musicSlider.Show();
        sfxSlider.Show();

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
        goBackButton.ListenForSelection();
        controlsButton.ListenForSelection();
       
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

        masterSlider.SafeUnHoverOver();
        musicSlider.SafeUnHoverOver();
        sfxSlider.SafeUnHoverOver();

    }

    void requestGoTo_GameOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(gameOptionPair));
    }



    void request_GoBack()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(goBackPair));
    }

    void setMasterVolume(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.audio.MasterVolume = args.newValue;
    }
    void setMusicVolume(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.audio.MusicVolume = args.newValue;
    }
    void setSFXVolume(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.audio.SFXVolume = args.newValue;
    }

}
