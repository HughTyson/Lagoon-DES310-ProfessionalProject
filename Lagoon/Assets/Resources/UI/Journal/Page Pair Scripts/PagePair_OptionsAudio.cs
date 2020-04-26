using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_OptionsAudio : BasePagePair
{
    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableAndUnhoverableButton controlsButton;
    [SerializeField] SelectableAndUnhoverableButton gameButton;


    [SerializeField] Slider_Default masterSlider;
    [SerializeField] Slider_Default musicSlider;
    [SerializeField] Slider_Default sfxSlider;

    [SerializeField] SelectableButton_TextButton back_SButton;

    [SerializeField] PagePair_OptionsControl controlOptionPair;
    [SerializeField] PagePair_OptionsGame gameOptionPair;

    [SerializeField] BasePagePair goBackPair;

    private void Awake()
    {
        back_SButton.Event_Selected += request_GoBack;

        controlsButton.Event_Selected += requestGoTo_ControlOptions;
        gameButton.Event_Selected += requestGoTo_GameOptions;

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        controlsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });
        gameButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });


        goBackButton.GroupWith(controlsButton);
        controlsButton.GroupWith(gameButton);


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
    public override void BegunEnteringPage()
    {
        masterSlider.SetValue(GM_.Instance.audio.MasterVolume);
        musicSlider.SetValue(GM_.Instance.audio.MusicVolume);
        sfxSlider.SetValue(GM_.Instance.audio.SFXVolume);



        goBackButton.Show();
        controlsButton.Show();
        gameButton.Show();
        masterSlider.Show();
        musicSlider.Show();
        sfxSlider.Show();
        back_SButton.Show();



    }
    public override void PassingBy()
    {
        masterSlider.SetValue(GM_.Instance.audio.MasterVolume);
        musicSlider.SetValue(GM_.Instance.audio.MusicVolume);
        sfxSlider.SetValue(GM_.Instance.audio.SFXVolume);

        goBackButton.Show();
        controlsButton.Show();
        gameButton.Show();
        masterSlider.Show();
        musicSlider.Show();
        sfxSlider.Show();
        back_SButton.Show();
    }

    public override void FinishedEnteringPage()
    {
        goBackButton.Event_Selected += request_GoBack;
        goBackButton.ListenForSelection();
        controlsButton.ListenForSelection();
        gameButton.ListenForSelection();

        masterSlider.HoverOver();
    }

    void sliderSelected()
    {
        goBackButton.Event_Selected -= request_GoBack;
    }
    void sliderUnSelected()
    {
        goBackButton.Event_Selected += request_GoBack;
        goBackButton.ListenForSelection();
    }

    public override void BegunExitingPage()
    {
        goBackButton.Event_Selected -= request_GoBack;

        masterSlider.SafeUnHoverOver();
        musicSlider.SafeUnHoverOver();
        sfxSlider.SafeUnHoverOver();
        back_SButton.SafeUnHoverOver();
    }

    void requestGoTo_GameOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(gameOptionPair));
    }
    void requestGoTo_ControlOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(controlOptionPair));
    }


    void request_GoBack()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(goBackPair));
    }




}
